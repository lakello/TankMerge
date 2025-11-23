namespace AudioModule.Extensions
{
	using MessageModule;
	using Messages;

	public static class AudioExtensions
	{
		public static AudioMessageData ToMessageData(this AudioID audioID)
		{
			return new AudioMessageData
			{
				ID = audioID
			};
		}

		public static void PlayOneShot(this AudioID audioID)
		{
			audioID.ToMessageData().PlayOneShot();
		}

		public static void PlayOneShot(this AudioMessageData messageData)
		{
			new M_PlayClipByType
			{
				MessageData = messageData
			}.Publish();
		}

		public static void PlayBackground(this AudioID audioID)
		{
			audioID.ToMessageData().PlayBackground();
		}

		public static void PlayBackground(this AudioMessageData messageData)
		{
			new M_PlayBackground
			{
				MessageData = messageData
			}.Publish();
		}
	}
}