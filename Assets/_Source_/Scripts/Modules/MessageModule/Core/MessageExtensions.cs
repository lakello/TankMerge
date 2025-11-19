namespace MessageModule
{
	using _GameResources.Scripts.Modules.UtilsModule.Core.Log;
	using R3;
	using UnityEngine.Scripting;

	public static partial class MessageExtensions
	{
		private static DebugLogger _logger = new DebugLogger(nameof(MessageBroker));

		[Preserve]
		static MessageExtensions()
		{
		}

		public static void Publish<TMessage>()
			where TMessage : IMessage, new()
		{
			new TMessage().Publish();
		}

		public static void Publish<TMessage>(this TMessage message)
			where TMessage : IMessage
		{
			message.Broker.Publish(message);
		}

		public static Observable<TMessage> Receive<TMessage>()
			where TMessage : IMessage, new()
		{
			return new TMessage().Receive();
		}

		public static Observable<TMessage> Receive<TMessage>(this TMessage message)
			where TMessage : IMessage
		{
			return message.Broker.Receive<TMessage>();
		}
	}
}