namespace EasyCards.Weapons
{
    using System;
    using RogueGenesia.Actors.Survival;
    using RogueGenesia.Data;

    public class XPRing : Weapon
    {
        public XPRing(IntPtr ptr)
            : base(ptr)
        {
            this.ComboCount = 1;
            this.DelayBetweenCombo = 0.5f;
        }


        public override void OnAttack(PlayerEntity Owner, AttackInformation _attackInformation) => Owner.AddSoulExp(Math.Pow(2.0, GameData.PlayerDatabase[0]._soulLevel._soulLevel / 5.0));

        public override void LevelUp() => this.DelayBetweenCombo -= 1f / 400f;
    }
}
