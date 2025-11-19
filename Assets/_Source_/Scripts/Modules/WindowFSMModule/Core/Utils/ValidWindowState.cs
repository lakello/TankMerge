using System;
using System.Linq;
using System.Reflection;
using FSMModule;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace WindowFSMModule.Utils
{
	[Serializable]
	public partial struct ValidWindowState
	{
		[ValueDropdown("GetStates")]
		[SerializeField]
		private string _stateName;

		public string Name => _stateName;
		
		public bool IsValid<T>(IStateChangeable<T> stateChangeable)
			where T : StateMachine<T>
		{
			if (_stateName == stateChangeable.CurrentState.Name)
			{
				return true;
			}

			return false;
		}

		public static implicit operator string(ValidWindowState windowState) => windowState._stateName;
	}

	public static class ValidWindowStatesExtensions
	{
		public static bool IsValidArray<T>(this ValidWindowState[] windowStates, IStateChangeable<T> stateChangeable)
			where T : StateMachine<T>
		{
			foreach (var state in windowStates)
			{
				if (state.IsValid(stateChangeable))
				{
					return true;
				}
			}
			
			return false;
		}
	}

#if UNITY_EDITOR
	public partial struct ValidWindowState
	{
		private ValueDropdownList<string> GetStates()
		{
			var types = GetTypes(typeof(WindowState));

			var list = new ValueDropdownList<string>();
			types.ForEach(t => list.Add(t.Name, t.Name));

			return list;
		}

		private Type[] GetTypes(Type baseType)
		{
			return Assembly.GetAssembly(baseType).GetTypes()
				.Where(type => type.IsAbstract == false && (baseType.IsAssignableFrom(type) || type == baseType))
				.ToArray();
		}
	}
#endif
}