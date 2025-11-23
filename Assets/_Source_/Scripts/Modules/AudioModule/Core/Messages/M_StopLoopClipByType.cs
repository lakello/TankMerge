namespace AudioModule.Messages
{
	using MessageModule;

	public struct M_StopLoopClipByType : IMessage
	{
		public AudioMessageData MessageData;

		public IMessageBroker Broker => AudioService.Broker;
	}
}