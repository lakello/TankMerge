namespace AudioModule.Messages
{
	using MessageModule;
	using UnityEngine;

	public struct M_PlayClipByTypeInPoint : IMessage
	{
		public AudioMessageData MessageData;
		public Vector3 Position;
		
		public IMessageBroker Broker => AudioService.Broker;
	}
}