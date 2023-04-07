using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using MessagePack;
using System.Linq;

namespace HollowKnightAI.Environments
{
	public class TestEnv2
	{
		private Utils.Socket socket;
		private Game.HitboxReaderHook hitboxReader = new();
		private Game.InputDeviceShim inputDevice = new();

		private string sceneName = "GG_Hornet_1";

		public TestEnv2()
		{
			socket = new("ws://localhost:8080");
			hitboxReader.Load();
			InputManager.AttachDevice(inputDevice);
			socket.Connect();
			// This is a test
		}

		private IEnumerator Setup()
		{
			while(true)
			{
				yield return new Utils.Socket.WaitForMessage(socket);
				byte[] message = socket.UnreadMessages.Dequeue();
				//not init message
				if (message[0] != 0x00) continue;
				var data = MessagePackSerializer.Deserialize<Utils.Reset>(message.Skip(1).ToArray());
				yield return Game.SceneHooks.LoadBossScene(data.SceneName);
				break;
			}
		}

		private IEnumerator Loop() {
				yield return new Utils.Socket.WaitForMessage(socket);
				var message = socket.UnreadMessages.Dequeue();
				// Reset message
				if (message[0] != 0x00)
				{
					var data = MessagePackSerializer.Deserialize<Utils.Reset>(message.Skip(1).ToArray());
					yield return Game.SceneHooks.LoadBossScene(sceneName);
				}
				else if (message[0] == 0x01)
				{
					var data = MessagePackSerializer.Deserialize<Utils.Step>(message.Skip(1).ToArray());
					// inputDevice.
					if (data.LRN ==)

				}
		}

		private IEnumerator _runtime() {
			yield return Setup();
			while (true) {
				yield return Loop();
			}
		}

		public void Run() {
			GameManager.instance.StartCoroutine(_runtime());
		}
	}
}