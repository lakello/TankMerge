namespace AudioModule.Messages
{
	using MessageModule;

	public struct M_PlayBackground : IMessage
	{
		public AudioMessageData MessageData;
		
		public IMessageBroker Broker => AudioService.Broker;
	}
}