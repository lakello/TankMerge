namespace AudioModule.Messages
{
	using MessageModule;
	using UnityEngine;

	public struct M_PlayClip : IMessage
	{
		public AudioClip Clip;
		
		public IMessageBroker Broker => AudioService.Broker;
	}
}