namespace UtilsModule.Other
{
	using System.Threading;
	using Cysharp.Threading.Tasks;
	using UnityEngine;

	public class OrderButtonHandler : ButtonHandler
	{
		[SerializeField]
		[SerializeReference]
		private IOrderButtonHandler[] _orderButtonHandlers;
		
		private CancellationTokenSource _tokenSource;
		
		protected override void OnClick()
		{
			if (_tokenSource != null)
			{
				return;
			}
			
			_tokenSource = new  CancellationTokenSource();
			Execute(_tokenSource.Token).Forget();
		}

		private async UniTask Execute(CancellationToken token)
		{
			foreach (var handler in _orderButtonHandlers)
			{
				token.ThrowIfCancellationRequested();
				
				await handler.Execute(token);
			}
		}
	}
}