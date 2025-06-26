using System.Collections;
using UnityEngine;

namespace StarCloudgamesLibrary
{
	public class ObserverSubject : MonoBehaviour
	{

		private readonly ArrayList observers = new ArrayList();

		public void Attach(Observer observer)
		{
			observers.Add(observer);
		}

		public void Detach(Observer observer)
		{
			observers.Remove(observer);
		}

		public void NotifyToObservers()
		{
			foreach (Observer observer in observers)
			{
				observer.Notify();
			}
		}
	}
}