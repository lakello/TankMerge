using R3;
using R3.Triggers;

namespace UtilsModule.Other
{
	public abstract class ObservableSubjectTrigger<T> : ObservableTriggerBase
	{
		private Subject<T> _subject;

		protected Observable<T> OnSubjectAsObservable()
		{
			return _subject ??= new Subject<T>();
		}

		protected void OnNext(T message)
		{
			_subject?.OnNext(message);
		}

		protected override void RaiseOnCompletedOnDestroy()
		{
			_subject?.OnCompleted();
		}
	}
}