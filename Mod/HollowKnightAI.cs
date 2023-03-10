using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace HollowKnightAI
{
    internal class HollowKnightAI : Mod
    {
        internal static HollowKnightAI Instance { get; private set; }

        public HollowKnightAI() : base("HollowKnightAI") { }

        public override string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public override void Initialize()
        {
            Log("Initializing");

            Instance = this;

            Log("Initialized");
        }
    }
}