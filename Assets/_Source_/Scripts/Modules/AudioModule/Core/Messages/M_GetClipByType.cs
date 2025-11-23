namespace AudioModule.Messages
{
	using System;
	using MessageModule;
	using UnityEngine;

	public struct M_GetClipByType : IMessage
	{
		public AudioID ID;
		public Action<AudioClip> Action;

		public IMessageBroker Broker => AudioService.Broker;
	}
}