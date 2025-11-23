namespace AudioModule.Messages
{
	using System;
	using MessageModule;

	public struct M_GetLoopAudioDataToken : IMessage
	{
		public AudioID ID;
		public Action<AudioDataToken?> GiveToken;
		
		public IMessageBroker Broker => AudioService.Broker;
	}
}