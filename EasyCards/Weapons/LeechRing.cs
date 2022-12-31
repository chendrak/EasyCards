namespace EasyCards.Weapons
{
    using System;
    using Common.Helpers;
    using Helpers;
    using Il2CppInterop.Runtime.Injection;
    using Il2CppInterop.Runtime.InteropTypes;
    using RogueGenesia.Actors.Survival;
    using RogueGenesia.Data;
    using RogueGenesia.GameManager;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class LeechRing : Weapon
    {
        private GameObject AuraFX;

        public LeechRing(IntPtr ptr) : base(ptr)
        {
            this.ComboCount = 1;
            this.Damage = 0.01f;
            this.DelayBetweenCombo = 0.6f;
            this.WeaponRange = 1f;
        }

        public LeechRing() : base(ClassInjector.DerivedConstructorPointer<LeechRing>())
        {
            ClassInjector.DerivedConstructorBody(this);
        }

        public override void OnAttack(PlayerEntity Owner, AttackInformation attackInformation)
        {
            if (this.AuraFX == null)
            {
                this.AuraFX = Object.Instantiate(Resources.Load<GameObject>(WeaponProjectiles.EVIL_RING),
                    Owner.transform.position + new Vector3(0.0f, 0.5f, 0.0f), Quaternion.identity);
                this.AuraFX.transform.SetParent(Owner.transform);
                Material material = this.AuraFX.GetComponentInChildren<MeshRenderer>().material;
                material.color = new Color(91f / 256f, 27f / 128f, 61f / 128f, 1f);
                material.SetColor("Base_Color", new Color(91f / 256f, 27f / 128f, 61f / 128f, 1f));
                material.SetColor("Color_1", new Color(91f / 256f, 27f / 128f, 61f / 128f, 1f));
                material.SetColor("Emissive", new Color(91f / 256f, 27f / 128f, 61f / 128f, 1f));
                material.SetTexture("_MainTex", (Texture)SpriteHelper.LoadPNGIntoTexture("LeechFXMask.png"));
            }

            float num1 = this.WeaponRange * Owner.GetPlayerStats.AreaSize.Value;
            float num2 = num1 * 2f;
            this.AuraFX.transform.localScale = new Vector3(num2, num2, num2);
            Vector3 position1 = Owner.transform.position;
            DamageMultiplierData multiplierNoCrit = Owner.GetDamageMultiplierNoCrit;
            DamageInformation damageInformation = new DamageInformation();
            damageInformation.DamageOwner = new Il2CppObjectBase(Owner.Pointer).Cast<IEntity>();
            damageInformation.DamageValue = this.Damage * multiplierNoCrit.DamageMultiplier;
            damageInformation.DamageSource = WeaponType.Other;
            GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag("Destructible");
            int enemyCount = EnemyManager.GetEnemiesCount;
            int length = gameObjectsWithTag.Length;
            for (int index = Math.Max(enemyCount, length) - 1; index > -1; --index)
            {
                Vector3 vector3_1;
                if (index < enemyCount)
                {
                    Monster enemy = EnemyManager.GetEnemy(index);
                    Vector3 position2 = enemy.GetLinkedGameObject().transform.position;
                    float num3 = enemy.Scale * 0.5f + num1;
                    float num4 = num3 * num3;
                    Vector3 vector3_2 = position1;
                    vector3_1 = position2 - vector3_2;
                    if (vector3_1.sqrMagnitude <= num4)
                    {
                        float b = Math.Max(Owner.pd._actualHealth, Owner.pd._playerStats.MaxHealth.Value);
                        Owner.pd._actualHealth =
                            Mathf.Min(Owner.pd._actualHealth + damageInformation.DamageValue * 0.25f, b);
                        enemy.TakeDamage(damageInformation);
                        this.ApplyModifier(Owner, new Il2CppObjectBase(enemy.Pointer).Cast<IEntity>(),
                            damageInformation.DamageValue);
                    }
                }

                if (index < length)
                {
                    CrateCollisionDetection component =
                        gameObjectsWithTag[index].GetComponent<CrateCollisionDetection>();
                    if (!((UnityEngine.Object)component == (UnityEngine.Object)null) &&
                        !(component.Pointer == IntPtr.Zero))
                    {
                        float num5 = component.transform.localScale.x * 0.5f + num1;
                        float num6 = num5 * num5;
                        vector3_1 = component.transform.position - position1;
                        if (vector3_1.sqrMagnitude <= num6)
                            gameObjectsWithTag[index].GetComponent<CrateCollisionDetection>().TakeDamage(damageInformation);
                    }
                }
            }
        }


        public override void LevelUp()
        {
            switch (this._level)
            {
                case 7:
                    this.DelayBetweenCombo -= 0.03f;
                    this.Damage += 0.005f;
                    this.WeaponRange += 1.5f;
                    break;
                case > 7:
                    this.WeaponRange += 0.75f;
                    this.Damage += 1f / 400f;
                    break;
                default:
                    this.DelayBetweenCombo -= 0.0045f;
                    this.Damage += 0.005f;
                    this.WeaponRange += 0.5f;
                    break;
            }
        }
    }
}
