using Harmony12;
using Kingmaker.Visual.CharacterSystem;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace TextureLoader
{
    public class EquipmentEntityDescription
    {
        public string Id;
        public string Texture;
        public ReplacementType Type;
        public string Replaces;
        [JsonIgnore]
        public Texture2D m_Texture;
        [JsonIgnore]
        private Action<EquipmentEntity> m_Setter;
        static private Action<EquipmentEntity> DummySetter = (ee) => { };
        public void Load(string path)
        {
            m_Texture = Util.LoadTexture(Path.Combine(path, Texture));
        }
        private void InitSetter(EquipmentEntity ee)
        {
            if (Type == ReplacementType.BodyPart)
            {
                var bpIndex = ee.BodyParts
                    .FindIndex(bp => bp.Textures[0].GetMainTextureName() == Replaces);
                if (bpIndex < 0)
                {
                    Main.Error($"Could not find bodypart for EE[{ee.name}].BodyParts[{Replaces}]");
                    m_Setter = DummySetter;
                    return;
                }
                m_Setter = (_ee) =>
                {
                    var ctd = _ee.BodyParts[bpIndex].Textures[0];
                    Access.CharacterTextureDescription_Texture(ctd) = m_Texture;
                };
            }
            else if (Type == ReplacementType.OutfitPartPrefab)
            {
                var opIndex = ee.OutfitParts
                    .FindIndex(op => Access.OutfitPart_Prefab(op).gameObject.name == Replaces);
                if (opIndex < 0)
                {
                    Main.Error($"Could not find OutfitPartPrefab for EE[{ee.name}].OutfitParts[{Replaces}]");
                    m_Setter = DummySetter;
                    return;
                }
                var prefab = Access.OutfitPart_Prefab(ee.OutfitParts[opIndex]);
                var mat = prefab?.GetComponentInChildren<Renderer>()?.material;
                if(mat == null)
                {
                    Main.Error($"Could not find material for EE[{ee.name}].OutfitParts[{prefab.name}]");
                    m_Setter = DummySetter;
                    return;

                }
                m_Setter = (_ee) =>
                {
                    var _prefab = Access.OutfitPart_Prefab(_ee.OutfitParts[opIndex]);
                    var _mat = _prefab.GetComponentInChildren<Renderer>().material;
                    _mat.mainTexture = m_Texture;
                };
            }
            else if (Type == ReplacementType.OutfitPartMaterial)
            {
                var opIndex = ee.OutfitParts
                    .FindIndex(op => Access.OutfitPart_Material(op)?.name == Replaces);
                if (opIndex < 0)
                {
                    Main.Error($"Could not find OutfitPartMaterial for EE[{ee.name}].OutfitParts[{Replaces}]");
                    m_Setter = DummySetter;
                    return;
                }
                m_Setter = (_ee) =>
                {
                    var _mat = Access.OutfitPart_Material(_ee.OutfitParts[opIndex]);
                    _mat.mainTexture = m_Texture;
                };
            }
            else if (Type == ReplacementType.ColorProfilePrimary)
            {
                var index = ee.ColorsProfile.PrimaryRamps.FindIndex(t => t.name == Replaces);
                if (index < 0)
                {
                    Main.Error($"Could not find {Replaces} in {ee.name}({Id}) {Type}");
                    m_Setter = DummySetter;
                    return;
                }
                m_Setter = (_ee) =>
                {
                    _ee.ColorsProfile.PrimaryRamps[index] = m_Texture;
                };
            }
            else if (Type == ReplacementType.ColorProfileSecondary)
            {
                var index = ee.ColorsProfile.SecondaryRamps.FindIndex(t => t.name == Replaces);
                if (index < 0)
                {
                    Main.Error($"Could not find {Replaces} in {ee.name}({Id}) {Type}");
                    m_Setter = DummySetter;
                    return;
                }
                m_Setter = (_ee) =>
                {
                    _ee.ColorsProfile.SecondaryRamps[index] = m_Texture;
                };
            }
            else if (Type == ReplacementType.PrimaryRamp)
            {
                var ramp = Access.PrimaryRamps(ee);
                var index = ramp.FindIndex(t => t.name == Replaces);
                if (index < 0)
                {
                    Main.Error($"Could not find {Replaces} in {ee.name}({Id}) {Type}");
                    m_Setter = DummySetter;
                    return;
                }
                m_Setter = (_ee) =>
                {
                    var _ramp = Access.PrimaryRamps(ee);
                    _ramp[index] = m_Texture;
                };
            }
            else if (Type == ReplacementType.SecondaryRamp)
            {
                var ramp = Access.SecondaryRamps(ee);
                var index = ramp.FindIndex(t => t.name == Replaces);
                if (index < 0)
                {
                    Main.Error($"Could not find {Replaces} in {ee.name}({Id}) {Type}");
                    m_Setter = DummySetter;
                }
                m_Setter = (_ee) =>
                {
                    var _ramp = Access.SecondaryRamps(ee);
                    _ramp[index] = m_Texture;
                };
            }
            else
            {
                m_Setter = DummySetter;
                Main.Error($"Invalid replacement type {Type} for {Id}");
            }
        }
        public void ReplaceTexture(EquipmentEntity ee)
        {
            if (m_Setter == null) InitSetter(ee);
            m_Setter(ee);
        }
    }
}
