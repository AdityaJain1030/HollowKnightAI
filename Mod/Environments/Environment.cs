using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HollowKnightAI.Environments
{
	abstract class Environment
	{
	
		public abstract IEnumerator Setup();

		public abstract IEnumerator Loop();

		public abstract IEnumerator Cleanup();

		public abstract IEnumerator Reset();

		public abstract void DoAction();

		public abstract void GetObservation();

		public abstract void GetReward();

		public abstract void GetDone();

	}
}