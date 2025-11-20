namespace miniit.GAME
{
	using MessageModule;

	public static class GameMessageBrokerHolder
	{
		public static IMessageBroker Broker { get; } = new MessageBroker();
	}
}