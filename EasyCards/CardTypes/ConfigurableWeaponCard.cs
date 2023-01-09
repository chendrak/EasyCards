namespace EasyCards.CardTypes;

using System;
using Common.Logging;
using Models.Templates;
using RogueGenesia.Actors.Survival;
using RogueGenesia.Data;
using Services;

public class ConfigurableWeaponCard : Weapon
{
    private string WeaponEffectName;
    private WeaponEffect? WeaponEffect;

    public ConfigurableWeaponCard(IntPtr ptr) : base(ptr)
    {
        WeaponEffectRegistry.OnWeaponEffectRegisteredEvent += OnWeaponEffectRegistered;
        WeaponEffectRegistry.OnWeaponEffectUnregisteredEvent += OnWeaponEffectUnregistered;
        this.WeaponEffectName = WeaponToEffectMapper.GetEffectNameForWeapon(this.WeaponName);
    }

    public override void Init(PlayerEntity Owner)
    {
        SetWeaponEffect(WeaponToEffectMapper.GetEffectForWeapon(this.GetType().Name));
    }

    private void OnWeaponEffectUnregistered(Type type)
    {
        if (type.Name == this.WeaponEffectName)
        {
            this.WeaponEffect = null;
        }
    }

    private void OnWeaponEffectRegistered(Type type)
    {
        if (type.Name == this.WeaponEffectName)
        {
            SetWeaponEffect(WeaponEffectRegistry.GetEffectForType(type));
        }
    }

    public virtual void SetWeaponEffect(WeaponEffect? effect)
    {
        Log.Info($"Setting Weapon Effect: {effect}");
        this.WeaponEffect = effect;
        while (this.WeaponEffect?.Level < this.Level)
        {
            this.WeaponEffect?.LevelUp();
        }
    }

    public override void OnAttack(PlayerEntity Owner, AttackInformation attackInformation) => this.WeaponEffect?.OnAttack(this, Owner, attackInformation);

    public override void OnTakeDamage(PlayerEntity Owner, IEntity damageOwner, ref float modifierDamageValue,
        float damageValue) =>
        this.WeaponEffect?.OnTakeDamage(Owner, damageOwner, ref modifierDamageValue, damageValue);

    public override void OnRemove()
    {
        this.WeaponEffect?.OnRemove();
        this.WeaponEffect = null;

        WeaponEffectRegistry.OnWeaponEffectRegisteredEvent -= OnWeaponEffectRegistered;
        WeaponEffectRegistry.OnWeaponEffectUnregisteredEvent -= OnWeaponEffectUnregistered;
    }

    public override void LevelUp()
    {
        base.LevelUp();
        this.WeaponEffect?.LevelUp();
    }
}
