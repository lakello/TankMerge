#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace FSMModule.Debug
{
	public class StateMachineDebuggerWindow : EditorWindow
	{
		private Vector3 _scrollPosition;

		[MenuItem("Window/Analysis/State Machine Debugger")]
		public static void ShowWindow()
		{
			GetWindow<StateMachineDebuggerWindow>("State Machine Debugger");
		}

		private void OnGUI()
		{
			if (EditorApplication.isPlaying == false)
			{
				EditorGUILayout.HelpBox("Only Runtime", MessageType.Warning);

				return;
			}

			if (StateMachineInstancesContainer.Machines is { Count: < 1 })
				return;

			_scrollPosition = EditorGUILayout.BeginScrollView(
				_scrollPosition,
				false,
				true);

			StateMachineInstancesContainer.Machines.ForEach(machine =>
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(machine.MachineName);
				GUILayout.Label(machine.StateName);
				GUILayout.EndHorizontal();
			});

			EditorGUILayout.EndScrollView();
		}

		private void OnInspectorUpdate()
		{
			Repaint();
		}
	}
}
#endif