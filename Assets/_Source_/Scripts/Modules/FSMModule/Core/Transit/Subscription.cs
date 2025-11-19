using System;

namespace FSMModule.Transit
{
	public struct Subscription
	{
		public ITransitSubject TransitSubject { get; private set; }

		public Action Observer { get; private set; }

		public Subscription(ITransitSubject transitSubject, Action observer)
		{
			TransitSubject = transitSubject;
			Observer = observer;
		}
	}
}