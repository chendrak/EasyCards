namespace EasyCards.WeaponEffects.Projectile;

using Il2CppSystem.Collections.Generic;
using Models.Templates;
using RogueGenesia.Actors.Survival;
using RogueGenesia.Data;

public class ProjectileWeaponEffect : WeaponEffect
{
    public WeaponProjectileConfig weaponProjectileConfig { get; private set; }

    public void SetWeaponProjectileConfig(WeaponProjectileConfig weaponProjectileConfig) =>
        this.weaponProjectileConfig = weaponProjectileConfig;

    public override void OnAttack(Weapon weapon, PlayerEntity owner, AttackInformation attackInformation)
    {
        var targets = GetTargetsForType(weaponProjectileConfig.ProjectileTargetingType, weapon, owner);
    }

    private List<DefaultProjectilAI> GetTargetsForType(ProjectileTargetingType projectileTargetingType, Weapon weapon,
        PlayerEntity owner)
    {
        switch(projectileTargetingType)
        {
            case ProjectileTargetingType.Manual:
                return weapon.GetDirectionManualTargeting(weaponProjectileConfig.ctor, owner, 0, 10f);
            case ProjectileTargetingType.Closest:
                return weapon.GetClosestTargeting(weaponProjectileConfig.ctor, owner);
            case ProjectileTargetingType.UniformAround:
                return weapon.GetUniformArroundTargeting(weaponProjectileConfig.ctor, owner);
            case ProjectileTargetingType.RandomAround:
                return weapon.GetRandomArroundTargeting(weaponProjectileConfig.ctor, owner);
            case ProjectileTargetingType.RandomAroundExceptClosest:
                return weapon.GetRandomArroundExceptOneTargeting(weaponProjectileConfig.ctor, owner);
            case ProjectileTargetingType.Healthiest:
                return weapon.GetHealthyTargeting(weaponProjectileConfig.ctor, owner);
            case ProjectileTargetingType.LeastHealthy:
                return weapon.GetLowestHealthyTargeting(weaponProjectileConfig.ctor, owner);
            default:
                throw new ArgumentOutOfRangeException(nameof(projectileTargetingType), projectileTargetingType, null);
        }
    }
}
