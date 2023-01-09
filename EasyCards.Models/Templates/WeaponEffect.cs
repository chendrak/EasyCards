namespace EasyCards.Models.Templates;

using RogueGenesia.Actors.Survival;
using RogueGenesia.Data;

public abstract class WeaponEffect
{
    public int Level { get; private set; } = 1;
    public virtual void OnLoad() { }
    public virtual void OnUnload() { }

    public virtual void OnAttack(Weapon weapon, PlayerEntity owner, AttackInformation attackInformation) { }

    public virtual void OnTakeDamage(PlayerEntity owner, IEntity damageOwner, ref float modifierDamageValue, float damageValue) {}

    public virtual void Fractal() { }
    public virtual void OnRemove() { }

    public virtual void LevelUp() => this.Level++;
}
