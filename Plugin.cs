using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ASIO.NET;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


namespace LowLatencyTromboneChamp
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance;
        public AudioClip currentClip;
        
        private void Awake()
        {
            Instance = this;
            LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            var harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll();
        }
        

        #region logging
        internal static void LogDebug(string message) => Instance.Log(message, LogLevel.Debug);
        internal static void LogInfo(string message) => Instance.Log(message, LogLevel.Info);
        internal static void LogWarning(string message) => Instance.Log(message, LogLevel.Warning);
        internal static void LogError(string message) => Instance.Log(message, LogLevel.Error);
        private void Log(string message, LogLevel logLevel) => Logger.Log(logLevel, message);
        #endregion
        
        public Asio deviceOut;
        public bool isAsioInitted = false;
        public bool areSamplesloaded = false;
        public SoundStream currentStream = null;
        public void InitTromboneClips(AudioClip[] clips)
        {
            foreach (var clip in clips)
            {
                Logger.LogInfo(clip.name);
                var channels = clip.channels;
                var sampleRate = clip.frequency;
                var data = new float[clip.samples * channels];
                clip.GetData(data, 0);
                deviceOut.Load(data, (uint)sampleRate, (uint)channels);
                areSamplesloaded = true;
            }
        }

        public void PlaySound(int id)
        {
            Logger.LogWarning("Playing Asio Note!!");
            if (currentStream != null) currentStream.Stop();
            currentStream = deviceOut.Play(
                soundID: id+1,
                outChannels: new uint[] { 0, 1 },
                looping: true);
            
        }

        public bool hide = false;
        private void OnGUI()
        {
            if (hide) return;
            if (!isAsioInitted)
            {
                GUILayout.Label("Select Your Asio Device");
                foreach (var device in Asio.GetDeviceNames())
                {
                    if (GUILayout.Button(device))
                    {
                        deviceOut = Asio.CreatePlayer(device, 48000);
                        isAsioInitted = true;
                    }
                }
            }
            else
            {
                if (GUILayout.Button("Show Asio Panel"))
                {
                    deviceOut.ShowControlPanel();
                }
                if (GUILayout.Button("Hide"))
                {
                    hide = true;
                }
            }

        }
    }
}
