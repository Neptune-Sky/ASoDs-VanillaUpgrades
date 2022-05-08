﻿using Newtonsoft.Json.Linq;
using System;
using System.IO;
using UnityEngine;

namespace VanillaUpgrades
{
    public class WindowManager : MonoBehaviour
    {
        public Vector2 defaultBuildSettings = new Vector2(Screen.width * 0.05f, Screen.height * 0.94f);

        public Vector2 defaultAdvancedInfo = new Vector2(0f, Screen.height * 0.05f);

        public Vector2 defaultConfig = new Vector2(10f, Screen.height - Config.windowHeight);

        public Vector2 defaultWorldTime = new Vector2(Screen.width * 0.958f, Screen.height * 0.045f);

        public JObject defaults = JObject.Parse("{buildSettings:{}, advancedInfo:{}, config:{}, worldTime:{}}");

        public static JObject settings;

        public string windowDir = Main.modDir + "WindowPositions.txt";

        public static WindowManager inst;

        // Keeps windows from being moved off the screen.
        public static Rect ConfineRect(Rect window)
        {
            window.x = Mathf.Clamp(window.x, 0, Screen.width - window.width);
            window.y = Mathf.Clamp(window.y, 0, Screen.height - window.height);
            return window;
        }

        // Returns a window ID that will not conflict with other windows.
        public static int GetValidID()
        {
            return GUIUtility.GetControlID(FocusType.Passive);
        }

        public void Awake()
        {
            defaults["buildSettings"]["x"] = defaultBuildSettings.x;
            defaults["buildSettings"]["y"] = defaultBuildSettings.y;
            defaults["advancedInfo"]["x"] = defaultAdvancedInfo.x;
            defaults["advancedInfo"]["y"] = defaultAdvancedInfo.y;
            defaults["config"]["x"] = defaultConfig.x;
            defaults["config"]["y"] = defaultConfig.y;
            defaults["worldTime"]["x"] = defaultWorldTime.x;
            defaults["worldTime"]["y"] = defaultWorldTime.y;

            if (!File.Exists(windowDir))
            {
                File.WriteAllText(windowDir, defaults.ToString());
            }

            try
            {
                settings = JObject.Parse(File.ReadAllText(windowDir));
                Vector2 check;
                check = new Vector2((float)settings["buildSettings"]["x"], (float)settings["buildSettings"]["y"]);
                check = new Vector2((float)settings["advancedInfo"]["x"], (float)settings["advancedInfo"]["y"]);
                check = new Vector2((float)settings["config"]["x"], (float)settings["config"]["y"]);
                check = new Vector2((float)settings["worldTime"]["x"], (float)settings["worldTime"]["y"]);

                if (settings["buildSettings"] == null)
                {
                    settings["buildSettings"]["x"] = defaultBuildSettings.x;
                    settings["buildSettings"]["y"] = defaultBuildSettings.y;
                }
                if (settings["advancedInfo"] == null)
                {
                    settings["advancedInfo"]["x"] = defaultAdvancedInfo.x;
                    settings["advancedInfo"]["y"] = defaultAdvancedInfo.y;
                }
                if (settings["config"] == null)
                {
                    settings["config"] = defaultConfig.x;
                    settings["config"]["y"] = defaultConfig.y;
                }
                if (settings["worldTime"] == null)
                {
                    settings["worldTime"]["x"] = defaultWorldTime.x;
                    settings["worldTime"]["y"] = defaultWorldTime.y;
                }
            }
            catch (Exception e)
            {
                File.WriteAllText(windowDir, defaults.ToString());
                ErrorNotification.Error("Window positions file was of an invalid format, and was reset to defaults.");
                settings = defaults;
            }

            inst = this;
        }
    }
}
