namespace MiniIT.GAME.PLAYER
{
    using BATTLE;
    using MessageModule;

    public struct DamageTakedMessage : IMessage
    {
        public Fraction SelfFraction;
        public bool     IsAlive;
        public int      TakeDamageReward;
        public int      DeadReward;

        public IMessageBroker Broker => MessageBrokerHolder.Game;
    }
}