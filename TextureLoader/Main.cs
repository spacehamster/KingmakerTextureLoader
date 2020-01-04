using UnityEngine;
using UnityModManagerNet;
using static UnityModManagerNet.UnityModManager;
using System;
using System.Reflection;
using System.IO;
using Kingmaker.Visual.CharacterSystem;
using Kingmaker.ResourceLinks;
using System.Collections.Generic;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.Items.Equipment;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;

namespace TextureLoader
{
#if DEBUG
    [EnableReloading]
#endif
    public static class Main
    {
        public static bool enabled;
        public static ModEntry modEntry;
        public static Dictionary<string, List<BlueprintDescription>> BlueprintDescriptions = new Dictionary<string, List<BlueprintDescription>>();
        public static Dictionary<string, List<EquipmentEntityDescription>> EquipmentEntityDescriptions = new Dictionary<string, List<EquipmentEntityDescription>>();
        public static void Log(string text)
        {
            modEntry?.Logger?.Log(text);
        }
        public static void Error(string text)
        {
            modEntry?.Logger?.Log(text);
        }
        public static void Error(Exception exception)
        {
            modEntry?.Logger?.Log(exception.ToString());
        }
        static bool Load(ModEntry modEntry)
        {
            Main.modEntry = modEntry;
            try
            {
                Access.Init();
                LoadDescriptions(modEntry);
                var harmony = Harmony12.HarmonyInstance.Create(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                modEntry.OnToggle = OnToggle;
#if DEBUG
                modEntry.OnUnload = Unload;
#endif
            }
            catch (Exception ex)
            {
                Error(ex);
                return false;
            }
            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }
        static bool Unload(UnityModManager.ModEntry modEntry)
        {
            Harmony12.HarmonyInstance.Create(modEntry.Info.Id).UnpatchAll(modEntry.Info.Id);
            return true;
        }
        static void LoadDescriptions(UnityModManager.ModEntry modEntry)
        {
            foreach(var path in Directory.GetFiles(modEntry.Path, "*.json", SearchOption.AllDirectories))
            {
                var fileName = Path.GetFileNameWithoutExtension(path);
                if (fileName == "info" || fileName == "settings") continue;
                try
                {
                    using (StreamReader file = File.OpenText(path))
                    {
                        string jsonDir = Path.GetDirectoryName(path);
                        JsonSerializer serializer = new JsonSerializer();
                        TextureDescription description = (TextureDescription)serializer.Deserialize(file, typeof(TextureDescription));
                        if (description.Blueprints == null)
                        {
                            Main.Error($"Error loading blueprint description from {path}");
                            continue;
                        }
                        if (description.EquipmentEntities == null)
                        {
                            Main.Error($"Error loading equipment entities description from {path}");
                            continue;
                        }
                        foreach (var blueprint in description.Blueprints)
                        {
                            blueprint.Load(jsonDir);
                            if(BlueprintDescriptions.ContainsKey(blueprint.Id) &&
                                BlueprintDescriptions[blueprint.Id].Any(item => item.Type == blueprint.Type))
                            {
                                Main.Error($"Warning - Duplicate texture replacement found for blueprint {blueprint.Id}");
                            }
                            BlueprintDescriptions.AddContainer(blueprint.Id, blueprint);
                        }
                        foreach (var equipmentEntity in description.EquipmentEntities)
                        {
                            equipmentEntity.Load(jsonDir);
                            if (EquipmentEntityDescriptions.ContainsKey(equipmentEntity.Id) &&
                                EquipmentEntityDescriptions[equipmentEntity.Id].Any(item => item.Type == equipmentEntity.Type && item.Replaces == equipmentEntity.Replaces))
                            {
                                Main.Error($"Warning - Duplicate texture replacement found for equipment entity {equipmentEntity.Id}");
                            }
                            EquipmentEntityDescriptions.AddContainer(equipmentEntity.Id, equipmentEntity);
                        }
                        Main.Log($"Loaded texture pack {path}");
                    }
                } catch(Exception ex)
                {
                    Error(ex);
                }
            }
        }
    }
    [Harmony12.HarmonyPatch(typeof(AssetBundle), "LoadFromFile", new Type[] { typeof(string) })]
    static class AssetBundle_LoadFromFilePatch
    {
        static void Postfix(string path, ref AssetBundle __result)
        {
            try
            {
                var assetId = Path.GetFileName(path).Replace("resource_", "");
                if (Main.EquipmentEntityDescriptions.TryGetValue(assetId, out List<EquipmentEntityDescription> items)){
                    var equipmentEntity = __result.LoadAllAssets<EquipmentEntity>()[0];
                    if (equipmentEntity == null)
                    {
                        Main.Error($"{assetId} is not an Equipment Bundle");
                        return;
                    }
                    foreach (var item in items)
                    {
                        item.ReplaceTexture(equipmentEntity);
                    }
                }
            }
            catch (Exception ex)
            {
                Main.Error(ex);
            }
        }
    }
    [Harmony12.HarmonyPatch(typeof(LibraryScriptableObject), "LoadDictionary", new Type[0])]
    static class LibraryScriptableObject_LoadDictionary_Patch
    {
        static bool initialized = false;
        static void Postfix(LibraryScriptableObject __instance)
        {
            try
            {
                if (initialized) return;
                initialized = true;
                foreach(var kv in Main.BlueprintDescriptions)
                {
                    string assetId = kv.Key;
                    BlueprintScriptableObject blueprint;
                    if (!ResourcesLibrary.LibraryObject.BlueprintsByAssetId.TryGetValue(assetId, out blueprint))
                    {
                        Main.Error($"{assetId} is not a valid blueprint Id");
                        continue;
                    }
                    var descriptions = kv.Value;
                    foreach(var description in descriptions)
                    {
                        description.ReplaceTexture(blueprint);
                    }
                }
            }
            catch (Exception ex)
            {
                Main.Error(ex);
            }
        }
    }
}