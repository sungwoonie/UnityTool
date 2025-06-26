using System.Collections.Generic;
using UnityEngine.Events;

namespace StarCloudgamesLibrary
{
	public class EventBus
	{
		private static GameState currentState;
		private static readonly IDictionary<GameState, UnityEvent> events = new Dictionary<GameState, UnityEvent>();

		public static void SubscribeEvent(GameState event_type, UnityAction listener)
		{
			UnityEvent _event;

			if (events.TryGetValue(event_type, out _event))
			{
				_event.AddListener(listener);
			}
			else
			{
				_event = new UnityEvent();
				_event.AddListener(listener);
				events.Add(event_type, _event);
			}
		}

		public static void UnSubscribeEvent(GameState event_type, UnityAction listener)
		{
			UnityEvent _event;

			if (events.TryGetValue(event_type, out _event))
			{
				_event.RemoveListener(listener);
			}
		}

		public static void Publish(GameState event_type)
		{
			UnityEvent _event;

			currentState = event_type;

			if (events.TryGetValue(event_type, out _event))
			{
				_event.Invoke();
			}

			DebugManager.DebugInGameMessage($"{event_type} published");
		}

		public static GameState GetCurrentState()
		{
			return currentState;
		}
	}
}