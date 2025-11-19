namespace MessageModule
{
	public static class NetworkMessage
	{
		public static IMessageBroker Broker {get; } = new MessageBroker();
	}
}