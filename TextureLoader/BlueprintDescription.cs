using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.Items.Weapons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TextureLoader
{
    public class BlueprintDescription
    {
        public string Id;
        public string Texture;
        public ReplacementType Type;
        [JsonIgnore]
        private Texture2D m_Texture;
        public void Load(string path)
        {
            m_Texture = Util.LoadTexture(Path.Combine(path, Texture));
        }
        public void ReplaceTexture(BlueprintScriptableObject blueprint)
        {
            if (m_Texture == null) return;
            var visualParameters = GetVisualParameters(blueprint);
            if(Type == ReplacementType.Weapon)
            {
                visualParameters.Model.GetComponentInChildren<MeshRenderer>().sharedMaterial.mainTexture = m_Texture;
            } else if(Type == ReplacementType.Sheath)
            {
                visualParameters.SheathModel.GetComponentInChildren<MeshRenderer>().sharedMaterial.mainTexture = m_Texture;
            } else
            {
                Main.Error($"Invalid replacement type {Type} for {Id}");
            }
        }
        private WeaponVisualParameters GetVisualParameters(BlueprintScriptableObject blueprint)
        {
            if (blueprint is BlueprintItemEquipmentHand weapon)
            {
                return weapon.VisualParameters;
            }
            else if (blueprint is BlueprintWeaponType weaponType)
            {
                return weaponType.VisualParameters;
            }
            else
            {
                var typeName = blueprint?.GetType().Name ?? "Null";
                throw new Exception($"Can't get visual parameters from blueprint type {typeName}");
            }
        }
    }
}
