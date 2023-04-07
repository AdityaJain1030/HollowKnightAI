using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HollowKnightAI.Environments
{
	public abstract class Environment
	{
	
		public abstract IEnumerator Setup();

		public abstract IEnumerator Loop();

		public abstract IEnumerator Cleanup();

		public abstract IEnumerator Reset();

		public abstract void DoAction();

		public abstract void GetObservation();

		public abstract void GetReward();

		public abstract void GetDone();

		private IEnumerator _runtime() {
			yield return Setup();
			while (true) {
				yield return Loop();
			}
		}

		public void Start() {
			GameManager.instance.StartCoroutine(Setup());

		}

	}
}