using Il2CppSLZ.Bonelab;
using LabFusion.Data;
using LabFusion.Patching;
using LabFusion.Player;
using LabFusion.Utilities;
using MelonLoader;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2CppSLZ.Marrow;
using Il2CppSLZ.Marrow.Data;
using LabFusion.RPC;
using BoneLib;
using LabFusion.Network;
using HarmonyLib;
using Il2CppSLZ.Marrow.Warehouse;
using Il2CppSLZ.Marrow.Combat;
using LabFusion.Senders;
using Il2CppSLZ.Marrow.Pool;
using UnityEngine;
using System.Collections;
using LabFusion.SDK.Metadata;
using LabFusion.SDK.Gamemodes;
using LabFusion.Representation;
using UnityEngine.Playables;
using LabFusion.Marrow.Integration;
using static Il2CppSLZ.Bonelab.BaseGameController;
using LabFusion.Grabbables;
using LabFusion.MonoBehaviours;
using LabFusion.Entities;
using Il2CppUltEvents;
using MelonLoader.Utils;
using Il2CppGrpc.Core;
using static UnityEngine.Rendering.Universal.LibTessDotNet.MeshUtils;
using LabFusion.Marrow;
using Il2CppSteamworks;
using Il2CppSLZ.Marrow.Interaction;
using Steamworks.Data;
using BoneLib.BoneMenu;
using Il2CppSLZ.VRMK;

namespace MelonLoaderMod1
{
    internal class Patches
    {
        private static string selectedbarcode;
        private static Hand hand;
        private static SpawnGun gun;
        public static Il2CppSLZ.VRMK.Avatar avatar;
        public static Gun currentgun;
        public static Magazine currentmag;
        public static int roundmagmult = 1;
        public static string selectedimage;
        public static GameObject currentimageboard;
        public static bool voiddamage = false;
        public static string[] images;
        [HarmonyLib.HarmonyPatch(typeof(CheatTool))]
        [HarmonyLib.HarmonyPatch(nameof(CheatTool.Start))]
        public static class DevPatch
        {
            // Patch method that would set the "selectedbarcode" variable to the barcode of the selected spawnable
            [HarmonyLib.HarmonyPostfix]
            [HarmonyLib.HarmonyPatch(typeof(SpawnGun), nameof(SpawnGun.OnSpawnableSelected))]
            public static void OnSpawnableSel(SpawnableCrate crate)
            {
                if (crate == null)
                {
                    MelonLogger.Msg("Spawnable is null");
                    return;
                }

                selectedbarcode = crate.Barcode._id;
                if (TestPage.syncspawnmenuwithbarcode)
                {
                    TestPage.setbarcode.Value = selectedbarcode;
                    return;
                }

            }
            // Debug method to look at FusionMessages created
            [HarmonyPatch(typeof(FusionMessage), nameof(FusionMessage.Create))]
            public static void FusionMessageCreated(byte tag, FusionWriter writer)
            {
                MelonLogger.Msg(tag);
                MelonLogger.Msg(writer.Buffer);
            }
             
            // Patch method that would set the "currentimageboard" GameObject variable to the ImageBoard Object, would also set the "gun" SpawnGun variable to the held SpawnGun Object
            [HarmonyPatch(typeof(Grip), nameof(Grip.OnAttachedToHand))]
            [HarmonyPostfix]
            public static void grippytest(Grip __instance, Hand hand)
            {
                
                if(hand != Player.LeftHand && hand != Player.RightHand)
                {
                    MelonLogger.Msg("Hand is not ours");
                    return;
                }
                var item = hand.GetComponent<InventoryHand>();
                var gm = item.Host.GetHostGameObject().GetComponent<Transform>();
                if(item.Host.GetHostGameObject().GetComponent<SpawnGun>() != null)
                {
                    MelonLogger.Msg("Spawn gun is attached");
                    gun = item.Host.GetHostGameObject().GetComponent<SpawnGun>();
                    return;
                }
                if (gm.name.StartsWith("ImageBoard"))
                {
                    currentimageboard = gm.gameObject;
                    return;
                }
                if (!hand.manager.IsLocalPlayer())
                {
                    return;
                }
                if (!TestPage.imagemidas)
                {
                    return;
                }
                if (gm.name.StartsWith("ImageBoard"))
                {
                    currentimageboard = gm.gameObject;
                    var meshrenderer = gm.transform.FindChild("Mesh").FindChild("Picture_Target").GetComponent<MeshRenderer>();
                    var fusionrpc = gm.transform.FindChild("UltEvents").FindChild("FusionRPC").FindChild("ReceiveData").GetComponent<RPCString>();
                    var texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                    string imagePath = Path.Combine(MelonEnvironment.UserDataDirectory, "Images", selectedimage);
                    if (File.Exists(imagePath))
                    {
                        byte[] imagedata = File.ReadAllBytes(imagePath);
                        texture.LoadImage(imagedata);
                        fusionrpc.SetValue(Convert.ToBase64String(imagedata));
                        texture.filterMode = FilterMode.Bilinear;
                        texture.wrapMode = TextureWrapMode.Clamp;
                        texture.Apply();

                        meshrenderer.material.mainTexture = texture;

                        meshrenderer.material.SetFloat("_DetailAlbedoScale", 1.0f);
                        meshrenderer.material.SetFloat("_DetailNormalScale", 0.0f);

                    }

                }
                return;
            }
            // Patch method that would fire a Spawn Loop if the duplicatefromspawngun option is set to True
            [HarmonyLib.HarmonyPostfix]
            [HarmonyLib.HarmonyPatch(typeof(SpawnGun), nameof(SpawnGun.OnFire))]

            public static void OnFirePostFix()
            {
                if (gun.isDespawnMode)
                {
                    MelonLogger.Msg("Despawn mode is enabled");
                    return;
                }
                MelonCoroutines.Start(OnFire());
            }
            public static IEnumerator OnFire()
            {
                if (!NetworkInfo.HasLayer)
                {
                    MelonLogger.Msg("No network layer");
                    if (TestPage.duplicatefromspawngun)
                    {
                        for (int i = 0; i < TestPage.spawnedamountfromspawngun; i++)
                        {
                            MelonLogger.Msg("Spawning");
                            Spawnable spawnable = new Spawnable()
                            {
                                crateRef = new SpawnableCrateReference(selectedbarcode)
                            };
                            AssetSpawner.Register(spawnable);
                            AssetSpawner.Spawn(spawnable, gun.placerPreview.transform.position, gun.placerPreview.transform.rotation, new Il2CppSystem.Nullable<Vector3>(), null, false, new Il2CppSystem.Nullable<int>(), null, null);
                            yield return new WaitForSeconds(TestPage.duplicatespawninterval);
                        }
                    }
                    yield break;
                }

                if (TestPage.duplicatefromspawngun)
                {
                    MelonLogger.Msg("Spawning duplicates");
                    Spawnable spawnable = new Spawnable()
                    {
                        crateRef = new SpawnableCrateReference(selectedbarcode)
                    };

                    for (int i = 0; i < TestPage.spawnedamountfromspawngun; i++)
                    {
                        MelonLogger.Msg("Spawning");
                        NetworkAssetSpawner.Spawn(new NetworkAssetSpawner.SpawnRequestInfo()
                        {
                            spawnable = spawnable,
                            position = gun.placerPreview.transform.position,
                            rotation = gun.placerPreview.transform.rotation,
                        });
                        yield return new WaitForSeconds(TestPage.duplicatespawninterval);
                    }
                }
                else
                {
                    yield break;
                }
            }
        }
        // Patch method that would set damage received to 0 if the "voiddamage" option is set to True
        [HarmonyLib.HarmonyPatch(typeof(Player_Health))]
        public static class HealthPatches
        {
            [HarmonyPatch(nameof(Player_Health.TAKEDAMAGE))]
            [HarmonyPrefix]
            public static void TakeDamage(float damage, Player_Health __instance)
            {
                if(!__instance.gameObject == Player.RigManager.gameObject)
                {
                    return;
                } else
                {
                    if (voiddamage)
                    {
                        damage = 0f;
                        MelonLogger.Msg("Nulled damage applied");
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            // Patch method that would multiply the agility and speed of the avatar
            [HarmonyPatch(typeof(Il2CppSLZ.VRMK.Avatar), nameof(Il2CppSLZ.VRMK.Avatar.ComputeBaseStats))]
            [HarmonyPrefix]
            public static void ComputeBaseStatsPostfix(Il2CppSLZ.VRMK.Avatar __instance)
            {
                MelonLogger.Msg("Avatar stats computed");
                if (__instance == null)
                {
                    MelonLogger.Msg("AUGHGHGHGHGHGHhhh");
                    return;
                }
                MelonLogger.Msg("Avatar stats computed x2");
                MelonLogger.Msg(__instance._agility);
                MelonLogger.Msg(__instance._speed);
                TestPage.applyAvatarStats(__instance);
            }

            public static class RevengeAttackPatches
            {
                // Patch method that would send damage to the last attacker if the "revengeattack" option is set to True
                [HarmonyPatch(nameof(PlayerDamageReceiver.ReceiveAttack))]
                [HarmonyPrefix]

                private static void RevengeAttack(PlayerDamageReceiver __instance)
                {
                    if(!Player.RigManager.gameObject == __instance.gameObject)
                    {
                        return;
                    }
                    MelonLogger.Msg("Attacked");
                    TestPage.currenthealth = __instance.health.curr_Health;
                    TestPage.currenthealth = Player.RigManager.GetComponent<Player_Health>().curr_Health;
                    TestPage.last_attacker = (byte)FusionPlayer.LastAttacker;
                    
                    if (TestPage.revengeattack)
                    {
                        Attack attack = new Attack();
                        attack.damage = 9223372036854775807;
                        MelonLogger.Msg("Sending attack");
                        PlayerSender.SendPlayerDamage(TestPage.last_attacker, attack);
                        return;
                    }
                    return;
                }
            }
            [HarmonyPatch(typeof(Gun))]
            public static class GunPatches
            {
               
                // Debug method that would log any MonoBehaviors invoked
                [HarmonyPatch(typeof(MonoBehaviour), nameof(MonoBehaviour.Invoke))]
                [HarmonyPostfix]
                public static void UltEventHolderInvoked(string methodName, float time)
                {

                    MelonLogger.Msg("UltEventHolder invoked: " + methodName);
                }
                // Logs actions that were sent in a server
                [HarmonyPatch(typeof(PlayerSender), nameof(PlayerSender.SendPlayerAction))]
                [HarmonyPrefix]
                public static void SendPlayerAction(PlayerActionType type)
                {
                    MelonLogger.Msg(type.ToString());

                }

                [HarmonyPatch(typeof(FusionDevTools))]
                // Patch Methods that would attempt to disable despawning any categorized devtools 
                public static class FusionDevToolsPatch
                {
                    [HarmonyPatch(nameof(FusionDevTools.DespawnDevTool))]
                    [HarmonyPrefix]
                    public static bool DespawnDevTool(ref bool __result) 
                    {
                        __result = false;
                        MelonLogger.Msg("Despawn Dev tool removed");
                        return false;
                    }
                    [HarmonyPatch(nameof(FusionDevTools.PreventSpawnGun))]
                    [HarmonyPrefix]
                    public static bool PreventSpawn(ref bool __result)
                    {
                        __result = false;
                        MelonLogger.Msg("Spawn gun is disabled");
                        return false;
                        
                    }
                }
            }
        }
    }
}

