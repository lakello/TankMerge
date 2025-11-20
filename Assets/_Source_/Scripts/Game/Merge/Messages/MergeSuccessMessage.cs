namespace miniit.GAME.MERGE.Messages
{
	using CELL;
	using MessageModule;

	public class MergeSuccessMessage : IMessage
	{
		public GridCell TargetCell;
		public int Level;
		
		public IMessageBroker Broker => GameMessageBrokerHolder.Broker;
	}
}