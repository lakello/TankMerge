namespace MiniIT.GAME
{
    using MessageModule;

    public static class MessageBrokerHolder
    {
        public static IMessageBroker Game { get; } = new MessageBroker();
        public static IMessageBroker Battle { get; } = new MessageBroker();
    }
}