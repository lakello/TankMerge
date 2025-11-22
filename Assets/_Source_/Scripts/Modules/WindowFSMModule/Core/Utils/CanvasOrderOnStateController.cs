using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UtilsModule.Other;
using ZLinq;

namespace WindowFSMModule.Utils
{
	[RequireComponent(typeof(Canvas))]
	public class CanvasOrderOnStateController : MonoBehaviour
	{
		[SerializeField]
		[TableList(DrawScrollView = true, MaxScrollViewHeight = 200, MinScrollViewHeight = 100)]
		private List<OrderData> _ordersData;
		
		private Dictionary<string, int> _orders;
		private Canvas _canvas;
		private WindowStateMachine _windowStateMachine;
		
		private void Awake()
		{
			_canvas = GetComponent<Canvas>();
			_windowStateMachine = GlobalData.Container.Resolve<WindowStateMachine>();
			_orders = _ordersData.AsValueEnumerable().ToDictionary(o => o.State.Name, o => o.Order);
		}

		private void OnEnable()
		{
			_windowStateMachine.StateChanged += OnStateChanged;
		}

		private void OnStateChanged()
		{
			_canvas.sortingOrder = _orders.GetValueOrDefault(_windowStateMachine.CurrentState.Name, 0);
		}
		
		[Serializable]
		private struct OrderData
		{
			public ValidWindowState State;
			public int Order;
		}
	}
}