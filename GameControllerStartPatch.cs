using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PostProcessing;
using Logger = BepInEx.Logging.Logger;

[assembly: SecurityPermission(System.Security.Permissions.SecurityAction.RequestMinimum, SkipVerification = true)]

namespace LowLatencyTromboneChamp
{

	[HarmonyPatch(typeof(GameController))]
	[HarmonyPatch("playNote")] // if possible use nameof() here
	class LowLatencyGameControllerPlayNotePatch
	{
		static void Postfix(GameController __instance)
		{
			if (!Plugin.Instance.areSamplesloaded)
			{
				Plugin.Instance.InitTromboneClips(__instance.trombclips.tclips);
			}

			float num = 9999f;
			int num2 = 0;
			for (int i = 0; i < 15; i++)
			{
				float num3 = Mathf.Abs(__instance.notelinepos[i] - __instance.pointer.transform.localPosition.y);
				if (num3 < num)
				{
					num = num3;
					num2 = i;
				}
			}
			Debug.Log("closest index: " + num2);
			__instance.notestartpos = __instance.notelinepos[num2];
			__instance.currentnotesound.clip = __instance.trombclips.tclips[Mathf.Abs(num2 - 14)];
			__instance.currentnotesound.volume = 0f;
			__instance.currentnotesound.Stop();

			Plugin.Instance.PlaySound(Mathf.Abs(num2 - 14));
			return;
		}
	}
	
	[HarmonyPatch(typeof(GameController))]
	[HarmonyPatch("stopNote")] // if possible use nameof() here
	class GameControllerStopNotePatch
	{
		static void Postfix(GameController __instance)
		{
			Plugin.Instance.currentStream?.Stop();
			return;
		}
	}
	
	[HarmonyPatch(typeof(GameController))]
	[HarmonyPatch("Update")] // if possible use nameof() here
	class GameControllerUpdatePatch
	{
		static void Postfix(GameController __instance)
		{
			__instance.currentnotesound.volume = 0f;
			if (__instance.noteplaying && Plugin.Instance.areSamplesloaded && Plugin.Instance.isAsioInitted)
			{
				Plugin.Instance.currentStream?.SetSpeed(__instance.currentnotesound.pitch);
			}

			return;
		}
	}
}
