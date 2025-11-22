namespace MiniIT.GAME.PLAYER.Messages
{
    using Battle;
    using MessageModule;

    public struct DamageTakedMessage : IMessage
    {
        public Fraction SelfFraction;
        public bool IsAlive;
        public int TakeDamageReward;
        public int DeadReward;
        
        public IMessageBroker Broker => MessageBrokerHolder.Game;
    }
}