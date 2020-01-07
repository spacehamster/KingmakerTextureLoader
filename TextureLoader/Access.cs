using Kingmaker.Visual.CharacterSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TextureLoader
{
    public static class Access
    {
        public static Harmony12.AccessTools.FieldRef<EquipmentEntity.OutfitPart, GameObject> OutfitPart_Prefab;
        public static Harmony12.AccessTools.FieldRef<EquipmentEntity.OutfitPart, Material> OutfitPart_Material;
        public static Harmony12.AccessTools.FieldRef<EquipmentEntity, List<Texture2D>> PrimaryRamps;
        public static Harmony12.AccessTools.FieldRef<EquipmentEntity, List<Texture2D>> SecondaryRamps;
        public static Harmony12.AccessTools.FieldRef<CharacterTextureDescription, Texture2D> CharacterTextureDescription_Texture;
        public static void Init()
        {

            OutfitPart_Prefab = Harmony12.AccessTools.FieldRefAccess<EquipmentEntity.OutfitPart, GameObject>("m_Prefab");
            OutfitPart_Material = Harmony12.AccessTools.FieldRefAccess<EquipmentEntity.OutfitPart, Material>("m_Material");
            PrimaryRamps = Harmony12.AccessTools.FieldRefAccess<EquipmentEntity, List<Texture2D>>("m_PrimaryRamps");
            SecondaryRamps = Harmony12.AccessTools.FieldRefAccess<EquipmentEntity, List<Texture2D>>("m_SecondaryRamps");
            CharacterTextureDescription_Texture = Harmony12.AccessTools.FieldRefAccess<CharacterTextureDescription, Texture2D>("m_Texture");
        }
    }
}
