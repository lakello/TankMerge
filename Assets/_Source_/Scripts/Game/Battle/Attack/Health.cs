namespace MiniIT.GAME.Battle
{
	using R3;
	using UnityEngine;

	public class Health : MonoBehaviour
	{
		[SerializeField]
		private float maxHealth;
		
		public bool IsAlive => Current is { Value: > 0, };
		public bool IsMax { get; private set; }
		public ReactiveProperty<float> Current { get; } = new ReactiveProperty<float>(1);

		public void Init(float healthMultiplier)
		{
			Current.Value = maxHealth * healthMultiplier;
			IsMax = true;
		}

		public void TakeDamage(float damage)
		{
			IsMax = false;
			Current.Value -= damage;
		}
	}
}