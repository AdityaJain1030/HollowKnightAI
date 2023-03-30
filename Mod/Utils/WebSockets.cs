using WebSocketSharp;
using System.Collections.Generic;
using UnityEngine;

namespace HollowKnightAI.Utils
{
	/// <summary>
	/// A wrapper around WebSocketSharp.WebSocket that adds a few additional utilities
	/// </summary>
	public class Socket : WebSocket
	{
		public Queue<byte[]> UnreadMessages { get; private set; } = new Queue<byte[]>();
		public byte[] LastMessageSent { get; private set; }
		public Socket(string url, params string[] protocols) : base(url, protocols)
		{
			this.OnMessage += (sender, e) =>
			{
				UnreadMessages.Enqueue(e.RawData);
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

		public class WaitForMessage : CustomYieldInstruction
		{
			private Socket socket;
			private bool wait = true;

			public WaitForMessage(Socket socket)
			{
				this.socket = socket;
				this.socket.OnMessage += (sender, e) =>
				{
					wait = false;
				};
			}
			public override bool keepWaiting
			{
				get
				{
					return wait;
				}
			}
		}
	}
}