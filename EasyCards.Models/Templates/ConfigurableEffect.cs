namespace EasyCards.Models.Templates;

using RogueGenesia.Actors.Survival;
using RogueGenesia.Data;
using UnityEngine;

public class ConfigurableEffect
{
    public string Name;
    public string AssetBasePath = "";
    public EffectType Type;
    public EffectActivationRequirement ActivationRequirement = EffectActivationRequirement.None;
    public EffectActivationRequirementProperties ActivationRequirementProperties;
    public EffectTrigger Trigger;
    public EffectAction Action;
    public EffectProperties Properties;

    private long enemiesKilled;
    private float activationTime;
    private float totalDamageTaken;
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

    private AvatarData PlayerData => GameData.PlayerDatabase[0];

    public void Apply()
    {
        if (!this.Enabled)
            return;

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
                Debug.Log($"HealPercentage: {this.Properties.Percentage}%");
                if (this.Properties.Percentage is { } percentage)
                {
                    var playerMaxHealth = this.PlayerData._playerStats.MaxHealth.Value;
                    var healAmount = playerMaxHealth * (percentage / 100);
                    this.PlayerData.HealPlayer(healAmount);
                }
                break;
            case EffectAction.ChangeCharacterSprites:
                // Functionality removed
                break;
            default:
                Debug.LogWarning($"Unsupported action {this.Action}");
                break;
        }
    }

    public void OnUpdate(PlayerEntity owner, float currentTime)
    {
        if (!this.Enabled)
            return;

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
                if (this.Properties.Interval is { } interval)
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

    public void OnTakeDamage(float damageTaken)
    {
        if (this.Enabled && this.Trigger == EffectTrigger.OnTakeDamage)
        {
            this.Apply();
        }

        this.totalDamageTaken += damageTaken;
        if (this.ActivationRequirement == EffectActivationRequirement.DamageTaken &&
            this.totalDamageTaken >= this.ActivationRequirementProperties.TotalDamageTaken)
        {
            this.Enable(Time.time);
            this.totalDamageTaken = 0f;
        }
    }

    public void Reset()
    {
        this.activationTime = -1;
        this.Enabled = false;
    }

    public override string ToString() => this.Name;

    public void OnKill(Monster monster)
    {
        this.enemiesKilled++;

        if (this.ActivationRequirement == EffectActivationRequirement.EnemiesKilled &&
            this.enemiesKilled >= this.ActivationRequirementProperties.EnemiesKilled)
        {
            this.Enable(Time.time);
            this.enemiesKilled = 0;
        }

        if (!this.Enabled)
            return;

        if (this.Trigger == EffectTrigger.OnBossKill && monster.Boss)
        {
            Debug.Log($"Killed boss, applying effect: {this}");
            this.Apply();
        }
        else if (this.Trigger == EffectTrigger.OnEliteKill && monster.Elite)
        {
            Debug.Log($"Killed Elite, applying effect: {this}");
            this.Apply();
        }
        else if (this.Trigger == EffectTrigger.OnKill)
        {
            this.Apply();
        }
    }
}

public class EffectProperties
{
    public float? Amount;
    public float? Percentage;
    public float? Duration;
    public float? Interval;

    public override string ToString() => $"{nameof(this.Amount)}: {this.Amount}, {nameof(this.Percentage)}: {this.Percentage}, {nameof(this.Duration)}: {this.Duration}, {nameof(this.Interval)}: {this.Interval}";
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
    DamageTaken
}

public struct EffectActivationRequirementProperties
{
    public float? TotalDamageTaken;
    public long? EnemiesKilled;
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
    OnTakeDamage
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
