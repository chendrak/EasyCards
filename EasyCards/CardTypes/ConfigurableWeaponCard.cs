namespace EasyCards.CardTypes;

using System;
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

        }
    }

    public void SetWeaponEffect(WeaponEffect? effect) => this.WeaponEffect = effect;

    public override void OnAttack(PlayerEntity Owner, AttackInformation attackInformation) => this.WeaponEffect?.OnAttack(Owner, attackInformation);

    public override void OnTakeDamage(PlayerEntity Owner, IEntity damageOwner, ref float modifierDamageValue,
        float damageValue) =>
        this.WeaponEffect?.OnTakeDamage(Owner, damageOwner, ref modifierDamageValue, damageValue);

    public override void OnRemove()
    {
        this.WeaponEffect?.OnRemove();
        this.WeaponEffect = null;
    }

    public override void LevelUp() => this.WeaponEffect?.LevelUp();
}
