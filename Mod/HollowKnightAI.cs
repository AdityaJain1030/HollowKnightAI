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

            ModHooks.HeroUpdateHook += () => {
                if (Input.GetKeyDown(KeyCode.F1))
                {
                    start = true;
                    hitboxReader.Load();
                     socket = new("ws://localhost:8080");
                    socket.Connect();
                    
                }
                if (Input.GetKeyDown(KeyCode.F3))
                {
                    ez = new GameObject().AddComponent<RawImage>();
                    ez.texture = new Texture2D(212, 120);
                    ez.rectTransform.sizeDelta = new Vector2(212, 120);
                }
                if (Input.GetKeyDown(KeyCode.F2))
                {
                    socket.Send("Hello World");
                    Log("Sent message");
                    Log("Socket state: " + socket.ReadyState);
                    Log("Last message sent: " + socket.LastMessageSent);
                }

                if (start)
                {
                    Utils.Image image = new(Screen.width, Screen.height);
                    var hitboxes = hitboxReader.GetHitboxes();
                    Camera camera = Camera.main;
                    foreach (var pair in hitboxes)
                    {
                        byte greyscale = (byte)((255/5) * ((int)pair.Key + 1));
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
    }
}
