

# Kingmaker Texture Loader

Mod to custom texture replacements for the Game Pathfinder Kingmaker.
## Install
1. Download and install [Unity Mod Manager](https://www.nexusmods.com/site/mods/21)
2. Download the [mod](https://github.com/spacehamster/KingmakerVisualAdjustmentsMod/releases)
3. Extract the archive and put the mod folder into 'Mods' folder of the Game (\Steam\steamapps\common\Pathfinder Kingmaker\Mods\TextureLoader)
4. Run the game and load your save
5. Copy any texture packs into the new TextureLoader folder as a subfolder.

## Creating texture packs

First download the [blueprint dumps](https://github.com/spacehamster/KingmakerCustomBlueprints/releases/tag/blueprints) and [Asset Studio](https://github.com/Perfare/AssetStudio)

Find the model you are interesting in modifying in the blueprint dump. 

Weapons are are a type of blueprint found in `Kingmaker.Blueprints.Items.Weapons.BlueprintItemWeapon`. Weapons without custom models are in `Kingmaker.Blueprints.Items.Weapons.BlueprintWeaponType` and are a type of `Blueprint`. Armor, clothes and playable race body models are found in `Kingmaker.Visual.CharacterSystem.EquipmentEntity` are a type of `EquipmentEntity`. 

Existing textures can be inspected and extracted with Asset Studio.

Then create a json file named `Textures.json` describing the texture replacements you wish to make. 

```
{
	"Blueprints" : 
	[
		//BlueprintReplacements
	],
    "EquipmentEntity" : 
	[
		//EquipmentEntity
	],
}
```

### Blueprint Replacement Format

| Name    | Description                                                  |
| ------- | ------------------------------------------------------------ |
| Id      | The `m_AssetGuid` of the blueprint which holds the target model |
| Type    | The type of model to effect. Can be `Weapon` or `Sheath`     |
| Texture | The path of the texture to add load to the texture. Paths are relative to the `Textures.json` |

### Equipment Entity Format Replacement Format

| Name     | Description                                                  |
| -------- | ------------------------------------------------------------ |
| Id       | The `m_AssetGuid` of the blueprint which holds the target model |
| Type     | The type of model to effect. Can be `BodyPart`, `OutfitPart`, `ColorProfilePrimary`, `ColorProfileSecondary`, `PrimaryRamp`or `SecondaryRamp` |
| Replaces | The name of the texture or model to replace on the target.   |
| Texture  | The path of the texture to add load to the texture. Paths are relative to the `Textures.json` |

See [here](https://github.com/spacehamster/KingmakerTextureLoader/tree/master/TestTextures/TestPack) for an example texture pack.

