# EasyCards

#### Custom Cards Made Easy
This mod allows you to easily add custom cards into the game by specifying them in `JSON` files.

To add your own cards, create a new folder inside `BepInEx\plugins` and create a new `.cards.json` file (like `custompack.cards.json`) inside that folder.
An example structure could look like this:

```
BepInEx
  - plugins
    - ChendraksCardPack
      - ccp.cards.json
      - Assets
        - card1.png
        - card2.png
```

EasyCards will scan for `*.cards.json` files in `BepInEx\plugins` subfolders.

Then start up the game and go to `Stat -> Soul Cards`. If everything went well, your cards should show up.

The full file format is specified below.

## Troubleshooting
If your cards don't show up, please check `BepInEx\LogOutput.log` and `BepInEx\ErrorLog.log` for potential ideas as to why.
If there is no information in the logs, you can enable additional logs by opening `BepInEx\config\EasyCards.cfg` and setting
`LogCards = true`. After saving the file, restart the app and EasyCards will log A LOT of information about your cards and
maybe give you additional information.

If all else fails, feel free to swing by the [Rogue: Genesia Discord](https://discord.gg/WbrgtCaP4T) and ask there.

## Changelog

#### 1.1.14

* Fix for most recent game version

#### 1.1.13

* Add Steam Workshop support
* Add Debug Log option to mod options

#### 1.1.12

* Fix for the most recent game changes regarding localization

#### 1.1.11

* Fix for the mod not working in more recent versions of the game.

#### 1.1.10

* Fix for most recent game updates
* Due to internal changes in the game with the release of avatars, the ability to change the character sprite using a card has been removed.

#### 1.1.9

* Fixed an issue with loading existing card packs 

#### 1.1.8

* Fixed another internal issue 

#### 1.1.7

* Fix some internals that help with creating cards
* To log all cards' information, you can now press `CTRL + SHIFT + L`, as opposed to having to enable it in the config. 

#### 1.1.6

**BREAKING CHANGES:**

* The most recent beta introduced a lot of internal changes. This update makes the mod compatible with that most recent version.
* For existing card packs to be compatible again, the `Tags` element in each card needs to be empty OR be removed.

**Additional changes:**

* The mod now checks the game version for compatibility and won't load on an unsupported version. Meaning: This version will only work on the current beta!
* Update Card Tag documentation to the new ones in the game
* Added `CardDropChance_*` modifiers documentation

#### 1.1.5

* Add `OnTakeDamage` trigger
* Add `DamageTaken` activation requirement
* Add `EnemiesKilled` activation requirement

#### 1.1.4

* Add ability to have stats shown on effect cards
* Fixed bug where stats wouldn't apply to cards with effects
* Fixed bug where applied skins wouldn't reset in all cases after a run 

#### 1.1.3

* Reset character animations back to Rog after the end of a run
* Fixed a small bug that would happen when you define a card without any modifiers
* Fixed errors in the example effects

#### 1.1.2

* Moved Effects logic to stat cards and got rid of the `Effects` section
* The `Stats` section in JSON files should now be called `StatCards`. Old files will still work.
* Fixed a bug that would trigger boss & elite kill events on normal mobs

#### 1.1.1

* Revert to old packaging, since the new packaging didn't work for some people.

#### 1.1.0

* Add support for cards with effects.

#### 1.0.16

* Add ability to disable cards in rogs or survivors mode
* Fix an issue that could happen if `TexturePath` wasn't set

#### 1.0.15

* Update logic to support mod managers: EasyCards will now scan for `*.cards.json` in `BepInEx\plugins` subfolders.
* Clean up directory structure
* If a cards image can't be loaded, EasyCards will now assign the placeholder image by default
* Updated this documentation with the changes

#### 1.0.14

* Fix issue when loading multiple card packs. Thanks @Grand and @PlushPaws

#### 1.0.13

* Fixing removals... *flips table*

#### 1.0.12

* Fixing requirements - Thanks @PlushPaws for reporting

#### 1.0.11

* Re-add missing dll - Thanks @PlushPaws for reporting

#### 1.0.10

* Some internal code clean up
* Preparation for more advanced features that are in the pipeline

#### 1.0.9

* Fix for stat requirements - Thanks @PlushPaws

#### 1.0.8

* Fix for banishing cards - Thanks @PlushPaws for pointing it out and being persistent

#### 1.0.7

* Code restructure
* Initial Release on Thunderstore

## File Format
```json
{
  // The name you want to show up in-game for your cards
  // Optional, will default to EasyCards if not provided
  "ModSource": "Your Mod Name Here",
  "StatCards": [
    {
      // Internal name of your new card. This needs to be unique.
      // Consider using a prefix for all your cards.
      // Required
      "Name": "CCP_Card1", 
      
      // The path to the image you want displayed on the card.
      // This path is relative to where your json file is on your hard drive.
      // Note: Most of the games card assets are 32x32 pixels.
      // Required
      "TexturePath": "CustomCardPack/Card1.png",
      
      // The rarity of the card. See the list below for options.
      // Required
      "Rarity": "Epic",
      
      // The tags the card has. See the list below for options.
      // Optional
      "CardTags": [ "Moon", "Sun" ],
      
      // How likely is this card to drop?
      // Required
      "DropWeight": 0.10,
      
      // How likely is the card to re-appear after you got it
      // Required
      "LevelUpWeight": 0.10,
      
      // The maximum level of the card. If higher than 1, all stats will
      // be multiplied by the level when you level up.
      // Required
      "MaxLevel": 1,
      
      // A list of modifiers. See below for details
      // Requiered (at least 1)
      "Modifiers": [
        {
          "ModifierValue": 15,
          "ModifierType": "Additional",
          "Stat": "CriticalChance"
        }
      ],
      
      // Translations for the your cards name
      // Required
      "NameLocalization": {
        "en": "My First Card"
      },

      // Translations for the your cards descriptions
      // Optional
      "DescriptionLocalization": {
        "en": "My First Cards Description"
      },

      // For this card to show up, ANY of the below is required
      // Optional
      "RequiresAny": {
        "Cards": [{"Name": "Egg", "Level": 1}],
        "Stats": {
          "StatRequirements": [{"Name":  "Damage", "Value": 8999}],
          "RequirementType": "Min"
        }
      },
      
      // For this card to show up, ALL of the below is required.
      // The format is the same as `RequiresAny`
      // Optional
      "RequiresAll": {},
      
      // The names of cards you want to banish when your card is selected.
      // Can be any custom card or card that is included with the game.
      // Only blocks them from showing up again, if you have them, they will
      // stay in your inventory.
      // Optional
      "BanishesCardsByName": [ "Katana", "VoidSpirit" ],
      
      // The modifier names that you want this card to banish.
      // This example banishes every card that modifies `DamageMitigation`
      // Optional
      "BanishesCardsWithStatsOfType": [ "DamageMitigation" ],

      // When you select this card, remove all listed cards from your inventory 
      // Optional
      "RemovesCards": [ "Katana", "VoidSpirit" ],

      // Allows you to disable the card in a Rogs or Survivors mode. 
      // Optional
      "DisabledInMode": "Rogs",
        
      "Effects": [
      {
          // The internal name for your effect. It's not used for anything outside of logging.
          // If you don't provide one, it will be generated.
          // Optional
          "Name": "MyAwesomeEffect",
      
          // The type of the effect. See below for all possible values.
          // Required
          "Type": "OneTime",
      
          // What is required for the effect to be enabled?
          // Optional, defaults to None
          "ActivationRequirement": "None",
      
          // The action to be taken when the effect is applied.
          // Required
          "Action": "HealAmount",
      
          // What needs to happen for the effect to apply? The effect needs to be enabled for this.
          // Required for some Types
          "Trigger": "OnEliteKill",
      
          // Allows you to configure certain aspects of an effect.
          // Required
          "Properties": {
            // The exact amount for Action.
            // Example: [Action = HealAmount, Amount = 10] would heal you for 10 when the effect is triggered.
            // Required for some Actions.
            "Amount": 1.0,
      
            // The percentage for the effects Action.
            // Example: [Action = HealPercentage, Percentage = 10] would heal you for 10% of your max hp
            // when the effect is triggered.
            // Required for [Action = HealPercentage]
            "Percentage": 12,
      
            // The duration of the effect. The effect will be disabled after [Duration] expires.
            // Specified in seconds. Fractions of a second can be defined as 0.1 for 100ms.
            // Required for [Type = Duration]
            "Duration": 12,
      
            // Amount of time between effect activations.
            // Example: [Action = HealAmount, Amount = 20, Interval = 30] -> Heal you for 20 hp every 30s.
            // Specified in seconds. Fractions of a second can be defined as 0.1 for 100ms.
            // Required for [Type = Interval]
            "Interval": 30
          }
      }
      ]
    }
  ]
}
```

## Card Properties

### Rarity
The rarity of the card.

Possible values: `Tainted`, `Normal`, `Uncommon` , `Rare`, `Epic`, `Heroic`, `Ascended`, `Synergy`, `Evolution`

### Tags
Tags for your card. Think of them as grouping them.

Possible values: `None`, `Moon`, `Sun`, `Fire`, `Wind`, `Hunt`, `Wild`, `Void`, `Dark`, `Metal`

### DisabledInMode
Represents the mode this card will be disabled in.

Possible values: `Rogs`, `Survivors`

### Modifiers
Specifies modifiers that the card applies to your stats.

Example:
```json
{
  "ModifierValue": 15,
  
  // Specifies HOW the `ModifierValue` is applied
  "ModifierType": "Additional",
  
  // Specifies WHICH stat this modifier applies to
  "Stat": "CriticalChance"
}
```

#### Stat
A stat that your card modifies.

Possible values: `MaxHealth`, `HealthRegen`, `Defence`, `DamageMitigation`, `XPMultiplier`, `PickUpDistance`, `AdditionalProjectile`, `ProjectilePiercing`, `ProjectileLifeTime`, `ProjectileSpeed`, `ProjectileSize`, `AreaSize`, `KnockBack`, `MoveSpeed`, `AttackCoolDown`, `AttackDelay`, `Damage`, `CriticalChance`, `CriticalMultiplier`, `DashSpeed`, `DashDuration`, `DashDelay`, `DashCoolDown`, `DashCharge`, `DashChargePerCoolDown`, `GoldMultiplier`, `SoulCoinMultiplier`, `DefencePiercing`, `Corruption`, `AnachronisticDurationMultiplier`, `CardDropChance_Tainted`, `CardDropChance_Normal`, `CardDropChance_Uncommon`, `CardDropChance_Rare`, `CardDropChance_Epic`, `CardDropChance_Heroic`, `CardDropChance_Ascended`, `CardDropChance_Synergy`, `CardDropChance_Evolution`, `CardDropChance_Moon`, `CardDropChance_Sun`, `CardDropChance_Fire`, `CardDropChance_Wind`, `CardDropChance_Hunt`, `CardDropChance_Wild`, `CardDropChance_Void`, `CardDropChance_Dark`, `CardDropChance_Metal`

#### ModifierType
_How_ does your card modify a stat?

Possible values: `Additional`, `Multiplier`, `Compound`

**NOTE:** For most multiplication cases, you will want to use `Compound`, especially for negative multiplisers.


### RequiresAny & RequiresAll

Example
```json
"RequiresAny": {
    "Cards": [
        {"Name": "Card1", "Level": 1},
        {"Name": "Card2", "Level": 3},
    ],
    "Stats": {
      "StatRequirements": [
        {"Name":  "Damage", "Value": 8999},
        {"Name":  "Corruption", "Value": 10},
      ],
      "RequirementType": "Min"
    }
},
```

#### Cards
A list of cards that are required. Each entry looks like this:
```json
{"Name": "Card1", "Level": 2},
```

`Name` is the internal name of the card, `Level` is the minimum level.

So in the above example, the requirement is fulfilled if you have `Card1` at `Level` 2 in your inventory.

#### Stats
Stat requirements are more complex. An entry looks like this:
```json
"Stats": {
  "StatRequirements": [
    {"Name": "Damage", "Value": 8999},
    {"Name": "Corruption", "Value": 10},
  ],
  "RequirementType": "Min"
}
```

`StatRequirements` contains a list of stats `Stat` names and values to fulfill. `RequirementType` specifies if the stat requirements are `Min`(imums) or `Max`(imums).

That means, the above example translates to:
`For this card to show up, the player must have AT LEAST 8999 Damage and AT LEAST 10 Corruption`.

If we were to change the `RequirementType` to `Max`, it would change to `For this card to show up, the player must have at LESS THAN 8999 Damage and LESS THAN 10 Corruption`.

### Effects
This is a list of effects that are applied to the card.

#### Name
An optional name for the effect. This is only used for debug logs and will be auto-generated if not provided. 

#### Type
Describes the type of this effect.

_**Options:**_
- `OneTime`: The effect is executed once, then disabled again
- `Duration`: After activation, the effect is active for a certain amount of time, then disables itself. This type requires a `Trigger` in `Properties`
- `Interval`: After activation, the effect repeats on an interval
- `Trigger`: The effect is activated when a certain event happens

#### ActivationRequirement
What needs to happen for this effect to become active?

_**Default:**_ `None`

_**Options:**_
- `None`: The effect is always active
- `StageStart`: The effect will activate at the start of a stage in Rogs Mode. In Survival, this is the same as `None`
- `StageEnd`: The effect will trigger at the end of a stage in Rogs Mode. Can not trigger in Survival. ***!!! NOT IMPLEMENTED YET !!!***
- `EnemiesKilled`: The effect will activate when a certain amount of enemies was killed.
- `DamageTaken`: The effect will activate after you have taken a certain amount of damage from enemies. The required amount is defined in [ActivationRequirementProperties.TotalDamageTaken](#activationrequirementproperties)

#### ActivationRequirementProperties
Certain [ActivationRequirements](#activationrequirement) have parameters, which are defined here.

_**Options:**_
- `TotalDamageTaken`: For `ActivationRequirement.DamageTake`: Specifies the amount of damage you need to take before the effect activates.
- `EnemiesKilled`: For `ActivationRequirement.EnemiesKilled`: Specifies the number of enemies you need to kill before the effect activates.


#### Trigger
Describes what needs to happen for the effect to be applied.

_**Options:**_
- `OnStageStart`: Effect applied at the start of a stage in Rogs mode or immediately in Survivors mode.
- `OnStageEnd`: Effect applies at the end of a stage in Rogs mode and never in Survivors mode. ***!!! NOT IMPLEMENTED YET !!!***
- `OnKill`: Effect applies when any enemy is killed (that includes Elites and Bosses)
- `OnEliteKill`: Effect applies ONLY when an elite enemy is killed.
- `OnBossKill`: Effect applies ONLY when a boss enemy is killed.
- `OnDash`: Effect applies when you dash
- `OnDeath`: Effect applies when you die. **NOTE:** Attempting to heal yourself with this trigger will NOT revive you
- `OnTakeDamage`: Effect applies when you take damage from any source.

#### Action
Describes what this effect will do

_**Options:**_
- `AddGold`: Gives you the amount of gold specified in `Properties.Amount`
- `AddBanishes`: Gives you the amount of banishes specified in `Properties.Amount`
- `AddRerolls`: Gives you the amount of rerolls specified in `Properties.Amount`
- `AddRarityRerolls`: Gives you the amount of rarity rerolls specified in `Properties.Amount`
- `HealAmount`: Heals you by the exact amount specified in `Properties.Amount`
- `HealPercentage`: Heals you by the percentage of your max health specified in `Properties.Percentage`

#### Properties
Contains properties that are required for certain other aspects of an effect.

```json
"Properties": {
  "Amount": 12000,
  "Percentage": 95.5,
  "Duration": 30,
  "Interval": 30,
}
```

_**Options:**_
- `Amount`: An exact amount of something.
  - **Type:** `Number` [1, 2.0, 1.222, etc]
  - **Applies to actions:** `AddGold`, `AddBanishes`, `AddRerolls`, `AddRarityRerolls`, `HealAmount`


- `Percentage`: An percentage of something.
  - **Type:** `Number` [1, 2.0, 1.222, etc]
  - **Applies to actions:** `HealPercentage` 


- `Duration`: A duration in seconds.
  - **Type:** `Number` [1, 2.0, 1.222, etc]
  - **Applies to types:** `Duration`


- `Interval`: An interval in seconds.
  - **Type:** `Number` [1, 2.0, 1.222, etc]
  - **Applies to types:** `Interval`

    
#### CharacterSpriteConfiguration
**NOTE:** This functionality has been deprecated and now does nothing! 

### Limitations
- Effects do not scale with level

### Examples
Examples for effect cards and a description of what they do can be found on [Github](https://github.com/shaacker/EasyCards/blob/master/Documentation/EffectCardExamples.md).
