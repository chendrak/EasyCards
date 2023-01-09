namespace EasyCards.Models.Templates;

using Common.Extensions;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;
using Il2CppSystem.Reflection;
using RogueGenesia.Actors.Survival;
using RogueGenesia.Data;
using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;
using Type = System.Type;

public class WeaponProjectileConfig
{
    public ProjectileTargetingType ProjectileTargetingType { get; private init; }
    public readonly ConstructorInfo ctor;

    public WeaponProjectileConfig(ProjectileAIType aiType, ProjectileTargetingType targetingType)
    {
        this.ctor = ProjectileAiMapper.MapTypeToAIConstructor(aiType);
        this.ProjectileTargetingType = targetingType;
    }

    public DefaultProjectilAI SpawnProjectile(Weapon weapon, PlayerEntity owner, AttackInformation attackInformation)
    {
        var emptyArray = new Il2CppReferenceArray<Object>(0);
        var projectile = this.ctor.Invoke(emptyArray) as DefaultProjectilAI;

        return projectile;
    }
}

public static class ProjectileAiMapper
{
    public static ConstructorInfo? MapTypeToAIConstructor(ProjectileAIType aiType)
    {
        Type type;
        switch (aiType)
        {
            case ProjectileAIType.Default:
                type = typeof(DefaultProjectilAI);
                break;
            case ProjectileAIType.AirGroundMissile:
                type = typeof(AirGroundMissibleAI);
                break;
            case ProjectileAIType.Bouncy:
                type = typeof(BouncyProjectilAI);
                break;
            case ProjectileAIType.ExpandingAOE:
                type = typeof(ExpandingAOEAI);
                break;
            case ProjectileAIType.Gravity:
                type = typeof(GravityProjectilAI);
                break;
            case ProjectileAIType.Guided:
                type = typeof(GuidedProjectilAI);
                break;
            case ProjectileAIType.KatanaVisual:
                type = typeof(KatanaVisual);
                break;
            case ProjectileAIType.Orbital:
                type = typeof(OrbitalProjectileAI);
                break;
            case ProjectileAIType.Pike:
                type = typeof(PikeProjectileAi);
                break;
            case ProjectileAIType.Planetary:
                type = typeof(PlanetaryProjectilAI);
                break;
            case ProjectileAIType.Wisp:
                type = typeof(WispProjectilAI);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(aiType), aiType, null);
        }

        return type.GetEmptyIl2CppCtor();
    }
}

public enum ProjectileAIType
{
    Default,
    AirGroundMissile,
    Bouncy,
    ExpandingAOE,
    Gravity,
    Guided,
    KatanaVisual,
    Orbital,
    Pike,
    Planetary,
    Wisp
}

public enum ProjectileTargetingType
{
    Manual,
    Closest,
    UniformAround,
    RandomAround,
    RandomAroundExceptClosest,
    Healthiest,
    LeastHealthy
}
