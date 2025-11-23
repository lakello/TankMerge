namespace AudioModule.Messages
{
	using MessageModule;

	public struct M_PlayLoopClipByType : IMessage
	{
		public AudioMessageData MessageData;

		public IMessageBroker Broker => AudioService.Broker;
	}
}