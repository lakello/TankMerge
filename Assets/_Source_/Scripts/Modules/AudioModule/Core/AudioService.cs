namespace AudioModule
{
	using System;
	using System.Collections.Generic;
	using AssetLoadModule;
	using Cysharp.Threading.Tasks;
	using MessageModule;
	using Messages;
	using R3;
	using UnityEngine;
	using UnityEngine.AddressableAssets;
	using UnityEngine.Pool;
	using UtilsModule.Disposable;
	using Object = UnityEngine.Object;

	public class AudioService : IDisposable
	{
		private readonly Dictionary<AudioID, AudioDataHolder> _loopSources = new();
		private readonly CompositeDisposable _disposable;
		private readonly ObjectPool<AudioSource> _pointAudioPool;
		private readonly ObjectPool<AudioDataHolder> _audioPool;
		private readonly ObjectPool<AudioSource> _oneShotPool;

		private Dictionary<AudioID, AudioSettings.AudioData> _audioClips;
		private bool _isInitialized;
		
		public static IMessageBroker Broker { get; } = new MessageBroker();

		public AudioService(AssetReference audioDataReference, AudioSource pointAudioPrefab, AudioSource audioPrefab)
		{
			var poolHolder = new GameObject("Pool Holder");
			Object.DontDestroyOnLoad(poolHolder);

			_pointAudioPool = new ObjectPool<AudioSource>(() => Object.Instantiate(pointAudioPrefab, poolHolder.transform));
			_audioPool = new ObjectPool<AudioDataHolder>(
				() => new AudioDataHolder(Object.Instantiate(audioPrefab, poolHolder.transform)));
			_oneShotPool = new ObjectPool<AudioSource>(() => Object.Instantiate(audioPrefab, poolHolder.transform));

			var backgroundSource = new GameObject("BackgroundSource").AddComponent<AudioSource>();
			var clipSource = new GameObject("ClipSource").AddComponent<AudioSource>();

			Object.DontDestroyOnLoad(backgroundSource);
			Object.DontDestroyOnLoad(clipSource);

			backgroundSource.loop = true;

			_disposable = new CompositeDisposable();

			new M_PlayClip().Receive()
				.Subscribe(m => clipSource.PlayOneShot(m.Clip))
				.AddTo(_disposable);

			new M_PlayClipByType().Receive()
				.Subscribe(m => DoWithAudioType(m.MessageData, data =>
				{
					if (m.MessageData.UsePool)
					{
						var source = _oneShotPool.Get();
						source.PlayOneShot(data.Clip);
						source.volume = data.Volume;
						ReleaseSource(source, _oneShotPool, data.Clip.length).Forget();
					}
					else
					{
						clipSource.PlayOneShot(data.Clip);
						clipSource.volume = data.Volume;
					}
				}).Forget())
				.AddTo(_disposable);

			new M_PlayClipByTypeInPoint().Receive()
				.Subscribe(m =>
				{
					var source = _pointAudioPool.Get();
					source.transform.position = m.Position;
					DoWithAudioType(m.MessageData, data =>
					{
						source.PlayOneShot(data.Clip);
						source.volume = data.Volume;
						ReleaseSource(source, _pointAudioPool, data.Clip.length).Forget();
					}).Forget();
				})
				.AddTo(_disposable);
			
			new M_GetClipByType().Receive()
				.Subscribe(m => DoWithAudioType(
					new AudioMessageData
					{
						ID = m.ID
					},
					data => m.Action(data.Clip)).Forget())
				.AddTo(_disposable);

			new M_PlayBackground().Receive()
				.Subscribe(m =>
				{
					DoWithAudioType(
						m.MessageData,
						data =>
						{
							if (backgroundSource.clip != data.Clip)
							{
								backgroundSource.Stop();
								backgroundSource.clip = data.Clip;
								backgroundSource.volume = data.Volume;
								backgroundSource.Play();
							}
						}).Forget();
				})
				.AddTo(_disposable);

			new M_PlayLoopClipByType().Receive()
				.Subscribe(m => PlayLoop(m.MessageData))
				.AddTo(_disposable);

			new M_StopLoopClipByType().Receive()
				.Subscribe(m => StopLoop(m.MessageData))
				.AddTo(_disposable);

			new M_GetLoopAudioDataToken().Receive()
				.Subscribe(m =>
				{
					if (_loopSources.TryGetValue(m.ID, out var holder))
					{
						m.GiveToken(holder.GetToken);
					}
				})
				.AddTo(_disposable);

			LoadData(audioDataReference).Forget();

			GlobalDisposableHolder.Register(this);
		}

		public void Dispose()
		{
			_disposable.Dispose();
		}

		private void PlayLoop(AudioMessageData messageData)
		{
			DoWithAudioType(
				messageData,
				data =>
				{
					if (_loopSources.TryGetValue(messageData.ID, out var holder) == false)
					{
						holder = _audioPool.Get();
						_loopSources.Add(messageData.ID, holder);
					}

					holder.Stop();

					holder.Source.loop = true;
					holder.Source.clip = data.Clip;
					holder.Source.volume = data.Volume;
					holder.Source.gameObject.name = messageData.ID.ToString();

					holder.Play(data);
				}).Forget();
		}

		private void StopLoop(AudioMessageData messageData)
		{
			DoWithAudioType(
				messageData,
				_ =>
				{
					if (_loopSources.TryGetValue(messageData.ID, out var source))
					{
						source.Stop();
						source.Source.loop = false;
						source.Source.clip = null;
						source.Source.gameObject.name = nameof(AudioSource);

						_audioPool.Release(source);
						_loopSources.Remove(messageData.ID);
					}
				}).Forget();
		}

		private async UniTaskVoid ReleaseSource<T>(T source, ObjectPool<T> pool, float time)
			where T : class
		{
			await UniTask.Delay(TimeSpan.FromSeconds(time));
			pool.Release(source);
		}

		private async UniTaskVoid LoadData(AssetReference audioDataReference)
		{
			await AssetLoadHelper.Load<AudioSettings>(
				audioDataReference,
				result => _audioClips = result.GetData());

			_isInitialized = true;
		}

		private async UniTaskVoid DoWithAudioType(AudioMessageData messageData, Action<AudioSettings.AudioData> action)
		{
			if (messageData.WaitInit && _isInitialized == false)
			{
				await UniTask.WaitUntil(() => _isInitialized);
			}

			if (_isInitialized == false)
			{
				return;
			}

			if (_audioClips == null
				|| _audioClips.TryGetValue(messageData.ID, out var data) == false)
			{
				return;
			}

			action(data);
		}
	}
}