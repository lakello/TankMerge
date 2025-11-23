namespace MiniIT.GAME.MERGE
{
    using MessageModule;

    public class MergeSuccessMessage : IMessage
    {
        public GridCell TargetCell;
        public int      Level;

        public IMessageBroker Broker => MessageBrokerHolder.Game;
    }
}