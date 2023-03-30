using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Modding;
// using TorchSharp;
using UnityEngine;
using UnityEngine.UI;
using HollowKnightAI.Utils;
using System.Linq;

using UObject = UnityEngine.Object;

// using static TorchSharp.torch.nn;

namespace HollowKnightAI
{
	internal class HollowKnightAI : Mod
	{
		internal static HollowKnightAI Instance { get; private set; }
		internal Game.HitboxReaderHook hitboxReader = new();
		internal Environments.Testing testing;
		internal Utils.Socket socket;
		internal RawImage ez;

		public HollowKnightAI() :
			base("HollowKnightAI")
		{
		}

		public override string GetVersion()
		{
			return Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}

		public override void Initialize()
		{
			Log("Initializing");

			Instance = this;

			Log("Initialized");

			Log("Done");

			bool start = false;

			ModHooks.HeroUpdateHook += () =>
			{
				if (Input.GetKeyDown(KeyCode.F1))
				{
					socket = new("ws://localhost:8080");
					hitboxReader.Load();
					socket.Connect();
                    GameManager.instance.StartCoroutine(Setup());
				}
                if (Input.GetKeyDown(KeyCode.F2))
                {
                 Log(socket.UnreadMessages.Count);
                }

				if (start)
				{
					Utils.Image image = new(Screen.width, Screen.height);
					var hitboxes = hitboxReader.GetHitboxes();
					Camera camera = Camera.main;
					foreach (var pair in hitboxes)
					{
						byte greyscale = (byte)((255 / 5) * ((int)pair.Key + 1));
						foreach (var hitbox in pair.Value)
						{
							// dont render invisible hitboxes
							if (hitbox == null || !hitbox.isActiveAndEnabled)
							{
								continue;
							}
							switch (hitbox)
							{
								case BoxCollider2D box:
									List<Vector2> points = box.ToScreenCoordinates(camera);
									// log all points
									image.DrawPolygon(points, greyscale);
									break;
								case CircleCollider2D circle:
									(Vector2 center, int radius) = circle.ToScreenCoordinates(camera);
									image.DrawCircle(center, radius, greyscale);
									break;
								case PolygonCollider2D polygon:
									List<List<Vector2>> polygonPoints = polygon.ToScreenCoordinates(camera);
									foreach (var shape in polygonPoints)
									{
										image.DrawPolygon(shape, greyscale);
									}
									break;
								case EdgeCollider2D edge:
									List<Vector2> edgePoints = edge.ToScreenCoordinates(camera);
									image.DrawPolygon(edgePoints, greyscale);
									break;
								default:
									break;
							}
						}
					}
					socket.Send(image.pixels);
				}
			};

		}
		public IEnumerator Setup()
		{
            Log("Setting up");
            socket.Send(new byte[] { 0 });
			yield return new WaitForMessage(socket, Log);
			var message = socket.UnreadMessages.Dequeue();
            Log("Got message");
            Log(System.Text.Encoding.Default.GetString(message));
			if (System.Text.Encoding.Default.GetString(message) == "Hello"){
                Log("Loading boss scene");
				Game.SceneHooks.LoadBossScene("GG_Hornet_1");
                Log("Loaded boss scene");
            }
			yield return Loop();
		}

		public IEnumerator Loop()
		{
			while (true)
			{
				yield return new WaitForMessage(socket, Log);
                Log("Got message");
				var message = socket.UnreadMessages.Dequeue();
				socket.Send(message);
			}
			// Game.SceneHooks.LoadBossScene("GG_Workshop");
		}

        public class WaitForMessage : CustomYieldInstruction
		{
			private Socket socket;
			private bool wait = true;
            private Action<string> Log;

			public WaitForMessage(Socket socket, Action<string> log)
			{
				this.socket = socket;
                Log = log;
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
