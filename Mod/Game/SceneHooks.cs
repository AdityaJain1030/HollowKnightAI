using System.Collections;
using UnityEngine;

namespace HollowKnightAI.Game
{
	public static class SceneHooks
	{
		public static IEnumerator LoadBossScene(string scene_name)
        {
            var HC = HeroController.instance;
            var GM = GameManager.instance;

            //Copy paste of the FSM that loads a boss from HoG
            PlayerData.instance.dreamReturnScene = "GG_Workshop";
            PlayMakerFSM.BroadcastEvent("BOX DOWN DREAM");
            PlayMakerFSM.BroadcastEvent("CONVO CANCEL");

            HC.ClearMPSendEvents();
            GM.TimePasses();
            GM.ResetSemiPersistentItems();
            HC.enterWithoutInput = true;
            HC.AcceptInput();

            GM.BeginSceneTransition(new GameManager.SceneLoadInfo
            {
                SceneName = scene_name,
                EntryGateName = "door_dreamEnter",
                EntryDelay = 0,
                Visualization = GameManager.SceneLoadVisualizations.GodsAndGlory,
                PreventCameraFadeOut = true
            });
            yield return FixSoul();
			yield return new WaitForSeconds(2f);
        }
		private static IEnumerator FixSoul()
        {
            yield return new WaitForFinishedEnteringScene();
            yield return null;
            yield return new WaitForSeconds(1f); //this line differenciates this function from ApplySettings
            HeroController.instance.AddMPCharge(1);
            HeroController.instance.AddMPCharge(-1);
        }
	}
}