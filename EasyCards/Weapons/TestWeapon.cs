namespace EasyCards.Weapons;

using System;
using CardTypes;
using Helpers;
using Models.Templates;
using UnityEngine;

public class TestWeapon : ConfigurableWeaponCard
{
    public TestWeapon(IntPtr ptr) : base(ptr)
    {
        this.Damage = 3f;
        this.Projectile = 3;
        this.Piercing = 999;
        this._weaponProjectile = Resources.Load<GameObject>(WeaponProjectiles.THORN_SPIKE);
    }

    public override void SetWeaponEffect(WeaponEffect? effect)
    {
        base.SetWeaponEffect(effect);

    }
}
