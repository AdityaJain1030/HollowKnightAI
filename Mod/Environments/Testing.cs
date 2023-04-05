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
			string data = System.Text.Encoding.UTF8.GetString(message);
			HollowKnightAI.Instance.Log(data);
			if (data == "Hello"){
				yield return Game.SceneHooks.LoadBossScene("GG_Hornet_1");

			}
			yield return Loop();
		}

		public IEnumerator Loop() {
			while (true) {
				yield return new Utils.Socket.WaitForMessage(socket);
				var message = socket.UnreadMessages.Dequeue();
				string data = System.Text.Encoding.UTF8.GetString(message);
				if (data == "Hello")
					yield return Game.SceneHooks.LoadBossScene("GG_Hornet_1");
			}
			// Game.SceneHooks.LoadBossScene("GG_Workshop");
		}
	}
}