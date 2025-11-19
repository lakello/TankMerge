using System;

namespace UtilsModule.Disposable
{
	public static class DisposableExtension
	{
		public static void DisposeOnQuitGame(this IDisposable disposable)
		{
			GlobalDisposableHolder.Register(disposable);
		}
		
		public static void DisposeOnExitScene(this IDisposable disposable, string sceneName)
		{ 
			SceneDisposableHolder.Register(disposable, sceneName);
		}
	}
}