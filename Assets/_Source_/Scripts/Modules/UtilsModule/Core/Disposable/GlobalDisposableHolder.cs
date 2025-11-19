using System;
using System.Collections.Generic;
using UtilsModule.Singleton;

namespace UtilsModule.Disposable
{
	public class GlobalDisposableHolder : SingletonMono<GlobalDisposableHolder>
	{
		private readonly List<IDisposable> _disposables = new List<IDisposable>();
		
		private void OnDestroy() =>
			_disposables.ForEach(disposable => disposable?.Dispose());

		public static void Create()
		{
			Create(SingletonMode.Global);
		}
		
		public static void Register(IDisposable disposable) =>
			Instance._disposables.Add(disposable);
	}
}