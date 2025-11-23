using UnityEngine;

namespace AudioModule
{
	public class AudioDataHolder
	{
		private int _id;

		public AudioDataHolder(AudioSource source)
		{
			Source = source;
			IsPlaying = true;
		}

		public bool IsPlaying { get; private set; }
		public AudioDataToken? GetToken => IsPlaying ? new AudioDataToken(this) : null;
		public AudioSource Source { get; }
		public AudioSettings.AudioData AudioData { get; private set; }

		public int ID => IsPlaying ? _id : -1;

		public void Play(AudioSettings.AudioData audioData)
		{
			AudioData = audioData;
			IsPlaying = true;
			Source.Play();
			_id++;
		}

		public void Stop()
		{
			IsPlaying = false;
			Source.Stop();
		}
	}
}