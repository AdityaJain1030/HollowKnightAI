// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace HollowKnightAI.Environments
// {
// 	abstract class Environment<SetupArgs>
// 	{
// 		public abstract IEnumerator Setup(SetupArgs args);
// 		public abstract void Update();
// 		public abstract void Cleanup();

// 		private IEnumerator Init(SetupArgs args)
// 		{
			
// 			GameManager.instance.StartCoroutine(UpdateCoroutine());
// 		}

// 		public void Start(SetupArgs args) {
// 			GameManager.instance.StartCoroutine(Setup(args));
// 		}
// 	}
// }