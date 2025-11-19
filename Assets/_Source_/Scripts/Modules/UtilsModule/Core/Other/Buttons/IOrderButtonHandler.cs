namespace UtilsModule.Other
{
	using System.Threading;
	using Cysharp.Threading.Tasks;

	public interface IOrderButtonHandler
	{
		public UniTask Execute(CancellationToken token);
	}
}