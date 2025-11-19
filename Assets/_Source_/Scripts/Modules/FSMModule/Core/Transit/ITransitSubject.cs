using System;

namespace FSMModule.Transit
{
	public interface ITransitSubject
	{
		public event Action StateTransiting;
	}
}