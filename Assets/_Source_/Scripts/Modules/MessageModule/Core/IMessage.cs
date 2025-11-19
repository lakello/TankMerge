namespace MessageModule
{
	public interface IMessage
	{
		public IMessageBroker Broker { get; }
	}
}