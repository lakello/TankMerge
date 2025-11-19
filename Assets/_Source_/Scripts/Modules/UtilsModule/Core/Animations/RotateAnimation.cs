using UnityEngine;

namespace UtilsModule.Animations
{
	public class RotateAnimation : MonoBehaviour
	{
		[SerializeField]
		private Vector2 _speedRange;
		[SerializeField]
		private Direction _direction = Direction.Random;
		[SerializeField]
		private Vector3 _axis = Vector3.up;

		private float _speed;
		
		private void Awake()
		{
			if (_direction == Direction.Random)
			{
				_direction = Random.Range(0, 2) == 0 ? Direction.Left : Direction.Right;
			}
			
			_speed = Random.Range(_speedRange.x, _speedRange.y);
		}

		private void Update()
		{
			transform.Rotate(_axis, _speed * Time.deltaTime);
		}

		private enum Direction
		{
			Left = -1,
			Random = 0,
			Right = 1
		}
	}
}