using UnityEngine;

namespace UtilsModule.Extensions
{
	public static class ColliderExtensions
	{
		public static bool IsPlayer(this Collider collider)
		{
			return collider.gameObject.layer == LayerMask.NameToLayer("Player");
		}
	}
}