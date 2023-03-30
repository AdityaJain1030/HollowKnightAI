using System.Collections;
using System.Linq;

namespace HollowKnightAI.Environments
{
	public class Testing
	{
		private Utils.Socket socket;
		private Game.HitboxReaderHook hitboxReader = new();

		public Testing()
		{
			socket = new("ws://localhost:8080");
			hitboxReader.Load();
			socket.Connect();
			// This is a test
		}

		public IEnumerator Setup()
		{
			yield return new Utils.Socket.WaitForMessage(socket);
			var message = socket.UnreadMessages.Dequeue();
			if (message[0] == (byte)0)
				Game.SceneHooks.LoadBossScene("GG_Hornet_1");
			GameManager.instance.StartCoroutine(Loop());
		}

		public IEnumerator Loop() {
			while(true) {
				yield return new Utils.Socket.WaitForMessage(socket);
				var message = socket.UnreadMessages.Dequeue();
				socket.Send(message);
			}
			// Game.SceneHooks.LoadBossScene("GG_Workshop");
		}
	}
}