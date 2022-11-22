namespace EasyCards.Models.Templates;

using Common.Helpers;
using ModGenesia;
using RogueGenesia.Actors.Survival;
using RogueGenesia.Data;
using UnityEngine;

public class ConfigurableEffect
{
    public string Name;
    public string AssetBasePath = "";
    public EffectType Type;
    public EffectActivationRequirement ActivationRequirement = EffectActivationRequirement.None;
    public EffectTrigger Trigger;
    public EffectAction Action;
    public EffectProperties Properties;

    private float activationTime;
    public bool Enabled { get; private set; }

    public void Enable(float time)
    {
        Debug.Log($"Enabling effect: {this}");
        this.activationTime = time;
        this.Enabled = true;

        if (this.Type == EffectType.OneTime)
        {
            this.Apply();
            this.Disable();
        }
    }

    public void Disable()
    {
        Debug.Log($"Disabling effect: {this}");
        this.Enabled = false;
    }

    private PlayerData PlayerData => GameData.PlayerDatabase[0];

    public void Apply()
    {
        if (!this.Enabled) return;

        Debug.Log($"Applying effect: {this}");

        switch (this.Action)
        {
            case EffectAction.AddGold:
                var amountToAdd = this.Properties.Amount ?? 0;
                Debug.Log($"AddGold: {amountToAdd}");
                this.PlayerData.Gold += amountToAdd;
                break;
            case EffectAction.AddBanishes:
                Debug.Log($"AddBanishes: {this.Properties.Amount}");
                this.PlayerData.BanishLeft += (int?)this.Properties.Amount ?? 0;
                break;
            case EffectAction.AddRerolls:
                Debug.Log($"AddRerolls: {this.Properties.Amount}");
                this.PlayerData.RerollLeft += (int?)this.Properties.Amount ?? 0;
                break;
            case EffectAction.AddRarityRerolls:
                Debug.Log($"AddRarityRerolls: {this.Properties.Amount}");
                this.PlayerData.RarityRerollLeft += (int?)this.Properties.Amount ?? 0;
                break;
            case EffectAction.HealAmount:
                Debug.Log($"HealAmount: {this.Properties.Amount}");
                this.PlayerData.HealPlayer(this.Properties.Amount ?? 0);
                break;
            case EffectAction.HealPercentage:
                Debug.Log($"HealPercentage: {this.Properties.Amount}");
                if (this.Properties.Percentage is {} percentage)
                {
                    var playerMaxHealth = this.PlayerData._playerStats.MaxHealth.Value;
                    var healAmount = playerMaxHealth * (percentage / 100);
                    this.PlayerData.HealPlayer(healAmount);
                }
                break;
            case EffectAction.ChangeCharacterSprites:
                Debug.Log($"ChangeCharacterSprites");
                if (this.Properties.CharacterSpriteConfiguration is {} config)
                {
                    Debug.Log($"Got config: {LoggingHelper.StructToString(config)}");
                    Texture2D? idleTexture = null;
                    var idleFrameCount = Vector2.zero;

                    Texture2D? runTexture = null;
                    var runFrameCount = Vector2.zero;

                    Texture2D? victoryTexture = null;
                    var victoryFrameCount = Vector2.zero;

                    Texture2D? deathTexture = null;
                    var deathFrameCount = Vector2.zero;

                    if (config.Idle is { } idleCfg)
                    {
                        var texturePath = Path.Combine(this.AssetBasePath, idleCfg.TexturePath);
                        idleTexture = SpriteHelper.LoadPNGIntoTexture(texturePath);
                        if (idleTexture == null)
                        {
                            Debug.LogWarning($"Unable to load idle texture from: {texturePath}");
                        }
                        idleFrameCount = new Vector2(idleCfg.FramesPerRow, idleCfg.Rows);
                    }

                    if (config.Run is { } runCfg)
                    {
                        var texturePath = Path.Combine(this.AssetBasePath, runCfg.TexturePath);
                        runTexture = SpriteHelper.LoadPNGIntoTexture(texturePath);
                        if (runTexture == null)
                        {
                            Debug.LogWarning($"Unable to load run texture from: {texturePath}");
                        }
                        runFrameCount = new Vector2(runCfg.FramesPerRow, runCfg.Rows);
                    }

                    if (config.Victory is { } victoryCfg)
                    {
                        var texturePath = Path.Combine(this.AssetBasePath, victoryCfg.TexturePath);
                        victoryTexture = SpriteHelper.LoadPNGIntoTexture(texturePath);
                        if (victoryTexture == null)
                        {
                            Debug.LogWarning($"Unable to load victory texture from: {texturePath}");
                        }
                        victoryFrameCount = new Vector2(victoryCfg.FramesPerRow, victoryCfg.Rows);
                    }

                    if (config.Death is { } deathCfg)
                    {
                        var texturePath = Path.Combine(this.AssetBasePath, deathCfg.TexturePath);
                        deathTexture = SpriteHelper.LoadPNGIntoTexture(texturePath);
                        if (deathTexture == null)
                        {
                            Debug.LogWarning($"Unable to load death texture from: {texturePath}");
                        }
                        deathFrameCount = new Vector2(deathCfg.FramesPerRow, deathCfg.Rows);
                    }

                    ModGenesia.ReplaceRogSkin(
                        avatarID: 0,
                        idleAnimation: idleTexture,
                        idleFrameCount: idleFrameCount,
                        runningAnimation: runTexture,
                        runningFrameCount: runFrameCount,
                        victoryAnimation: victoryTexture,
                        victoryFrameCount: victoryFrameCount,
                        deathAnimation: deathTexture,
                        deathFrameCount: deathFrameCount
                    );
                }
                break;
            default:
                Debug.LogWarning($"Unsupported action {this.Action}");
                break;
        }
    }

    public void OnUpdate(PlayerEntity owner, float currentTime)
    {
        if (!this.Enabled) return;

        switch (this.Type)
        {
            case EffectType.Duration:
            {
                // Check for expiration
                var duration = this.Properties.Duration ?? 0f;
                if (currentTime > this.activationTime + duration)
                {
                    this.Disable();
                }

                break;
            }
            case EffectType.Interval:
                // Check for next trigger
                if (this.Properties.Interval is {} interval)
                {
                    if (currentTime >= this.activationTime + interval)
                    {
                        this.Apply();
                        this.activationTime = Time.time;
                    }
                }
                break;
        }
    }

    public void Reset()
    {
        this.activationTime = -1;
        this.Enabled = false;
    }

    // public override string ToString() => $"{nameof(this.Name)}: {this.Name}, {nameof(this.Type)}: {this.Type}, {nameof(this.ActivationRequirement)}: {this.ActivationRequirement}, {nameof(this.Trigger)}: {this.Trigger}, {nameof(this.Action)}: {this.Action}, {nameof(this.Properties)}: {this.Properties}, {nameof(this.activationTime)}: {this.activationTime}, {nameof(this.Enabled)}: {this.Enabled}";
    public override string ToString() => this.Name;
}

public class EffectProperties
{
    public float? Amount;
    public float? Percentage;
    public float? Duration;
    public float? Interval;

    public CharacterSpriteProperties? CharacterSpriteConfiguration;

    public override string ToString() => $"{nameof(this.Amount)}: {this.Amount}, {nameof(this.Percentage)}: {this.Percentage}, {nameof(this.Duration)}: {this.Duration}, {nameof(this.Interval)}: {this.Interval}";
}

public struct SpriteConfiguration
{
    public string TexturePath;
    public int FramesPerRow;
    public int Rows;
}

public class CharacterSpriteProperties
{
    public SpriteConfiguration? Idle;
    public SpriteConfiguration? Run;
    public SpriteConfiguration? Victory;
    public SpriteConfiguration? Death;
}

public enum EffectType
{
    OneTime,
    Duration,
    Interval,
    Trigger
}

public enum EffectActivationRequirement
{
    None,
    StageStart,
    StageEnd,
    EnemiesKilled,
}

public enum EffectTrigger
{
    OnStageStart,
    OnStageEnd,
    OnKill,
    OnEliteKill,
    OnBossKill,
    OnDash,
    OnDeath,
}

public enum EffectAction
{
    AddGold,
    AddBanishes,
    AddRerolls,
    AddRarityRerolls,
    HealAmount,
    HealPercentage,
    ChangeCharacterSprites
}
