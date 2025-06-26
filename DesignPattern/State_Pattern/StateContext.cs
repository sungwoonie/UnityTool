using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarCloudgamesLibrary
{
	public class StateContext<T> 
	{
		public IState<T> CurrentState
		{
			get; set;
		}

		private readonly T controller;

		public StateContext(T _controller)
		{
			controller = _controller;
		}

		public void Transition()
		{
			CurrentState.Handle(controller);
		}

		public void Transition(IState<T> state)
		{
			if(CurrentState != null)
			{
				CurrentState.StopFunction();
			}
			CurrentState = state;
			CurrentState.Handle(controller);
			DebugManager.DebugInGameMessage($"{controller} state changed to {CurrentState}");
		}
	}
}