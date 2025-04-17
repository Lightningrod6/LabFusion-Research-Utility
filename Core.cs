using MelonLoader;
using BoneLib.BoneMenu;
using BoneLib;
using LabFusion;
using Il2CppSLZ.Marrow;
using LabFusion.Data;
using LabFusion.Player;
using LabFusion.Network;
using Il2CppSLZ.VRMK;
using LabFusion.Entities;
using UnityEngine;
using Il2CppSLZ.Bonelab;
using MelonLoader.Utils;
using System.Text.Json;
using Harmony;
[assembly: MelonInfo(typeof(MelonLoaderMod1.Core), "Fusion FunZies", "1.0.0", "Lightningrod654", null)]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

namespace MelonLoaderMod1
{
    public class Core : MelonMod
    {
        private static HarmonyLib.Harmony harmony;
        private static RigRefs refe;


        private static NetworkEntity nw;
        public static CheatTool currentinstance;
        internal static CheatTool playerCheat;
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("--------------");
            LoggerInstance.Msg(System.ConsoleColor.Red, "Fusion Utility Research Menu");
            LoggerInstance.Msg("Created by Lightningrod654, a research project that has now finished its course.");
            LoggerInstance.Msg(System.ConsoleColor.Cyan, "https://lightningweb.xyz");
            var images = System.IO.Directory.GetFiles($@"{MelonEnvironment.UserDataDirectory}\Images");
            Patches.images = images;
            MelonLogger.Msg("Added Images");
            LoggerInstance.Msg("--------------");
            harmony = HarmonyInstance;
            foreach(var patch in harmony.GetPatchedMethods())
            {
                LoggerInstance.Msg("Patched Method: " + patch.Name);
            }
            playerCheat = currentinstance;
            TestPage.BonemenuSetup();

        }


        public override void OnDeinitializeMelon()
        {
            base.OnDeinitializeMelon();
            LoggerInstance.Msg("Fusion Funzies has been unloaded. Goodbye!");
        }


        public override void OnUpdate()
        {
            base.OnUpdate();
            
            // Removes constraint from a rigmanager if "protectedfromconstraints" was set to true
            if (TestPage.protectedfromconstraints == true)
            {
                var physicsRig = RigData.Refs.RigManager.physicsRig;
                var constraints = physicsRig.GetComponentsInChildren<ConstraintTracker>();

                if (constraints != null)
                {
                    LocalPlayer.ClearConstraints();
                }
            }
        }
    }
}

