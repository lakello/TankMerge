namespace AudioModule.Messages
{
	using MessageModule;

	public struct M_PlayClipByType : IMessage
	{
		public AudioMessageData MessageData;
		
		public IMessageBroker Broker => AudioService.Broker;
	}
}