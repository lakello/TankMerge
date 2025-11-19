using System;
using Cysharp.Threading.Tasks;

namespace UtilsModule.Other
{

	public class LazyProperty<T>
	{
		private readonly Func<T> _initializer;
		
		private T _value;

		public LazyProperty(Func<T> initializer)
		{
			_initializer = initializer;
		}
		
		public T Value => _value ??= _initializer();
		public bool IsInitialized => Value != null;
		
		public static implicit operator T(LazyProperty<T> lazyProperty) => lazyProperty.Value;
	}

	public class LazyPropertyAsync<T>
	{
		private readonly Func<UniTask<T>> _initializer;
		
		private T _value;

		public LazyPropertyAsync(Func<UniTask<T>> initializer)
		{
			_initializer = initializer;
		}
		
		public bool IsInitialized => _value != null;
		public T Value => _value;

		public async UniTask Init()
		{
			if (_value == null)
			{
				_value = await _initializer();
			}
		}
		
		public async UniTask<T> GetValueAsync()
		{
			if (_value == null)
			{
				_value = await _initializer();
			}
			
			return _value;
		}
	}

	public class LazyPropertyStruct<T>
		where T : struct
	{
		private readonly Func<T> _initializer;
		
		private T _value;

		public LazyPropertyStruct(Func<T> initializer)
		{
			_initializer = initializer;
		}
		
		public T Value => IsInitialized ? _value : Initialize();
		public bool IsInitialized { get; private set; }

		public static implicit operator T(LazyPropertyStruct<T> lazyProperty) => lazyProperty.Value;

		private T Initialize()
		{
			_value = _initializer();
			IsInitialized = true;
			return _value;
		}
	}
}