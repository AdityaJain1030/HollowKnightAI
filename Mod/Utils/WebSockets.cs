using WebSocketSharp;

namespace HollowKnightAI.Utils
{
	/// <summary>
	/// A wrapper around WebSocketSharp.WebSocket that adds a few additional utilities
	/// </summary>
	public class Socket : WebSocket
	{
		public byte[] LastMessage { get; private set; }
		public byte[] LastMessageSent { get; private set; }
		public Socket(string url, params string[] protocols) : base(url, protocols)
		{
			this.OnMessage += (sender, e) =>
			{
				LastMessage = e.RawData;
			};
		}

		public new void Send(byte[] data)
		{
			base.Send(data);
			LastMessageSent = data;
		}

		public new void Send(string data)
		{
			base.Send(data);
			LastMessageSent = System.Text.Encoding.UTF8.GetBytes(data);
		}
		public new void SendAsync(byte[] data, System.Action<bool> completed)
		{
			base.SendAsync(data, completed);
			LastMessageSent = data;
		}
		public new void SendAsync(string data, System.Action<bool> completed)
		{
			base.SendAsync(data, completed);
			LastMessageSent = System.Text.Encoding.UTF8.GetBytes(data);
		}

		// public new void SendAndRecieve(byte[] data, System.Action<byte[]> completed)
		// {
		// 	base.SendAsync(data, (success) =>
		// 	{
		// 		completed(LastMessage);
		// 	});
		// }
	}
}