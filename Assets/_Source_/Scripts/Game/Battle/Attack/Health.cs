namespace MiniIT.GAME.Battle
{
	using R3;
	using UnityEngine;

	public class Health : MonoBehaviour
	{
		[SerializeField]
		private float maxHealth;
		
		public bool IsAlive => Current is { Value: > 0, };
		public ReactiveProperty<float> Current { get; private set; } = new ReactiveProperty<float>(1);

		public void Init(float healthMultiplier)
		{
			Current.Value = maxHealth * healthMultiplier;
		}

		public void TakeDamage(float damage)
		{
			Current.Value -= damage;
		}
	}
}