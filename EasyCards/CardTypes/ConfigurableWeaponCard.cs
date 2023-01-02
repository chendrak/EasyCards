namespace EasyCards.CardTypes;

using System;
using Models.Templates;
using RogueGenesia.Actors.Survival;
using RogueGenesia.Data;

public class ConfigurableWeaponCard : Weapon
{
    private WeaponEffect? WeaponEffect;

    public ConfigurableWeaponCard(IntPtr ptr) : base(ptr) { }

    public void SetWeaponEffect(WeaponEffect? effect) => this.WeaponEffect = effect;

    public override void OnAttack(PlayerEntity Owner, AttackInformation attackInformation) => this.WeaponEffect?.OnAttack(Owner, attackInformation);

    public override void OnTakeDamage(PlayerEntity Owner, IEntity damageOwner, ref float modifierDamageValue,
        float damageValue) =>
        this.WeaponEffect?.OnTakeDamage(Owner, damageOwner, ref modifierDamageValue, damageValue);

    public override void OnRemove() => this.WeaponEffect?.OnRemove();

    public override void LevelUp() => this.WeaponEffect?.LevelUp();
}
