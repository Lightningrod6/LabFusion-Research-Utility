using BoneLib.BoneMenu;
using System.Reflection;
using UnityEngine;
using System.Text.Json;
using BoneLib;
using LabFusion;
using Il2CppSLZ.Marrow.Data;
using BoneLib.Notifications;
using LabFusion.Network;
using Il2CppSLZ.Marrow.Warehouse;
using LabFusion.RPC;
//using System.Runtime.CompilerServices;
using MelonLoader;
using Il2CppSLZ.Marrow;
using LabFusion.Utilities;
using System.Windows.Input;
using LabFusion.SDK.Metadata;
using LabFusion.Entities;
using LabFusion.Player;
using LabFusion.Data;
using System.Collections;
using LabFusion.Senders;
using Il2CppSLZ.Marrow.Pool;
using Il2CppSLZ.Marrow.Combat;
using LabFusion.SDK.Gamemodes;
using Il2CppSLZ.Marrow.Interaction;
using Il2CppSLZ.Bonelab;
using Il2CppUltEvents;
using LabFusion.Extensions;
using MelonLoader.Utils;
using LabFusion.Marrow.Integration;
using UnityEngine.Networking;
using UnityEngine.Video;
using Steamworks.Data;
using Steamworks;
using Il2CppSLZ.VRMK;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using LabFusion.Patching;
using LabFusion.SDK.Points;
using LabFusion.Marrow;
using Harmony;

namespace MelonLoaderMod1
{
    public enum FireMode
    {
        SINGLE,
        AUTO
    }

    public class TestPage
    {
        private static BoneLib.BoneMenu.Page _logginpage;
        private static BoneLib.BoneMenu.Page _maincategory;
        public static BoneLib.BoneMenu.Page _healthstuff;
        private static BoneLib.BoneMenu.Page _trollstuff;
        private static BoneLib.BoneMenu.Page _playerstuff;
        private static BoneLib.BoneMenu.Page _spawnstuff;
        private static BoneLib.BoneMenu.Page _networkPlayer;
        private static BoneLib.BoneMenu.Page _debugging;
        private static BoneLib.BoneMenu.Page _protections;
        private static BoneLib.BoneMenu.Page _fusionlobbies;
        private static BoneLib.BoneMenu.Page imageboard;
        private static BoneLib.BoneMenu.Page images;
        private static BoneLib.BoneMenu.Page videos;
        private static BoneLib.BoneMenu.Page _fun;
        private static BoneLib.BoneMenu.StringElement code;
        private static BoneLib.BoneMenu.StringElement usernameasd;
        private static BoneLib.BoneMenu.FunctionElement login;
        public static BoneLib.BoneMenu.FunctionElement _lastattacker;
        public static BoneLib.BoneMenu.StringElement setbarcode;
        private static readonly HttpClient client = new HttpClient();
        public static bool imagemidas = false;
        public PlayerScoreKeeper scorekeeper;
        private static MelonPreferences_Category _category;
        public static bool recover_from_death;
        private static Coroutine _loopSpawnCoroutine;
        public static Il2CppSLZ.VRMK.Avatar currentavatar = null;
        public static RigRefs refer;
        public static bool turnoffnetworkspawning;
        public static string currentbarcode;
        public static bool revengeattack = false;
        public static bool furryprotection;
        private static int spawnamount = 1;
        private static bool _loopKillActive = false;
        private static bool loopspawn;
        private static bool loopspawnplayer;
        public static int spawnedamountfromspawngun = 1;
        public static bool duplicatefromspawngun;
        public static float duplicatespawninterval;
        private static bool enabled;
        public static int agilitymulti = 1;
        public static int vitalitymulti = 1;
        private static string lastavatar;
        public static StringElement hp;
        public static float currenthealth;
        private static string[] recently_spawned;
        public static byte last_attacker;
        public static string last_attacker_name;
        public static bool spawnwithimageboard = false;
        private static int spawn_force;
        private static int firework_amount = 10;
        public static bool invisible = false;
        private static string networkplayerAvatar;
        public static bool syncspawnmenuwithbarcode;
        public static bool protectedfromconstraints;
        private static float loopspawninterval = 1;
        public static bool weightlessgrab = true;
        public static float speedmulti = 1;
        public static float launchinterval;
        public static bool loggedin { get; set; } = false;
        public static string codea;
        public static string username;
        public static string steam_id;
        public static bool makeanenterence = false;
        private static int missileamount = 10;
        private static float missileinterval = 1;
        private static BoneLib.BoneMenu.FunctionElement loginthing;
        private static float firework_delay = 1;
        private static int firework_height= 20;
        public GameObject rigobj;

     //   public static bool CheckForFile()
     //   {
     //       if(!File.Exists(Core.filepath))
     //       {
     //           return false;
      //      } else
      //      {
      //          if(Login.CheckIfUsernameAndCodeValid(codea, username, "online"))
      //          {
      //              if (Login.CheckSteamID(ulong.Parse(steam_id)))
      //              {
      //                  loggedin = true;
       //                 return true;
       //             }
       //         }
       //         return false;
       //     }
       // }
        


        public static void BonemenuSetup()
        {
            TestPage._category = MelonPreferences.CreateCategory("FusionFunzies", "Fusion Funzies");
            _maincategory = BoneLib.BoneMenu.Page.Root.CreatePage("Fusion Funzies", UnityEngine.Color.green);
            _protections = _maincategory.CreatePage("Protections", UnityEngine.Color.green);
            _trollstuff = _maincategory.CreatePage("Trolling Tools", UnityEngine.Color.green);
            _fun = _maincategory.CreatePage("Fun Tools", UnityEngine.Color.green);
            imageboard = _maincategory.CreatePage("ImageBoard Tools", UnityEngine.Color.green);
            var explosive = _fun.CreatePage("Missile Tools", UnityEngine.Color.green);

            explosive.CreateFunction("Set off Mini Missiles", UnityEngine.Color.yellow, () => MelonCoroutines.Start(SetOffMiniMissiles()));
            explosive.CreateFunction("Set off Large Missiles", UnityEngine.Color.yellow, () => MelonCoroutines.Start(SetOffLargeMissiles()));
            explosive.CreateFloat("Interval", UnityEngine.Color.yellow, 0.1f, 0.1f, 0.1f, 10f, (i) =>
            {
                launchinterval = i;
            });
            images = imageboard.CreatePage("Images", UnityEngine.Color.green);
            imageboard.CreateFunction("Apply Image to All Image Boards", UnityEngine.Color.green, () => MelonCoroutines.Start(ApplyImageToAllImageBoards()));
            imageboard.CreateFunction("Set current imageboard to image", UnityEngine.Color.green, () => SetCurrentImageBoard());
            imageboard.CreateFunction("Refresh", UnityEngine.Color.cyan, () =>
            {
                RefreshImages();
            });
            imageboard.CreateString("Generate Image", UnityEngine.Color.cyan, "", (str) =>
            {
                MelonCoroutines.Start(GenerateAIImage(str));
            });
            foreach (var image in Patches.images)
            {
                images.CreateFunction(System.IO.Path.GetFileName(image), UnityEngine.Color.cyan, () =>
                {
                    MelonLogger.Msg("Image Found");
                    Patches.selectedimage = image;
                    MelonLogger.Msg(image);
                });
            }
            _debugging = _maincategory.CreatePage("Debug Tools", UnityEngine.Color.green);
            _protections.CreateBool("Constrainer", UnityEngine.Color.cyan, false, (constraints) =>
            {
                if (constraints)
                {
                    protectedfromconstraints = true;
                }
                else
                {
                    protectedfromconstraints = false;
                }
            });
            _debugging.CreateFunction("Fire all guns", UnityEngine.Color.cyan, GetAllGuns);
            _debugging.CreateFunction("Kill Self", UnityEngine.Color.cyan, () => DamageUser(PlayerIdManager.LocalId));
            var getrighthandbarcode = _debugging.CreatePage("Get Barcode(Right hand)", UnityEngine.Color.green);
            var getlefthandbarcode = _debugging.CreatePage("Get Barcode(Left hand)", UnityEngine.Color.green);
            getrighthandbarcode.CreateFunction("Get Barcode", UnityEngine.Color.cyan, () =>
            {
                if (BoneLib.Player.GetObjectInHand(BoneLib.Player.RightHand) == null)
                {
                    Notifier.Send(new Notification()
                    {
                        Title = (BoneLib.Notifications.NotificationText)"Error",
                        Message = (BoneLib.Notifications.NotificationText)"Right Hand is empty",
                        PopupLength = 5f,
                        Type = BoneLib.Notifications.NotificationType.Error,
                        ShowTitleOnPopup = true
                    });
                }
                else
                {
                    var barcode = BoneLib.Player.GetComponentInHand<Poolee>(BoneLib.Player.RightHand).SpawnableCrate.Barcode.ID;
                    var barcodepage = getrighthandbarcode.CreatePage(barcode, UnityEngine.Color.green);
                    barcodepage.CreateFunction("Copy Barcode", UnityEngine.Color.cyan, () =>
                    {
                        GUIUtility.systemCopyBuffer = barcode;
                        Notifier.Send(new Notification()
                        {
                            Title = (BoneLib.Notifications.NotificationText)"Copied",
                            Message = (BoneLib.Notifications.NotificationText)"Barcode copied to clipboard",
                            PopupLength = 5f,
                            Type = BoneLib.Notifications.NotificationType.Success,
                            ShowTitleOnPopup = true
                        });
                    });
                }
            });
            getlefthandbarcode.CreateFunction("Get Barcode", UnityEngine.Color.cyan, () =>
            {
                if (BoneLib.Player.GetObjectInHand(BoneLib.Player.LeftHand) == null)
                {
                    Notifier.Send(new Notification()
                    {
                        Title = (BoneLib.Notifications.NotificationText)"Error",
                        Message = (BoneLib.Notifications.NotificationText)"Left Hand is empty",
                        PopupLength = 5f,
                        Type = BoneLib.Notifications.NotificationType.Error,
                        ShowTitleOnPopup = true
                    });
                }
                else
                {
                    var barcode = BoneLib.Player.GetComponentInHand<Poolee>(BoneLib.Player.LeftHand).SpawnableCrate.Barcode.ID;
                    var barcodepage = getlefthandbarcode.CreatePage(barcode, UnityEngine.Color.green);
                    barcodepage.CreateFunction("Copy Barcode", UnityEngine.Color.cyan, () =>
                    {
                        GUIUtility.systemCopyBuffer = barcode;
                        Notifier.Send(new Notification()
                        {
                            Title = (BoneLib.Notifications.NotificationText)"Copied",
                            Message = (BoneLib.Notifications.NotificationText)"Barcode copied to clipboard",
                            PopupLength = 5f,
                            Type = BoneLib.Notifications.NotificationType.Success,
                            ShowTitleOnPopup = true
                        });
                    });
                }
            });
            _healthstuff = _maincategory.CreatePage("Health Tools", UnityEngine.Color.green);
            _debugging.CreateFunction("Get Health Data", UnityEngine.Color.cyan, (() => TestPage.getHealthData()));
            _healthstuff.CreateFunction("Give Max Health", UnityEngine.Color.cyan, (() => TestPage.ChangeHealthValues()));
            _healthstuff.CreateBool("Void all damage", UnityEngine.Color.cyan, false, (i) =>
            {
                if (i)
                {
                    Patches.voiddamage = true;
                }
                else
                {
                    Patches.voiddamage = false;
                }
            });
            _healthstuff.CreateBool("Recover From Death", UnityEngine.Color.cyan, false, (recover) =>
            {
                if (recover)
                {
                    recover_from_death = true;
                }
                else
                {
                    recover_from_death = false;
                }
            });
            _playerstuff = _maincategory.CreatePage("Player List", UnityEngine.Color.green);
            _playerstuff.CreateFunction("Refresh", UnityEngine.Color.gray, Refresh);
            _spawnstuff = _maincategory.CreatePage("Spawn Tools", UnityEngine.Color.green);
            _fun.CreateFunction("C4 Game", UnityEngine.Color.cyan, (() => TestPage.C4Game()));
            var firework_settings = _fun.CreatePage("Firework Settings", UnityEngine.Color.green);
            firework_settings.CreateInt("Set Amount", UnityEngine.Color.cyan, 1, 1, 1, 100, (i) =>
            {
                firework_amount = i;
            });
            firework_settings.CreateFloat("Set Delay", UnityEngine.Color.cyan, 1, 0.1f, 0.1f, 10, (i) =>
            {
                firework_delay = i;
            });
            firework_settings.CreateInt("Set Height", UnityEngine.Color.cyan, 20, 1, 1, 100, (i) =>
            {
                firework_height = i;
            });
            _fun.CreateFunction("Firework Show!", UnityEngine.Color.yellow, () =>
            {
                MelonCoroutines.Start(FireworkShow());
            });
            _fun.CreateFunction("AirStrike!!!", UnityEngine.Color.yellow, () =>
            {
                MelonCoroutines.Start(AirStrike());
            });
            var airstrike_settings = _fun.CreatePage("AirStrike Settings", UnityEngine.Color.green);
            airstrike_settings.CreateInt("Set Amount", UnityEngine.Color.cyan, 10, 1, 1, 100, (i) =>
            {
                missileamount = i;
            });
            airstrike_settings.CreateFloat("Set Interval", UnityEngine.Color.cyan, 1, 0.1f, 0.1f, 100, (i) =>
            {
                missileinterval = i;
            });
            _fun.CreateFunction("Fog", UnityEngine.Color.gray, () => TheFogIsComing());
            _trollstuff.CreateFunction("Blow up everyone", UnityEngine.Color.blue, () =>
            {
                var ids = PlayerIdManager.PlayerIds;
                foreach (var id in ids)
                {
                    if (id.IsMe)
                    {
                        continue;
                    }
                    BlowupPlayer(id.SmallId);
                }
            });
            _trollstuff.CreateFunction("Make Everyone Weak", UnityEngine.Color.gray, () =>
            {
                var ids = PlayerIdManager.PlayerIds;

                foreach (var id in ids)
                {
                    if (id.IsMe)
                    {
                        continue;
                    }
                    MakeWeak(id);
                }
            });
            var lastattacker = _trollstuff.CreatePage("Last Attacker", UnityEngine.Color.green);
            lastattacker.CreateFunction("Kill Last Attacker", UnityEngine.Color.cyan, () =>
            {
                if (last_attacker != 0)
                {
                    var id = PlayerIdManager.GetPlayerId(last_attacker);
                    DamageUser(id);
                }
                else
                {
                    Notifier.Send(new Notification()
                    {
                        Title = (BoneLib.Notifications.NotificationText)"Hold on",
                        Message = (BoneLib.Notifications.NotificationText)"You haven't been attacked yet",
                        PopupLength = 5f,
                        Type = BoneLib.Notifications.NotificationType.Error,
                        ShowTitleOnPopup = true
                    });
                }
            });
            lastattacker.CreateFunction("Make Weak", UnityEngine.Color.cyan, () =>
            {
                if (last_attacker != 0)
                {
                    var id = PlayerIdManager.GetPlayerId(last_attacker);
                    MakeWeak(id);
                }
                else
                {
                    Notifier.Send(new Notification()
                    {
                        Title = (BoneLib.Notifications.NotificationText)"Hold on",
                        Message = (BoneLib.Notifications.NotificationText)"You haven't been attacked yet",
                        PopupLength = 5f,
                        Type = BoneLib.Notifications.NotificationType.Error,
                        ShowTitleOnPopup = true
                    });
                }
            });
            lastattacker.CreateFunction("Teleport", UnityEngine.Color.cyan, () =>
            {
                if (last_attacker != 0)
                {
                    var id = PlayerIdManager.GetPlayerId(last_attacker);
                    Teleporttoplayer(id.SmallId);
                }
                else
                {
                    Notifier.Send(new Notification()
                    {
                        Title = (BoneLib.Notifications.NotificationText)"Hold on",
                        Message = (BoneLib.Notifications.NotificationText)"You haven't been attacked yet",
                        PopupLength = 5f,
                        Type = BoneLib.Notifications.NotificationType.Error,
                        ShowTitleOnPopup = true
                    });
                }
            });
            lastattacker.CreateFunction("Spawn Barcode", UnityEngine.Color.cyan, () =>
            {
                if (last_attacker != 0)
                {
                    var id = PlayerIdManager.GetPlayerId(last_attacker);
                    SpawnBarcodeOnPlayer(id);
                }
                else
                {
                    Notifier.Send(new Notification()
                    {
                        Title = (BoneLib.Notifications.NotificationText)"Hold on",
                        Message = (BoneLib.Notifications.NotificationText)"You haven't been attacked yet",
                        PopupLength = 5f,
                        Type = BoneLib.Notifications.NotificationType.Error,
                        ShowTitleOnPopup = true
                    });
                }
            });
            _spawnstuff.CreateFunction("Spawn Barcode", UnityEngine.Color.cyan, (() => TestPage.SpawnBarcode()));
            _spawnstuff.CreateFunction("Refresh Spawned", UnityEngine.Color.cyan, () => ListSpawnedProps());
            _spawnstuff.CreateBool("Loop Spawn", UnityEngine.Color.cyan, false, (loop) =>
            {
                if (loop)
                {
                    if (_loopSpawnCoroutine == null)
                    {
                        _loopSpawnCoroutine = (Coroutine)MelonCoroutines.Start(LoopSpawnCoroutine());
                    }
                }
                else
                {
                    if (_loopSpawnCoroutine != null)
                    {
                        MelonCoroutines.Stop(_loopSpawnCoroutine);
                        _loopSpawnCoroutine = null;
                    }
                }
            });
            _trollstuff.CreateFunction("Kill All", UnityEngine.Color.cyan, () =>
            {
                foreach (var id in PlayerIdManager.PlayerIds)
                {
                    if (id.IsMe)
                    {
                        continue;
                    }
                    DamageUser(id);
                }
            });

            _spawnstuff.CreateFunction("Despawn All", UnityEngine.Color.cyan, () =>
            {
                var entities = NetworkEntityManager.IdManager.RegisteredEntities.EntityIdLookup.Keys.ToArray();

                foreach (var entity in entities)
                {
                    var prop = entity.GetExtender<NetworkProp>();
                    if (prop == null)
                    {
                        continue;
                    }
                    MelonLogger.Msg(entity.Id);

                    PooleeUtilities.SendDespawn(entity.Id, false);
                }
            });
            _spawnstuff.CreateFunction("Spawn on Everyone", UnityEngine.Color.cyan, () =>
            {
                foreach (var id in PlayerIdManager.PlayerIds)
                {
                    SpawnBarcodeOnPlayer(id);
                }
            });
            _trollstuff.CreateBool("Revenge Attack", UnityEngine.Color.cyan, false, (revenge) =>
            {
                if (revenge)
                {
                    revengeattack = true;
                }
                else
                {
                    revengeattack = false;
                }
            });
            var settings = _spawnstuff.CreatePage("Settings", UnityEngine.Color.green);
            setbarcode = settings.CreateString("Set Barcode", UnityEngine.Color.cyan, "", (barcode) => {
                currentbarcode = barcode;
            });
            settings.CreateInt("Set Amount", UnityEngine.Color.cyan, 1, 1, 1, 100, (i) =>
            {
                spawnamount = i;
            });
            settings.CreateInt("Set Spawn Force", UnityEngine.Color.cyan, 1, 1, 1, 100, (a) =>
            {
                spawn_force = a;
            });
            settings.CreateFloat("Set Spawn Interval", UnityEngine.Color.cyan, 1, 0.1f, 0.1f, 10f, (a) =>
            {
                loopspawninterval = a;
            });
            settings.CreateInt("Duplicates from spawngun", UnityEngine.Color.cyan, 0, 1, 0, 100, (i) =>
            {
                spawnedamountfromspawngun = i;
            });
            settings.CreateBool("Sync Barcode to Menu", UnityEngine.Color.cyan, false, (sync) =>
            {
                if (sync)
                {
                    syncspawnmenuwithbarcode = true;
                }
                else
                {
                    syncspawnmenuwithbarcode = false;
                }
            });
            settings.CreateBool("Duplicate from Spawn Gun", UnityEngine.Color.cyan, false, (duplicate) =>
            {
                if (duplicate)
                {
                    duplicatefromspawngun = true;
                }
                else
                {
                    duplicatefromspawngun = false;
                }
            });
            settings.CreateFloat("Duplicate Spawn Interval", UnityEngine.Color.cyan, 0.1f, 0.1f, 0.1f, 10f, (i) =>
            {
                duplicatespawninterval = i;
            });
            var playersettings = _maincategory.CreatePage("Player Settings", UnityEngine.Color.green);
            playersettings.CreateFloat("Speed Multiplier", UnityEngine.Color.cyan, speedmulti, 1, 1, 100, (i) =>
            {
                BoneLib.Player.RigManager._avatar._speed = BoneLib.Player.RigManager._avatar._speed * i;
            });
            playersettings.CreateInt("Agility Multiplier", UnityEngine.Color.cyan, 1, 1, 1, 100, (i) =>
            {
                agilitymulti = i;
            });
            playersettings.CreateInt("Vitality Multiplier", UnityEngine.Color.cyan, 1, 1, 1, 100, (i) =>
            {
                vitalitymulti = i;
            });
            playersettings.CreateFunction("Apply Multiplies", UnityEngine.Color.cyan, () =>
            {
                applyAvatarStats(Player.RigManager.avatar);
            });
            playersettings.CreateBool("Invisible", UnityEngine.Color.cyan, false, (invis) =>
            {
                if (invis)
                {
                    invisible = true;
                }
                else
                {
                    invisible = false;
                }
            });
        }

     
       
        // Spawn Method from Daytrip that used the Fusion Writer instead of the usual method 
        public static void DayTripSpawnMethod(string barcode, Vector3 position, Quaternion rotation)
        {
            SerializedTransform transform = new SerializedTransform(position, rotation);
            SpawnRequestData data = SpawnRequestData.Create(PlayerIdManager.LocalId, barcode, transform, 0U, false);
            using(FusionWriter fusionWriter = FusionWriter.Create(21 + StringExtensions.GetSize(barcode)))
            {
                fusionWriter.Write<SpawnRequestData>(data);
                using(FusionMessage fusionMessage = FusionMessage.Create(NativeMessageTag.SpawnRequest, fusionWriter))
                {
                    MessageSender.SendToServer(NetworkChannel.Reliable, fusionMessage);
                }
            }
        }
        /*  public static bool RainbowName()
          {
              RigNameTag rigNameTag = Player.RigManager.GetComponent<RigNameTag>();
              while (true)
              {
                  rigNameTag.Color = Color.RGBToHSV()
              }
          } */
        public static void GetAllGuns()
        {
            Poolee[] poolees = UnityEngine.Object.FindObjectsOfType<Poolee>();

            foreach(var poolee in poolees)
            {
                var gun = poolee.gameObject.GetComponentInChildren<Gun>();

                if (gun == null)
                {
                    continue;
                }
                if (gun._poolee.GetComponentInChildren<SpawnGun>() != null)
                {
                    continue;
                }
                gun.Fire();
            }
        }

        public static IEnumerator SetOffLargeMissiles()
        {
            Poolee[] poolees = UnityEngine.Object.FindObjectsOfType<Poolee>();
            foreach (var pool in poolees)
            {
                if (!pool.name.StartsWith("Missile"))
                {
                    continue;
                }
                MelonLogger.Msg(pool.name);
                pool.gameObject.transform.FindChild("Flight").gameObject.SetActive(true);
                yield return new WaitForSeconds(launchinterval);
            }
            yield return null;
        }

        public static IEnumerator SetOffMiniMissiles()
        {
            Poolee[] poolees = UnityEngine.Object.FindObjectsOfType<Poolee>();

            foreach(var pool in poolees)
            {
                if(!pool.name.StartsWith("Mini Missile"))
                {
                    MelonLogger.Msg("Not a missile");
                    continue;
                }
                MelonLogger.Msg(pool.name);
                pool.gameObject.transform.FindChild("Flight (1)").gameObject.SetActive(true);
                yield return new WaitForSeconds(launchinterval);
            }
            yield return null;
        }
        public static void GetBarcodeInRightHand()
        {
            if (BoneLib.Player.GetObjectInHand(BoneLib.Player.RightHand) == null)
            {
                Notifier.Send(new Notification()
                {
                    Title = (BoneLib.Notifications.NotificationText)"Error",
                    Message = (BoneLib.Notifications.NotificationText)"Right Hand is empty",
                    PopupLength = 5f,
                    Type = BoneLib.Notifications.NotificationType.Error,
                    ShowTitleOnPopup = true
                });
            } else
            {
                string barcode = BoneLib.Player.GetComponentInHand<Poolee>(BoneLib.Player.RightHand).SpawnableCrate.Barcode.ID;
                Notifier.Send(new Notification()
                {
                    Title = (BoneLib.Notifications.NotificationText)"Barcode",
                    Message = (BoneLib.Notifications.NotificationText)barcode,
                    PopupLength = 5f,
                    Type = BoneLib.Notifications.NotificationType.Success,
                    ShowTitleOnPopup = true
                });
            }
        }

        public static void TurnPlayerToYou(byte smallid)
        {

            GameObject gmplayer = GameObject.Find($@"[RigManager (Networked)] (ID {smallid})");

            Vector3 myposition = Player.Head.position;

            Quaternion lookatme = Quaternion.LookRotation(myposition);
            RigPose rigpose = new RigPose();

            PlayerPoseUpdateData data = PlayerPoseUpdateData.Create(smallid, rigpose);

            using var writer = FusionWriter.Create(142);
            writer.Write<PlayerPoseUpdateData>(data);

            SerializedSmallQuaternion serializedSmallQuaternion = SerializedSmallQuaternion.Compress(lookatme);

            using var message = FusionMessage.Create(NativeMessageTag.PlayerPoseUpdate, writer);

            MessageSender.SendToServer(NetworkChannel.Unreliable, message);
        }

        // Testing Function for getting the spawn gun in the rigmanager
        public static SpawnGun GetSpawngun(SpawnGun gun)
        {
            var rigManager = Player.RigManager;
            if(rigManager == null)
            {
                return null;
            }
            var spawnGun = rigManager.GetComponentsInChildren<SpawnGun>(true).FirstOrDefault();
            if (spawnGun == null)
            {
                return null;
            }
            MelonLogger.Msg("Spawn gun was found");
            return spawnGun;

        }
        // Method for querying the list of Fusion Lobbies
        public static void RefreshFusionLobbies()
        {
            IMatchmaker.MatchmakerCallbackInfo info = new IMatchmaker.MatchmakerCallbackInfo();
            

            List<IMatchmaker.LobbyInfo> netlobbies = new();
            foreach(var lobby in netlobbies)
            {
                MelonLogger.Msg(lobby.metadata.LobbyInfo.LobbyId);
            }
        }

        // SpawnForce Method provided by daytrip
        public static IEnumerator WaitForSpawnedPooleeThenForce(string crateBarcode, GameObject tracker)
        {
            float timeout = 5f; // total seconds to wait
            bool forceApplied = false;

#if debug
    MelonLogger.Msg($"[SpawnForce] Looking for Poolee with barcode '{crateBarcode}' for up to {timeout} seconds...");
#endif

            while (timeout > 0f && !forceApplied)
            {
                Poolee[] poolees = UnityEngine.Object.FindObjectsOfType<Poolee>();

#if debug
        MelonLogger.Msg($"[SpawnForce] Found {poolees.Length} Poolees in the scene.");
#endif

                foreach (var poolee in poolees)
                {
                    var crate = poolee.SpawnableCrate;
                    if (crate == null)
                        continue;

                    string currentBarcode = crate.Barcode.ToString();
                    if (currentBarcode != crateBarcode)
                        continue; // Not the crate we're looking for

                    if (!poolee.gameObject.activeInHierarchy)
                    {
                        MelonLogger.Msg($"[SpawnForce] Poolee for '{currentBarcode}' is disabled (still pooled). Waiting...");
                        continue;
                    }

                    // Assign the tracker as a child of the spawned object (optional)
                    tracker.transform.SetParent(poolee.transform);
                    tracker.transform.localPosition = Vector3.zero;

                    Rigidbody rb = poolee.GetComponent<Rigidbody>() ?? poolee.gameObject.GetComponentInChildren<Rigidbody>();
                    if (rb != null)
                    {
                        float forceVal = spawn_force;
                        Vector3 dynamicForward = Player.RigManager.physicsRig.torso.transform.forward.normalized;
                        rb.AddForce(dynamicForward * forceVal, ForceMode.Impulse);

#if debug
                MelonLogger.Msg($"[SpawnForce] Applied {forceVal} impulse to '{rb.gameObject.name}', linked to tracker '{tracker.name}'.");
#endif
                        forceApplied = true;
                        break;
                    }
                    else
                    {
#if debug
                MelonLogger.Error("[SpawnForce] Matching Poolee has no Rigidbody; can’t apply force!");
#endif
                    }
                }

                if (!forceApplied)
                {
                    timeout -= Time.deltaTime;
                    yield return null;
                }
            }

            if (!forceApplied)
            {
                MelonLogger.Warning($"[SpawnForce] Timed out waiting for Poolee with barcode '{crateBarcode}'. No force applied.");
            }
        }
        // Method to apply new stats to BoneLab Avatars
        public static void applyAvatarStats(Il2CppSLZ.VRMK.Avatar avatar)
        {
            avatar._speed *= TestPage.agilitymulti;
            avatar._agility *= TestPage.agilitymulti;
            avatar._vitality *= TestPage.vitalitymulti;
            MelonLogger.Msg(avatar._speed);
            MelonLogger.Msg(avatar._agility);
            MelonLogger.Msg(avatar._vitality);
        }

        // Lists the Props that are spawned in a Lobby
        private static void ListSpawnedProps()
        {
            var spawneditems = _spawnstuff.CreatePage("Spawned Network Props", UnityEngine.Color.green);
            foreach (var id in NetworkEntityManager.IdManager.RegisteredEntities.EntityIdLookup.Keys)
            {
                var prop = id.GetExtender<NetworkProp>();
                if (prop == null)
                {
                    continue;
                }
                
                MelonLogger.Msg(prop);
                
                var item = spawneditems.CreatePage(prop.MarrowEntity.name, UnityEngine.Color.green);
               // item.CreateFunction("Launch", UnityEngine.Color.cyan, () => LaunchProp(prop.MarrowEntity.name));
                item.CreateFunction("Bring", UnityEngine.Color.cyan, () => BringProp(prop.MarrowEntity.name));
                item.CreateFunction("Despawn", UnityEngine.Color.cyan, () => DespawnProp(prop.MarrowEntity.name));
            }

            BoneLib.BoneMenu.Menu.OpenPage(_spawnstuff);
        }
       
        // Brings a Prop to the player, does not work on lobbies
        private static void BringProp(string prop)
        {
            GameObject gm = GameObject.Find(prop);
            if (!NetworkInfo.HasLayer)
            {
                gm.transform.position = Player.Head.position + Player.Head.forward * 2;
            } else
            {
                var poolee = Poolee.Cache.Get(gm);
                var netpoole = PooleeExtender.Cache.Get(poolee);
                var marrowentity = MarrowEntity.Cache.Get(gm);
               
                NetworkProp np = new NetworkProp(netpoole, marrowentity);
                np.MarrowEntity.transform.position = Player.Head.position + Player.Head.forward * 2;
                // poolee.transform.position = Player.Head.position + Player.Head.forward * 2;
            }
        }

        // Despawns a prop from the lobby
        private static void DespawnProp(string prop)
        {
            GameObject gm = GameObject.Find(prop);
            if (!NetworkInfo.HasLayer)
            {
                GameObject.Destroy(gm);
            }
            else
            {
                var poolee = Poolee.Cache.Get(gm);
                var netpoole = PooleeExtender.Cache.Get(poolee);
                var marrowentity = MarrowEntity.Cache.Get(gm);
                NetworkProp np = new NetworkProp(netpoole, marrowentity);
                var entity = NetworkEntityManager.IdManager.RegisteredEntities.GetEntity(np.NetworkEntity.Id);
                PooleeUtilities.SendDespawn(entity.Id, true);
            }
        }

        // Spawns a bunch of smoke grenade clouds around the user (sorta)
        private static IEnumerator TheFogIsComing()
        {
            Spawnable spawnable = new Spawnable()
            {
                crateRef = new SpawnableCrateReference("BaBaCorp.MiscExplosiveDevices.Spawnable.ExplosionSmokeGrenade")
            };
            System.Random rand = new System.Random();
            for (int i = 0; i < 20; i++)
            {
                Vector3 position = new Vector3(Player.Head.position.x + rand.Next(0, 40), Player.Head.position.y, Player.Head.position.y + rand.Next(0, 40));
                NetworkAssetSpawner.Spawn(new NetworkAssetSpawner.SpawnRequestInfo()
                {
                    spawnable = spawnable,
                    position = position,
                    rotation = Player.Head.rotation
                });
                yield return new WaitForSeconds(0.1f);
            }
        }

        // Turns the targeted player into a weaker avatar
        private static void MakeWeak(PlayerId id)
        {
            Spawnable spawnable = new Spawnable()
            {
                crateRef = new SpawnableCrateReference("DayTrip.Fursonas.Spawnable.ProjectileRainbowCat")
            };
            var playerposition = GetPelvis(id.SmallId);

            NetworkAssetSpawner.Spawn(new NetworkAssetSpawner.SpawnRequestInfo()
            {
                spawnable = spawnable,
                position = playerposition.position,
                rotation = playerposition.rotation
            });
        }


        // Spawns in multiple missiles at random x and z above the player
        private static IEnumerator AirStrike()
        {
            var playerposition = Player.Head.position;
            var quart = new Quaternion(0, 0, 0, 0);
            var rand = new System.Random();
            for(int i = 0; i < missileamount; i++)
            {
                var xpos = rand.Next(-30, 30);
                var zpos = rand.Next(-30, 30);

                var position = new Vector3(playerposition.x + xpos, playerposition.y + 30, playerposition.z + xpos);
                Spawnable spawnable = new Spawnable()
                {
                    crateRef = new SpawnableCrateReference("BaBaCorp.MiscExplosiveDevices.Spawnable.MiniMissile")
                };

                NetworkAssetSpawner.Spawn(new NetworkAssetSpawner.SpawnRequestInfo()
                {
                    spawnable = spawnable,
                    position = position,
                    rotation = quart
                });
                yield return new WaitForSeconds(missileinterval);
            }
            
        }

        // Provides a list of recently spawned props 
        private static void UpdateBarcodePage(string barcode)
        {
            var recently_spawned_page = _spawnstuff.CreatePage("Recently Spawned", UnityEngine.Color.green);
            var barcode_page = recently_spawned_page.CreatePage(barcode, UnityEngine.Color.green);
            barcode_page.CreateFunction("Copy Barcode", UnityEngine.Color.cyan, () =>
            {
                GUIUtility.systemCopyBuffer = barcode;
                Notifier.Send(new Notification()
                {
                    Title = (BoneLib.Notifications.NotificationText)"Copied",
                    Message = (BoneLib.Notifications.NotificationText)"Barcode copied to clipboard",
                    PopupLength = 5f,
                    Type = BoneLib.Notifications.NotificationType.Success,
                    ShowTitleOnPopup = true
                });
            });
        }

        // Adjusts the avatars speed, Test Function
        private static void BecomeFast()
        {
            Player.RigManager._avatar._speed = 100;
        }
        // Global function for spawning props, has not replaced old methods
        public static void SpawnBarcodeGlobal(string barcode)
        {
            if (!NetworkInfo.HasLayer)
            {
                HelperMethods.SpawnCrate(barcode, Player.Head.position + Player.Head.forward * 2);
            }
            else
            {
                var position = Player.Head.position + Player.Head.forward * 2;
                Spawnable spawnable = new Spawnable()
                {
                    crateRef = new SpawnableCrateReference(barcode)
                };
                NetworkAssetSpawner.Spawn(new NetworkAssetSpawner.SpawnRequestInfo()
                {
                    spawnable = spawnable,
                    position = position,
                    rotation = Player.Head.rotation
                });
            }
        }

        // Global function for spawning props on network players, has not replaced old methods
        public static void SpawnBarcodePlayerGlobal(PlayerId player, string barcode)
        {
            if (!NetworkInfo.HasLayer)
            {
                return;
            }
            else
            {
                var playerposition = GetPelvis(player.SmallId);
                var position = playerposition.position + playerposition.forward * 1.5f;
                Spawnable spawnable = new Spawnable()
                {
                    crateRef = new SpawnableCrateReference(barcode)
                };
                NetworkAssetSpawner.Spawn(new NetworkAssetSpawner.SpawnRequestInfo()
                {
                    spawnable = spawnable,
                    position = position,
                    rotation = playerposition.rotation
                });
            }
        }

        // Old method of spawning a prop on a network player
        private static void SpawnBarcodeOnPlayer(PlayerId id)
        {
            var playerposition = GetPelvis(id.SmallId);

            if (currentbarcode == null)
            {
                Notifier.Send(new Notification()
                {
                    Title = (BoneLib.Notifications.NotificationText)"Hold on",
                    Message = (BoneLib.Notifications.NotificationText)"Barcode field is Empty",
                    PopupLength = 5f,
                    Type = BoneLib.Notifications.NotificationType.Error,
                    ShowTitleOnPopup = true
                });
                MelonLogger.Msg(currentbarcode);
            }
            else
            {
                if (!NetworkInfo.HasLayer)
                {
                    Notifier.Send(new Notification()
                    {
                        Title = (BoneLib.Notifications.NotificationText)"Hold on",
                        Message = (BoneLib.Notifications.NotificationText)"This is for fusion servers only",
                        PopupLength = 5f,
                        Type = BoneLib.Notifications.NotificationType.Error,
                        ShowTitleOnPopup = true
                    });
                }
                else
                {
                    Vector3 position = playerposition.position + playerposition.forward * 1.5f;
                    for(int i = 0; i < spawnamount; i++)
                    {
                        Spawnable spawnable = new Spawnable()
                        {
                            crateRef = new SpawnableCrateReference(currentbarcode)
                        };
                        NetworkAssetSpawner.Spawn(new NetworkAssetSpawner.SpawnRequestInfo()
                        {
                            spawnable = spawnable,
                            position = position,
                            rotation = playerposition.rotation
                        });
                        //MelonCoroutines.Start(WaitForSpawnedPooleeThenForce(spawnable.crateRef.Crate.Barcode.ID));
                        UpdateBarcodePage(currentbarcode);
                    }
                    
                }
            }
        }

        // Method that adds a score to a player, does not work
        private static void AddScoreToPlayer()
        {
            var myid = PlayerIdManager.LocalId;
            ScoreKeeper<PlayerId> scoreKeeper = new PlayerScoreKeeper();
            scoreKeeper.AddScore(myid, 100);
            Notifier.Send(new Notification()
            {
                Title = (BoneLib.Notifications.NotificationText)"Score Added",
                Message = (BoneLib.Notifications.NotificationText)"100 points added to player",
                PopupLength = 5f,
                Type = BoneLib.Notifications.NotificationType.Success,
                ShowTitleOnPopup = true
            });
        }

        // Spawns a barcode using the barcode variable in the TestPage class, integrated check to see if the user is in a lobby
        private static void SpawnBarcode()
        {
            string barcode = currentbarcode;
            if (string.IsNullOrEmpty(barcode))
            {
                Notifier.Send(new Notification()
                {
                    Title = (BoneLib.Notifications.NotificationText)"Hold on",
                    Message = (BoneLib.Notifications.NotificationText)"Barcode field is Empty",
                    PopupLength = 5f,
                    Type = BoneLib.Notifications.NotificationType.Error,
                    ShowTitleOnPopup = true
                });
                MelonLogger.Msg(barcode);
            }
            else
            {
                if (!NetworkInfo.HasLayer)
                {
                    Spawnable spawnable = new Spawnable()
                    {
                        crateRef = new SpawnableCrateReference(barcode)
                    };
                    AssetSpawner.Register(spawnable);
                    AssetSpawner.Spawn(spawnable, Player.Head.position + Player.Head.forward * 2, Player.Head.rotation, new Il2CppSystem.Nullable<Vector3>(), null, false, new Il2CppSystem.Nullable<int>(), null, null);
                    var gm = spawnable.crateRef.Crate.MainGameObject.Asset;
                    MelonCoroutines.Start(WaitForSpawnedPooleeThenForce(spawnable.crateRef.Barcode.ID, gm));

                }
                else
                {

                    var position = Player.Head.position + Player.Head.forward * 2;
                    for (int i = 0; i < spawnamount; i++)
                    {
                        Spawnable spawnable = new()
                        {
                            crateRef = new SpawnableCrateReference(barcode)
                        };
                        NetworkAssetSpawner.Spawn(new NetworkAssetSpawner.SpawnRequestInfo()
                        {
                            spawnable = spawnable,
                            position = position,
                            rotation = Player.Head.rotation
                        });
                        var gm = spawnable.crateRef.Crate.MainGameObject.Asset.gameObject;
                        MelonCoroutines.Start(WaitForSpawnedPooleeThenForce(barcode, gm));
                    }
               
                    UpdateBarcodePage(barcode);
                }
            }
        }


        // Another Lobby Query Test function, does not work
        private static void GetAllLobbies()
        {
            var lobbies = new LobbyQuery();
            var results = lobbies.RequestAsync();

            foreach(var lobby in results.Result)
            {
                MelonLogger.Msg(lobby.Id);
            }
        }
        // Timed Method that spawns in Fireworks above the player in random x and z coordinates
        private static IEnumerator FireworkShow()
        {
            var sky = Player.Head.position.y + firework_height;
            System.Random rand = new System.Random();
            for (var i = 0; i < firework_amount; i++)
            {
                var xposition = rand.Next(0, 40);
                var zposition = rand.Next(0, 40);
                MelonLogger.Msg(xposition);
                MelonLogger.Msg(zposition);
                var position = new Vector3(xposition, sky, zposition);
                Spawnable spawnable = new Spawnable()
                {
                    crateRef = new SpawnableCrateReference("BaBaCorp.MiscExplosiveDevices.Spawnable.ExplosionFireworkRed")
                };
                NetworkAssetSpawner.Spawn(new NetworkAssetSpawner.SpawnRequestInfo()
                {
                    spawnable = spawnable,
                    position = position,
                    rotation = Player.Head.rotation
                });
                yield return new WaitForSeconds(firework_delay);
            }
        }

        // Method that shows the different actions that you can do to the selected player
        private static void PlayerTest(PlayerId id)
        {
            string display_name = id.Metadata.GetMetadata(MetadataHelper.NicknameKey);
            string username = id.Metadata.GetMetadata(MetadataHelper.UsernameKey);

            string display;

            if (string.IsNullOrWhiteSpace(display_name))
            {
                display = username;
            }
            else
            {
                display = display_name;
            }

            var category = _playerstuff.CreatePage(display, UnityEngine.Color.white);
            // var sendaction = category.CreatePage("Send Action", UnityEngine.Color.white);
            // sendaction.CreateFunction("Send Death", UnityEngine.Color.red, () => PlayerSender.SendPlayerAction(PlayerActionType.DEATH, id));
            category.CreateFunction($@"ID: {id.SmallId}", UnityEngine.Color.yellow, () => { MelonLogger.Msg(id); });
            category.CreateFunction("Get Player", UnityEngine.Color.yellow, () => GetPelvis(id.SmallId));
            category.CreateFunction("Blowup Player", UnityEngine.Color.yellow, () => BlowupPlayer(id.SmallId));
            category.CreateFunction("Spawn Barcode", UnityEngine.Color.yellow, () => SpawnBarcodeOnPlayer(id));
            category.CreateFunction("Teleport to Player", UnityEngine.Color.yellow, () => Teleporttoplayer(id.SmallId));
            //category.CreateFunction("Teleport Player Here", UnityEngine.Color.red, () => TeleportPlayerHere(id));
            category.CreateFunction("Damage", UnityEngine.Color.yellow, () => DamageUser(id));
            category.CreateFunction("Weaken", UnityEngine.Color.yellow, () => MakeWeak(id));
            category.CreateFunction("Turn towards you", UnityEngine.Color.yellow, () => TurnPlayerToYou(id.SmallId));
            // category.CreateFunction("Launch", UnityEngine.Color.red, () => Launch(id));
            // category.CreateFunction("Add Score", UnityEngine.Color.red, () => AddScoreToPlayer(id));
            category.CreateBool("Loop Spawn", UnityEngine.Color.yellow, false, (loop) => {
                if (loop)
                {
                    if (_loopSpawnCoroutine == null)
                    {
                        _loopSpawnCoroutine = (Coroutine)MelonCoroutines.Start(LoopSpawnPlayerCoroutine(id));
                    }
                }
                else
                {
                    if (_loopSpawnCoroutine != null)
                    {
                        MelonCoroutines.Stop(_loopSpawnCoroutine);
                        _loopSpawnCoroutine = null;
                    }
                }
            });
            category.CreateBool("Loop Kill", UnityEngine.Color.red, false, (loop) =>
            {
                var player = PlayerIdManager.GetPlayerId(id);
                if (id.LongId != player.LongId)
                {
                    return;
                } else
                {
                    if (loop)
                    {
                        MelonCoroutines.Start(DamageUserLoop(id));
                    }
                    else
                    {
                        _loopKillActive = false; // Set the flag to false to stop the loop
                    }
                }
            });
            //category.CreateFunction("Clone Player", UnityEngine.Color.red, () => DisconnectUser(id));
        }

        // Method that would teleport the player associated with the ID to you, does not work
        public static void TeleportPlayerHere(PlayerId id)
        {
            PlayerSender.SendPlayerTeleport(id, Player.Head.position + Player.Head.forward * 2);
        }

        // Test Method to create fake players, works but is client side
        public static void CreateNetworkPlayer()
        {
            //var playerid = PlayerIdManager.GetPlayerId(userid.LongId);
            // ConnectionSender.SendDisconnect(userid, "Cause I said so");
            //InternalServerHelpers.OnUserLeave(userid.LongId);
            // NetworkHelper.KickUser(userid);
            PlayerList list = new PlayerList();
            var id = new PlayerId(76561198979467314 + (ulong)1, (byte)(list.Players.Length + 1), LocalPlayer.Metadata.LocalDictionary, InternalServerHelpers.GetInitialEquippedItems());

            ConnectionRequestData data = new();
            ConnectionSender.SendPlayerJoin(id, data.avatarBarcode, data.avatarStats);

            list.WritePlayers();
            NetworkEntity networkentity = new();
            NetworkPlayer netplayer = new(networkentity, id);
            NetworkEntityManager.IdManager.RegisterEntity(id.SmallId, networkentity);
            InternalLayerHelpers.OnUserJoin(id);
            InternalServerHelpers.OnUserJoin(id, false);
            NetworkLayer.SupportedLayers[0].OnUserJoin(id);
            NetworkLayer.Layers[0].OnUserJoin(id);

                // Get the type of the class containing the internal method
            Type multiplayerHookingType = typeof(MultiplayerHooking);

                // Get the MethodInfo object for the internal method
            System.Reflection.MethodInfo internalOnPlayerJoinMethod = multiplayerHookingType.GetMethod("Internal_OnPlayerJoin", BindingFlags.NonPublic | BindingFlags.Static);

            if (internalOnPlayerJoinMethod != null)
            {
                    // Invoke the internal method
                internalOnPlayerJoinMethod.Invoke(null, new object[] { id });
                list.WritePlayers();
            }
            else
            {
                MelonLogger.Msg("Internal_OnPlayerJoin method not found.");
            }
        }

        // Method that would send a large amount of damage to a player
        public static void DamageUser(PlayerId userid)
        {

            Attack attack = new Attack();
            attack.damage = 9223372036854775807;
            attack.origin = GetPelvis(userid.SmallId).position;
            PlayerSender.SendPlayerDamage(userid, attack);
        }

        // Loop Method that would send damage to a player
        public static IEnumerator DamageUserLoop(PlayerId userid)
        {
            var id = PlayerIdManager.GetPlayerId(userid);
            _loopKillActive = true; // Set the flag to true when starting
            if (userid.LongId == 76561198979467313)
            {
                yield break;
            }
            while (_loopKillActive)
            {
                if(id == null)
                {
                    MelonLogger.Msg("User not found");  
                    _loopKillActive = false;
                    yield break;
                }
                Attack attack = new Attack
                {
                    damage = 9223372036854775807
                };
                PlayerSender.SendPlayerDamage(userid, attack);
                if (userid == null)
                {
                    _loopKillActive = false;
                }
                yield return new WaitForSeconds(1f);
            }
        }

        // Method that would teleport you to the player based off of their ID
        public static void Teleporttoplayer(byte smallid)
        {
            var rig = GetPelvis(smallid);
            MelonLogger.Msg(Player.PhysicsRig.transform.position);
            MelonLogger.Msg(rig.position);
           // Player.PhysicsRig.transform.position = rig.localPosition;
            MelonLogger.Msg(Player.PhysicsRig.transform.position);
            var rm = RigData.Refs.RigManager;
            rm.Teleport(rig.position);
        }

        // Test Method that would spawn a bowling ball and try to launch it
        public static IEnumerator LaunchBowlingball()
        {
            Vector3 position = Player.Head.position + Player.Head.forward * 2;
            int spawned = 0;
            if (!NetworkInfo.HasLayer)
            {
                HelperMethods.SpawnCrate("SLZ.BONELAB.Content.Spawnable.BowlingBall", position);

                GameObject gameObject =  GameObject.Find("BowlingBall [" + spawned + "]");
                Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                rb.AddForce(Player.Head.forward * 10f, ForceMode.Impulse);
            }
            else
            {
                Spawnable spawnable = new Spawnable()
                {
                    crateRef = new SpawnableCrateReference("SLZ.BONELAB.Content.Spawnable.BowlingBall")
                };
                NetworkAssetSpawner.Spawn(new NetworkAssetSpawner.SpawnRequestInfo()
                {
                    spawnable = spawnable,
                    position = position,
                    rotation = Player.Head.rotation
                });
                GameObject ball = GameObject.Find(spawnable.crateRef.Crate.AssetType.Name);
                ball.transform.SetParent(spawnable.crateRef.Crate.MainGameObject.Asset.transform);
                Vector3 forwardforce = Player.RigManager.physicsRig.transform.forward.normalized;
                yield return new WaitForSeconds(0.1f);
                Rigidbody rb = ball.GetComponent<Rigidbody>();
                rb.AddForce(forwardforce * spawn_force, ForceMode.Impulse);
            }
        }

        // Refreshes the player list with a new list of players
        private static void Refresh()
        {
            _playerstuff.RemoveAll();
            _playerstuff.CreateFunction("Refresh", UnityEngine.Color.gray, Refresh);

            foreach(var id in PlayerIdManager.PlayerIds)
            {
                PlayerTest(id);
            }

            BoneLib.BoneMenu.Menu.OpenPage(_playerstuff);
        }

        // Test Method that would get the x,y,z coordinates of a player
        public static void getPlayerPos(byte smallid)
        {
            GameObject rig = new TestPage().FindRigForPlayer(smallid);

            if (rig != null)
            {
                MelonLogger.Msg(rig.transform.forward);
                MelonLogger.Msg(rig.transform.position);
            }
        }


        // Method that would spawn an explosion on the player
        public static void BlowupPlayer(byte smallid)
        {
            MelonLogger.Msg(smallid);
            GameObject rig = new TestPage().FindRigForPlayer(smallid);
            MelonLogger.Msg(rig.transform.rotation);
            var player = GetPelvis(smallid);
            MelonLogger.Msg(player);

            Spawnable spawnable = new Spawnable()
            {
                crateRef = new SpawnableCrateReference("BaBaCorp.MiscExplosiveDevices.Spawnable.ExplosionSmallBigDamage")
            };

            NetworkAssetSpawner.Spawn(new NetworkAssetSpawner.SpawnRequestInfo()
            {
                spawnable = spawnable,
                position = player.position,
                rotation = rig.transform.rotation
            });
        }

        // Method to get the x,y,z coordinates of a player based off the avatar pelvis
        public static Transform GetPelvis(byte smallid)
        {
            var rigmanager = GameObject.Find($"[RigManager (Networked)] (ID {smallid})");
            MelonLogger.Msg(rigmanager);
            if (rigmanager == null)
                throw new InvalidOperationException("Rig not found.");
            var physicsRig = rigmanager.transform.Find("PhysicsRig");
            var pelvis = physicsRig.Find("Pelvis");
            MelonLogger.Msg(pelvis.position);
            if (pelvis == null)
                throw new InvalidOperationException("Pelvis not found.");
            MelonLogger.Msg(pelvis.position);

            return pelvis;
            
            
        }

        // Test Method to get the position of the local player
        public static Vector3 GetPlayerPosition()
        {
            if (!RigData.HasPlayer)
                throw new InvalidOperationException("Player not available.");

            var playerPosition = RigData.Refs.RigManager.physicsRig.feet.transform.position;
            MelonLogger.Msg($"Player position: {playerPosition}");
            return playerPosition;
        }

        // Test method that would get the rig of a player based off the Id
        private GameObject FindRigForPlayer(byte smallId)
        {
            string rigName = $"[RigManager (Networked)] (ID {smallId})";
            return GameObject.Find(rigName);
        }

        // Test Method that would spawn all the guns avaliable in BoneLab, only avaliable for network lobbies
        public static void SpawnAllGuns()
        {
            if (!NetworkInfo.HasLayer)
            {
                Notifier.Send(new BoneLib.Notifications.Notification()
                {
                    Title = (BoneLib.Notifications.NotificationText)"Hold on",
                    Message = (BoneLib.Notifications.NotificationText)"You made this for trolling, duh",
                    PopupLength = 5f,
                    Type = BoneLib.Notifications.NotificationType.Error,
                    ShowTitleOnPopup = true
                });
                throw new Exception("This was made for trolling");
            }
            else
            {
                var position = Player.Head.position + Player.Head.forward * 2;
                foreach (string str in BoneLib.CommonBarcodes.Guns.All)
                {
                    Spawnable spawnable = new Spawnable()
                    {
                        crateRef = new SpawnableCrateReference(str)
                    };
                    NetworkAssetSpawner.Spawn(new NetworkAssetSpawner.SpawnRequestInfo()
                    {
                        spawnable = spawnable,
                        position = position,
                        rotation = Player.Head.rotation
                    });
                }
            }
        }

        // Test method that would spawn all melees in BoneLab, only avaliable for network lobbies
        public static void SpawnAllMelee()
        {
            if (!NetworkInfo.HasLayer)
            {
                Notifier.Send(new BoneLib.Notifications.Notification()
                {
                    Title = (BoneLib.Notifications.NotificationText)"Hold on",
                    Message = (BoneLib.Notifications.NotificationText)"You made this for trolling, duh",
                    PopupLength = 5f,
                    Type = BoneLib.Notifications.NotificationType.Error,
                    ShowTitleOnPopup = true
                });
                throw new Exception("This was made for trolling");
            } else
            {
                var position = Player.Head.position + Player.Head.forward * 2;
                foreach (string str in BoneLib.CommonBarcodes.Melee.All)
                {
                    Spawnable spawnable = new Spawnable()
                    {
                        crateRef = new SpawnableCrateReference(str)
                    };
                    NetworkAssetSpawner.Spawn(new NetworkAssetSpawner.SpawnRequestInfo()
                    {
                        spawnable = spawnable,
                        position = position,
                        rotation = Player.Head.rotation
                    });
                }
            }


        }

        // Method that would spawn in 2 C4 props with one that would explode confetti and one that would explode, only avaliable for network lobbies
        public static void C4Game()
        {
            if (!NetworkInfo.HasLayer)
            {
                Notifier.Send(new BoneLib.Notifications.Notification()
                {
                    Title = (BoneLib.Notifications.NotificationText)"Hold on",
                    Message = (BoneLib.Notifications.NotificationText)"You made this for trolling, duh",
                    PopupLength = 5f,
                    Type = BoneLib.Notifications.NotificationType.Error,
                    ShowTitleOnPopup = true
                });
            }
            else
            {
                var position = Player.Head.position + Player.Head.forward * 2;
                Spawnable spawnable = new Spawnable()
                {
                    crateRef = new SpawnableCrateReference("BaBaCorp.MiscExplosiveDevices.Spawnable.CelebratoryC4")

                };
                Spawnable spawnable2 = new Spawnable()
                {
                    crateRef = new SpawnableCrateReference("BaBaCorp.MiscExplosiveDevices.Spawnable.TimedC4")
                };
                NetworkAssetSpawner.Spawn(new NetworkAssetSpawner.SpawnRequestInfo()
                {
                    spawnable = spawnable,
                    position = position,
                    rotation = Player.Head.rotation
                });
                NetworkAssetSpawner.Spawn(new NetworkAssetSpawner.SpawnRequestInfo()
                {
                    spawnable = spawnable2,
                    position = position,
                    rotation = Player.Head.rotation
                });
            }
        }

        // Method that would spawn in two specific guns, only avaliable for network lobbies
        public static void spawnLoadOut()
        {;
            if (!NetworkInfo.HasLayer)
            {
                Notifier.Send(new BoneLib.Notifications.Notification()
                {
                    Title = (BoneLib.Notifications.NotificationText)"Hold on",
                    Message = (BoneLib.Notifications.NotificationText)"You made this for trolling, duh",
                    PopupLength = 5f,
                    Type = BoneLib.Notifications.NotificationType.Error,
                    ShowTitleOnPopup = true
                });
                throw new Exception("This was made for trolling");
            }
            else
            {

            }
            {
                var position = Player.Head.position + Player.Head.forward * 2;
                Spawnable spawnable = new Spawnable()
                {
                    crateRef = new SpawnableCrateReference(BoneLib.CommonBarcodes.Guns.AKM)
                };
                NetworkAssetSpawner.Spawn(new NetworkAssetSpawner.SpawnRequestInfo
                {
                    spawnable = spawnable,
                    position = position,
                    rotation = Player.Head.rotation
                });
                Spawnable spawnable2 = new Spawnable()
                {
                    crateRef = new SpawnableCrateReference(BoneLib.CommonBarcodes.Guns.M9)
                };
                NetworkAssetSpawner.Spawn(new NetworkAssetSpawner.SpawnRequestInfo
                {
                    spawnable = spawnable2,
                    position = position,
                    rotation = Player.Head.rotation
                });
            }
        }

        // Debug Method that would get the health data of an avatar
        public static void getHealthData()
        {
            Player_Health playerhealth = Player.RigManager.GetComponent<Player_Health>();
            MelonLogger.Msg("Current health: " + playerhealth.curr_Health);
            MelonLogger.Msg("Max Health: " + playerhealth.max_Health);
            MelonLogger.Msg("Health Mode: " + playerhealth.healthMode);
            hp.Value = currenthealth.ToString();
        }
        // Test Method that would change the health values of an avatar to maximum values
        public static void ChangeHealthValues()
        {
            Player_Health playerhealth = Player.RigManager.GetComponent<Player_Health>();
            playerhealth.max_Health = 9223372036854775807;
            playerhealth.curr_Health = 9223372036854775807;
            playerhealth.regenerating = true;
            playerhealth.SetFullHealth();
        }

        // Test method that would download an image and show it on an ImageBoard prop, was never implemented
        public static IEnumerator GetImageFromURL(string url)
        {
            var request = UnityWebRequest.Get(url);
            request.SendWebRequest();
            while (!request.isDone)
            {
                yield return new WaitForSeconds(1f);
            }
            if(request.error != null)
            {
                MelonLogger.Msg(request.error);
            }
            if (request.isDone)
            {
                var image = request.downloadHandler.data;
                var texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                var meshrenderer = Patches.currentimageboard.transform.FindChild("Mesh").FindChild("Picture_Target").GetComponent<MeshRenderer>();
                var fusionrpc = Patches.currentimageboard.transform.FindChild("UltEvents").FindChild("FusionRPC").FindChild("ReceiveData").GetComponent<RPCString>();
                texture.LoadImage(image);
                fusionrpc.SetValue(Convert.ToBase64String(image));
                texture.filterMode = FilterMode.Bilinear;
                texture.wrapMode = TextureWrapMode.Clamp;
                texture.Apply();
                meshrenderer.material.mainTexture = texture;
                meshrenderer.material.SetFloat("_DetailAlbedoScale", 1.0f);
                meshrenderer.material.SetFloat("_DetailNormalScale", 0.0f);
            }
        }
        // Method that would send an API request to an Image Generator and display the generated image on an ImageBoard prop
        public static IEnumerator GenerateAIImage(string prompt)
        {
            var url = "https://image.pollinations.ai/prompt/" + prompt + "?model=flux&nologo=true&enhance=true";
            if(Patches.currentimageboard == null)
            {
                Notifier.Send(new Notification()
                {
                    Title = (BoneLib.Notifications.NotificationText)"Uh oh",
                    Message = (BoneLib.Notifications.NotificationText)"You don't have an active Imageboard",
                    PopupLength = 1.5f,
                    Type = BoneLib.Notifications.NotificationType.Error,
                    ShowTitleOnPopup = true
                });
            }
            var request = UnityWebRequest.Get(url);
            request.SendWebRequest();
            Notifier.Send(new Notification()
            {
                Title = (BoneLib.Notifications.NotificationText)"Generating Image",
                Message = (BoneLib.Notifications.NotificationText)"Generating image from AI",
                PopupLength = 1.5f,
                Type = BoneLib.Notifications.NotificationType.Information,
                ShowTitleOnPopup = true
            });
            while (!request.isDone)
            {
                yield return new WaitForSeconds(1f);
            }
            if (request.error != null)
            {
                MelonLogger.Msg(request.error);
                Notifier.Send(new Notification()
                {
                    Title = (BoneLib.Notifications.NotificationText)"Error",
                    Message = (BoneLib.Notifications.NotificationText)"An error occurred when generating the image, try again",
                    PopupLength = 2f,
                    Type = BoneLib.Notifications.NotificationType.Error,
                    ShowTitleOnPopup = true
                });
            }
            if(request.isDone)
            {
                var image = request.downloadHandler.data;
                var texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                var meshrenderer = Patches.currentimageboard.transform.FindChild("Mesh").FindChild("Picture_Target").GetComponent<MeshRenderer>();
                var fusionrpc = Patches.currentimageboard.transform.FindChild("UltEvents").FindChild("FusionRPC").FindChild("ReceiveData").GetComponent<RPCString>();
                texture.LoadImage(image);
                fusionrpc.SetValue(Convert.ToBase64String(image));
                texture.filterMode = FilterMode.Bilinear;
                texture.wrapMode = TextureWrapMode.Clamp;
                texture.Apply();

                meshrenderer.material.mainTexture = texture;

                Notifier.Send(new Notification()
                {
                    Title = (BoneLib.Notifications.NotificationText)"Image Generated",
                    Message = (BoneLib.Notifications.NotificationText)"Finished Generating!",
                    PopupLength = 1.5f,
                    Type = BoneLib.Notifications.NotificationType.Success,
                });

                meshrenderer.material.SetFloat("_DetailAlbedoScale", 1.0f);
                meshrenderer.material.SetFloat("_DetailNormalScale", 0.0f);
            }
        }
        // Method that would set the last held ImageBoard prop to the selected image
        public static void SetCurrentImageBoard()
        {
            var image = Patches.selectedimage;
            string imagePath = Path.Combine(MelonEnvironment.UserDataDirectory, "Images", image);
            byte[] imagedata = File.ReadAllBytes(imagePath);
            var texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            var meshrenderer = Patches.currentimageboard.transform.FindChild("Mesh").FindChild("Picture_Target").GetComponent<MeshRenderer>();
            var fusionrpc = Patches.currentimageboard.transform.FindChild("UltEvents").FindChild("FusionRPC").FindChild("ReceiveData").GetComponent<RPCString>();
            texture.LoadImage(imagedata);
            fusionrpc.SetValue(Convert.ToBase64String(imagedata));
            texture.filterMode = FilterMode.Bilinear;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply();

            meshrenderer.material.mainTexture = texture;

            meshrenderer.material.SetFloat("_DetailAlbedoScale", 1.0f);
            meshrenderer.material.SetFloat("_DetailNormalScale", 0.0f);
        }

        // Method that would apply the selected image to all spawned ImageBoard props
        public static IEnumerator ApplyImageToAllImageBoards()
        {
            Poolee[] poolees = UnityEngine.Object.FindObjectsOfType<Poolee>();
            foreach (var poolee in poolees)
            {
                if (!poolee.name.StartsWith("ImageBoard"))
                {
                    continue;
                }
                var meshrenderer = poolee.gameObject.transform.FindChild("Mesh").FindChild("Picture_Target").GetComponent<MeshRenderer>();
                var fusionrpc = poolee.gameObject.transform.FindChild("UltEvents").FindChild("FusionRPC").FindChild("ReceiveData").GetComponent<RPCString>();
                var texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                string imagepath = Path.Combine(MelonEnvironment.UserDataDirectory, "Images", Patches.selectedimage);
                byte[] imagebytes = File.ReadAllBytes(imagepath);
                texture.LoadImage(imagebytes);
                fusionrpc.SetValue(Convert.ToBase64String(imagebytes));
                texture.filterMode = FilterMode.Bilinear;
                texture.wrapMode = TextureWrapMode.Clamp;
                texture.Apply();

                meshrenderer.material.mainTexture = texture;

                meshrenderer.material.SetFloat("_DetailAlbedoScale", 1.0f);
                meshrenderer.material.SetFloat("_DetailNormalScale", 0.0f);
                yield return new WaitForSeconds(0.1f);

            }
        }
        // Method that refreshes the list of images in the users "Images" folder in the Melon Enviroments UserData folder
        public static void RefreshImages()
        {
            imageboard.RemoveAll();
            for(int i = 0; i < Patches.images.Length; i++)
            {
                Patches.images[i].Remove(i);
            }
            var readd_images = System.IO.Directory.GetFiles(Path.Combine(MelonEnvironment.UserDataDirectory, "Images"));
          //  var read_video_folders = System.IO.Directory.GetDirectories(MelonEnvironment.UserDataDirectory, "Video Test");
            Patches.images = readd_images;
            images = imageboard.CreatePage("Images", UnityEngine.Color.green);
           // videos = imageboard.CreatePage("Videos", UnityEngine.Color.green);
            var refresh = imageboard.CreateFunction("Refresh", UnityEngine.Color.green, () => RefreshImages());
            var setcurrentimageboard = imageboard.CreateFunction("Set Current Image Board", UnityEngine.Color.green, () => SetCurrentImageBoard());
            var applyimagetoimageboards = imageboard.CreateFunction("Apply Image to All Image Boards", UnityEngine.Color.green, () => MelonCoroutines.Start(ApplyImageToAllImageBoards()));
            imageboard.CreateString("Generate Image", UnityEngine.Color.cyan, "", (str) =>
            {
                MelonCoroutines.Start(GenerateAIImage(str));
            });
            foreach (var image in Directory.GetFiles(Path.Combine(MelonEnvironment.UserDataDirectory, "Images")))
            {
                var name = Path.GetFileName(image);
                images.CreateFunction(name, UnityEngine.Color.green, () =>
                {
                    Patches.selectedimage = name;
                    MelonLogger.Msg(Patches.selectedimage);
                });
            }
          //  foreach(var video in read_video_folders)
            BoneLib.BoneMenu.Menu.OpenPage(imageboard);
        }

        // Method that would continue to spawn a prop until the user stops it
        private static IEnumerator LoopSpawnCoroutine()
        {
            while (true)
            {
                SpawnBarcode();
                yield return new WaitForSeconds(loopspawninterval);
            }
        }
        // Method that would continue to spawn a prop on a player until the user stops it
        private static IEnumerator LoopSpawnPlayerCoroutine(PlayerId id)
        {
            while (true)
            {
                SpawnBarcodeOnPlayer(id);
                if(id == null)
                {
                    yield break;
                }
                yield return new WaitForSeconds(loopspawninterval);
            }
        }
    }
}
