using HarmonyLib;
using System;
using ModLoader;
using System.IO;
using System.Reflection;
using SFS.Variables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VanillaUpgrades
{
    public class Main : Mod
    {
        public Main() : base(
           "VanUp", // Mod id
           "VanillaUpgrades", // Mod Name
           "ASoD", // Mod Author
           "0.5.7", // Game version
           "v3.0", // Mod version
           "Upgrades the vanilla experience with quality-of-life features and keybinds. See the GitHub repository for a list of features."
           )
        { }
        public override void Early_Load()
        {
            patcher = new Harmony("mods.ASoD.VanUp");
            patcher.PatchAll();
            SceneManager.sceneLoaded += OnSceneLoaded;
            Application.quitting += OnQuit;
            return;
        }

        public override void Load()
        {
            mainObject = new GameObject("ASoDMainObject");
            mainObject.AddComponent<WindowManager>();
            mainObject.AddComponent<Config>();
            mainObject.AddComponent<ErrorNotification>();
            UnityEngine.Object.DontDestroyOnLoad(mainObject);
            mainObject.SetActive(true);

        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            switch (scene.name)
            {
                case "Build_PC":
                    buildObject = new GameObject("ASoDBuildObject");
                    buildObject.AddComponent<BuildSettings>();
                    buildObject.AddComponent<DVCalc>();
                    buildObject.SetActive(true);
                    UnityEngine.Object.Destroy(worldObject);
                    return;

                case "World_PC":
                    worldObject = new GameObject("ASoDWorldObject");
                    worldObject.AddComponent<WorldManager>();
                    worldObject.AddComponent<AdvancedInfo>();
                    // worldViewObject.AddComponent<TimewarpToClass>();
                    worldObject.AddComponent<FaceDirection>();
                    worldObject.AddComponent<WorldClockDisplay>();
                    worldObject.SetActive(true);
                    UnityEngine.Object.Destroy(buildObject);
                    return;

                default:
                    UnityEngine.Object.Destroy(buildObject);
                    UnityEngine.Object.Destroy(worldObject);
                    return;
            }
        }

        private void OnQuit()
        {
            File.WriteAllText(WindowManager.inst.windowDir, WindowManager.settings.ToString());
        }



        public static bool menuOpen;

        public static Bool_Local menuOpen1 = new Bool_Local();
        
        public static GameObject mainObject;

        public static GameObject buildObject;

        public static GameObject worldObject;

        public static Harmony patcher;

        public static string modDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
    }
}
