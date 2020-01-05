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
        public static Harmony12.AccessTools.FieldRef<EquipmentEntity.OutfitPart, GameObject> PrefabRef;
        public static Harmony12.AccessTools.FieldRef<EquipmentEntity, List<Texture2D>> PrimaryRamps;
        public static Harmony12.AccessTools.FieldRef<EquipmentEntity, List<Texture2D>> SecondaryRamps;
        public static Harmony12.AccessTools.FieldRef<CharacterTextureDescription, Texture2D> CharacterTextureDescription_Texture;
        public static void Init()
        {

            PrefabRef = Harmony12.AccessTools.FieldRefAccess<EquipmentEntity.OutfitPart, GameObject>("m_Prefab");
            PrimaryRamps = Harmony12.AccessTools.FieldRefAccess<EquipmentEntity, List<Texture2D>>("m_PrimaryRamps");
            SecondaryRamps = Harmony12.AccessTools.FieldRefAccess<EquipmentEntity, List<Texture2D>>("m_SecondaryRamps");
            CharacterTextureDescription_Texture = Harmony12.AccessTools.FieldRefAccess<CharacterTextureDescription, Texture2D>("m_Texture");
        }
    }
}
