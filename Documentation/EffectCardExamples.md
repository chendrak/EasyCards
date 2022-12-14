### 10k gold once
```json
  {
    "Name": "OneTime100K",
    "Type": "OneTime",
    "ActivationRequirement": "None",
    "Action": "AddGold",
    "Properties": {
      "Amount": 10000,
    }
  }
```

### Gold on kill

This effect will:
- Activate at the start of every stage in Rog Mode. It will only activate once in Survivors, as there are no stages.
- Be active for a `Duration` of 30s
- While it is active, you will receive 1 gold for every enemy killed (normal, elite or boss)

```json
  {
    "Name": "GoldOnKill",
    "Type": "Duration",
    "ActivationRequirement": "StageStart",
    "Trigger": "OnKill",
    "Action": "AddGold",
    "Properties": {
      "Amount": 1,
      "Duration": 30,
    }
  }
```

### Gold on elite kill
Same as above, but will only trigger on `Elite` enemies and grant 1k gold instead.

```json
  {
    "Name": "GoldOnEliteKill",
    "Type": "Duration",
    "ActivationRequirement": "StageStart",
    "Trigger": "OnEliteKill",
    "Action": "AddGold",
    "Properties": {
      "Amount": 1000,
      "Duration": 30,
    }
  }
```

### Gold on boss kill
Same as above, but will only trigger on `Boss` enemies and grant 50k gold instead.
```json
  {
    "Name": "GoldOnBossKill",
    "Type": "Duration",
    "ActivationRequirement": "StageStart",
    "Trigger": "OnBossKill",
    "Action": "AddGold",
    "Properties": {
      "Amount": 50000,
      "Duration": 30,
    }
  }
```

### Periodic heals
This effect will:
- Activate at the start of each stage in Rog Mode.
- Heal you for 13.37 HP every 2 seconds
- Never expire

```json
  {
    "Name": "HealOnInterval",
    "Type": "Interval",
    "ActivationRequirement": "StageStart",
    "Action": "HealAmount",
    "Properties": {
      "Amount": 13.37,
      "Interval": 2,
    }
  }
```

### Be someone else
This effect will:
- Activate as soon as you choose the card
- Replace the idle animation with a pikachu idle animation
- Replace the run animation with a pikachu run animation

```json
  {
    "Name": "BePikachu",
    "Type": "OneTime",
    "ActivationRequirement": "None",
    "Action": "ChangeCharacterSprites",
    "Properties": {
      "CharacterSpriteConfiguration": {
        "Idle": {
          "TexturePath": "PlushPaws/pikachu_idle.png",
          "FramesPerRow": 4,
          "Rows": 1,
        },
        "Run": {
          "TexturePath": "PlushPaws/pikachu_run.png",
          "FramesPerRow": 4,
          "Rows": 1,
        },
      }
    }
  }
```
