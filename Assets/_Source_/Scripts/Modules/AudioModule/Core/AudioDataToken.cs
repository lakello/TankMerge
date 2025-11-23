using UnityEngine;

namespace AudioModule
{
	public readonly struct AudioDataToken
	{
		private readonly AudioDataHolder _holder;
		private readonly int _id;
		
		public AudioDataToken(AudioDataHolder holder)
		{
			_holder = holder;
			_id = holder.ID;
		}
		
		public AudioSource Source => Cancelled ? null : _holder.Source;
		public AudioSettings.AudioData? AudioData => Cancelled ? null : _holder.AudioData;
		public bool Cancelled => CheckCancelled();

		private bool CheckCancelled()
		{
			return (_holder.ID == _id && _holder.IsPlaying) == false;
		}
	}
}