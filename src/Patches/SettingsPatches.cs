using HarmonyLib;
using Newtonsoft.Json.Linq;
using SFS;
using SFS.Builds;
using SFS.UI;
using System;
using System.IO;
using UnityEngine;

namespace VanillaUpgrades
{
    [HarmonyPatch(typeof(BasicMenu), nameof(BasicMenu.OnOpen))]
    public class ShowSettings
    {
        [HarmonyPostfix]
        public static void Postfix(BasicMenu __instance)
        {
            if (__instance.GetComponent<VideoSettingsPC>())
            {
                Config.showSettings = true;
            }
            Main.menuOpen = true;
            Main.menuOpen1.Value = true;
        }
    }

    [HarmonyPatch(typeof(BasicMenu), nameof(BasicMenu.OnClose))]
    public class HideSettings
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            Config.showSettings = false;
            Main.menuOpen = false;
            Main.menuOpen1.Value = false;
            File.WriteAllText(Config.configPath, Config.settings.ToString());
            if (Main.buildObject != null)
            {
                if ((bool)Config.settings["moreCameraZoom"])
                {
                    if (BuildManager.main.buildCamera.maxCameraDistance == 300) return;
                    BuildManager.main.buildCamera.maxCameraDistance = 300;
                    BuildManager.main.buildCamera.minCameraDistance = 0.1f;
                }
                else
                {
                    if (BuildManager.main.buildCamera.maxCameraDistance == 60) return;
                    BuildManager.main.buildCamera.maxCameraDistance = 60;
                    BuildManager.main.buildCamera.minCameraDistance = 10f;
                }
            }
            Config.windowColor = new Color((float)Config.settings["persistentVars"]["windowColor"]["r"], (float)Config.settings["persistentVars"]["windowColor"]["g"], (float)Config.settings["persistentVars"]["windowColor"]["b"], VideoSettingsPC.main.uiOpacitySlider.value);
        }

    }
}
