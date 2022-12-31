namespace EasyCards.CardTypes;

using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Logging;
using Common.Events;
using Effects;
using Il2CppSystem.Reflection;
using Models.Templates;
using RogueGenesia.Actors.Survival;
using RogueGenesia.Data;
using UnityEngine;

public class ConfigurableWeaponCard : Weapon
{
    public ConfigurableWeaponCard(IntPtr ptr) : base(ptr) { }

    public override float getDamage() => base.getDamage();

    public override float getAttackDelay() => base.getAttackDelay();

    public override int getProjectiles() => base.getProjectiles();

    public override int getPiercing() => base.getPiercing();

    public override float getArea() => base.getArea();

    public override int getSlashPerEnemy() => base.getSlashPerEnemy();

    public override int getComboCount() => base.getComboCount();

    public override float getDamagePerStack() => base.getDamagePerStack();

    public override float getLifeTime() => base.getLifeTime();

    public override int getTickCount() => base.getTickCount();

    public override float getProjectileSize() => base.getProjectileSize();

    public override float getCriticalMultiplier() => base.getCriticalMultiplier();

    public override float getProjectileSpeed() => base.getProjectileSpeed();

    public override float getKnockBack() => base.getKnockBack();

    public override int getTarget() => base.getTarget();

    public override void LevelUp() => base.LevelUp();

    public override void OnRemove() => base.OnRemove();

    public override void ApplyProjectileEvent(DefaultProjectilAI projectile, PlayerEntity Owner) => base.ApplyProjectileEvent(projectile, Owner);

    public override void ApplyModifier(PlayerEntity Owner, IEntity entity, float damageValue) => base.ApplyModifier(Owner, entity, damageValue);

    public override void Init(PlayerEntity Owner) => base.Init(Owner);

    public override void OnUpdate(PlayerEntity Owner) => base.OnUpdate(Owner);

    public override void OnFlatStatUpdate(AvatarData Owner, ref PlayerStats actualStats, bool fakeStats = false, bool baseState = false) => base.OnFlatStatUpdate(Owner, ref actualStats, fakeStats, baseState);

    public override void OnMultiplierStatUpdate(AvatarData Owner, ref PlayerStats actualStats, bool fakeStats = false,
        bool baseState = false) =>
        base.OnMultiplierStatUpdate(Owner, ref actualStats, fakeStats, baseState);

    public override void OnPostStatUpdate(AvatarData Owner, ref PlayerStats actualStats, bool fakeStats = false, bool baseState = false) => base.OnPostStatUpdate(Owner, ref actualStats, fakeStats, baseState);

    public override void OnAttack(PlayerEntity Owner, AttackInformation attackInformation) => base.OnAttack(Owner, attackInformation);

    public override void OnOtherWeaponAttack(PlayerEntity Owner, AttackInformation attackInformation) => base.OnOtherWeaponAttack(Owner, attackInformation);

    public override void OnDash(PlayerEntity Owner) => base.OnDash(Owner);

    public override void OnTakeDamage(PlayerEntity Owner, IEntity damageOwner, ref float modifierDamageValue, float damageValue) => base.OnTakeDamage(Owner, damageOwner, ref modifierDamageValue, damageValue);

    public override Il2CppSystem.Collections.Generic.List<DefaultProjectilAI> GetDirectionManualTargeting(ConstructorInfo projectileConstructor, PlayerEntity Owner, float baseAngle,
        float anglePerProjectile, bool centered = false, float angleOffset = 0) =>
        base.GetDirectionManualTargeting(projectileConstructor, Owner, baseAngle, anglePerProjectile, centered, angleOffset);

    public override Il2CppSystem.Collections.Generic.List<DefaultProjectilAI> GetUniformArroundTargeting(ConstructorInfo projectileConstructor, PlayerEntity Owner) => base.GetUniformArroundTargeting(projectileConstructor, Owner);

    public override Il2CppSystem.Collections.Generic.List<DefaultProjectilAI> GetRandomArroundTargeting(ConstructorInfo projectileConstructor, PlayerEntity Owner) => base.GetRandomArroundTargeting(projectileConstructor, Owner);

    public override Il2CppSystem.Collections.Generic.List<DefaultProjectilAI> GetRandomArroundExceptOneTargeting(ConstructorInfo projectileConstructor, PlayerEntity Owner) => base.GetRandomArroundExceptOneTargeting(projectileConstructor, Owner);

    public override Il2CppSystem.Collections.Generic.List<DefaultProjectilAI> GetClosestTargeting(ConstructorInfo projectileConstructor, PlayerEntity Owner, int force = -1) => base.GetClosestTargeting(projectileConstructor, Owner, force);

    public override Il2CppSystem.Collections.Generic.List<DefaultProjectilAI> GetHealthyTargeting(ConstructorInfo projectileConstructor, PlayerEntity Owner) => base.GetHealthyTargeting(projectileConstructor, Owner);

    public override Il2CppSystem.Collections.Generic.List<DefaultProjectilAI> GetLowestHealthyTargeting(ConstructorInfo projectileConstructor, PlayerEntity Owner) => base.GetLowestHealthyTargeting(projectileConstructor, Owner);

    public override string WeaponName { get; }
    public override int GetID { get; }
}
