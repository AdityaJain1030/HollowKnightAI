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

			On.BossSceneController.Awake += (orig, a) => Log("BossSceneController.Awake");
			// On.BossChallengeUI.Awake += (orig, a) => Log();

			ModHooks.HeroUpdateHook += () =>
			{
				if (Input.GetKeyDown(KeyCode.F1))
				{
					// StartTP();
					// var statues = GameObject.FindObjectsOfType<BossStatue>();
					// foreach (var statue in statues)
					{
						// statue.gameObject.SetActive(false);
						// Log(statue.dreamReturnGate.name);
					}
					testing = new();
					GameManager.instance.StartCoroutine(testing.Setup());
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

		public void StartTP()
		{
			var HC = HeroController.instance;
            var GM = GameManager.instance;

			PlayMakerFSM.BroadcastEvent("DREAM ENTER");
			PlayerData.instance.dreamReturnScene = "GG_Workshop";
			PlayerData.instance.bossReturnEntryGate = "door_dreamReturnGG_GG_Statue_Hornet";
			PlayMakerFSM.BroadcastEvent("BOX DOWN DREAM");
            PlayMakerFSM.BroadcastEvent("CONVO CANCEL");
			PlayMakerFSM.BroadcastEvent("GG TRANSITION OUT");
			BossSceneController.SetupEvent = (self) => {
                StaticVariableList.SetValue("bossSceneToLoad", "GG_Hornet_1");
                self.BossLevel = 1;
                self.DreamReturnEvent = "DREAM RETURN";
                self.OnBossSceneComplete += () => self.DoDreamReturn();
            };
			HC.ClearMPSendEvents();
            GM.TimePasses();
            GM.ResetSemiPersistentItems();
            HC.enterWithoutInput = true;
            HC.AcceptInput();

			GM.BeginSceneTransition(new GameManager.SceneLoadInfo
            {
                SceneName = "GG_Hornet_1",
                EntryGateName = "door_dreamEnter",
                EntryDelay = 0,
                Visualization = GameManager.SceneLoadVisualizations.GodsAndGlory,
                PreventCameraFadeOut = true
            });

		}
	}
}
