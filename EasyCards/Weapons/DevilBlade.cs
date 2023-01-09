namespace EasyCards.Weapons
{
    using System;
    using System.Collections.Generic;
    using Helpers;
    using Il2CppInterop.Runtime.Injection;
    using RogueGenesia.Actors.Survival;
    using RogueGenesia.Data;
    using RogueGenesia.GameManager;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class DevilBlade : Weapon
    {
        private static Vector3 PROJECTILE_OFFSET = new Vector3(0.0f, 1.4f, 0.0f);
        private static Vector3 ZONE_OFFSET = new Vector3(0.0f, 0.5f, 0.0f);
        private Dictionary<int, Zone> _zones;
        private List<int> _monstersToDelete;
        private List<Zone> _zonesToDestroy;
        private GameObject _zoneGo;

        public DevilBlade(IntPtr ptr) : base(ptr)
        {
            this.ComboCount = 1;
            this.Damage = 0.1f;
            this.Projectile = 1;
            this.WeaponRange = 3f;
            this.SlashPerEnemy = 1;
            this.DelayBetweenCombo = 0.5f;
            this._zones = new Dictionary<int, Zone>(32);
            this._zonesToDestroy = new List<Zone>(32);
            this._monstersToDelete = new List<int>(32);
        }

        public DevilBlade() : base(ClassInjector.DerivedConstructorPointer<DevilBlade>())
        {
            ClassInjector.DerivedConstructorBody(this);
        }

        public override void OnUpdate(PlayerEntity Owner)
        {
            float deltaTime = Time.deltaTime;
            int numEnemies = EnemyManager.GetEnemiesCount;
            DamageMultiplierData multiplierNoCrit = Owner.GetDamageMultiplierNoCrit;
            foreach (KeyValuePair<int, Zone> entry in this._zones)
            {
                GameObject gameObject = GameObject.Find(entry.Key.ToString());
                Zone zone = entry.Value;
                if (gameObject == null)
                {
                    this._zonesToDestroy.Add(zone);
                    this._monstersToDelete.Add(entry.Key);
                }
                else
                {
                    zone.go.transform.position = gameObject.transform.position + DevilBlade.ZONE_OFFSET;
                    if (zone.currentSize < zone.size)
                    {
                        zone.currentSize = Math.Min(zone.currentSize + deltaTime * 1.5f, zone.size);
                        float doubleZoneSize = zone.currentSize * 2f;
                        zone.go.transform.localScale = new Vector3(doubleZoneSize, doubleZoneSize, doubleZoneSize);
                    }

                    zone.coolDown -= deltaTime;
                    if (zone.coolDown <= 0.0)
                    {
                        zone.coolDown = zone.maxCoolDown;
                        for (int index = 0; index < numEnemies; ++index)
                        {
                            Monster enemy = EnemyManager.GetEnemy(index);
                            if (enemy != null && enemy.GetLinkedGameObject() != null)
                            {
                                Vector3 enemyPosition = enemy.GetLinkedGameObject().transform.position;
                                float num1 = enemy.Scale * 0.5f + zone.currentSize;
                                float num2 = num1 * num1;
                                Vector3 zonePosition = zone.go.transform.position;
                                if ((enemyPosition - zonePosition).sqrMagnitude <= num2)
                                {
                                    DamageInformation damageInfo = new DamageInformation();
                                    var ownerEntity = Owner.Cast<IEntity>();

                                    damageInfo.DamageOwner = ownerEntity;
                                    damageInfo.DamageValue = zone.damage * multiplierNoCrit.DamageMultiplier;
                                    damageInfo.DamageSource = WeaponType.Other;
                                    enemy.TakeDamage(damageInfo);
                                    var enemyEntity = enemy.Cast<IEntity>();
                                    this.ApplyModifier(Owner, enemyEntity, damageInfo.DamageValue);
                                }
                            }
                        }
                    }

                    this._zones[entry.Key] = zone;
                }
            }

            int numZonesToDestroy = this._zonesToDestroy.Count;
            int numMonstersToDelete = this._monstersToDelete.Count;
            for (int index = Math.Max(numZonesToDestroy, numMonstersToDelete) - 1; index > -1; --index)
            {
                if (index < numMonstersToDelete)
                {
                    this._zones.Remove(this._monstersToDelete[index]);
                    this._monstersToDelete.RemoveAt(index);
                }

                if (index < numZonesToDestroy)
                {
                    var decayPerMilli = 0.2f;

                    Zone zone = this._zonesToDestroy[index];
                    if (zone.currentSize > 0.0)
                    {
                        zone.currentSize = Math.Max(zone.currentSize - deltaTime * 10f, 0.0f);
                        // zone.currentSize = Math.Max(zone.currentSize - deltaTime * decayPerMilli, 0.0f);
                        var doubleZoneSize = zone.currentSize * 2f;
                        zone.go.transform.localScale = new Vector3(doubleZoneSize, doubleZoneSize, doubleZoneSize);
                        this._zonesToDestroy[index] = zone;
                    }
                    else
                    {
                        Object.Destroy(zone.go);
                        this._zonesToDestroy.RemoveAt(index);
                    }
                }
            }
        }

        public override void OnAttack(PlayerEntity Owner, AttackInformation attackInformation)
        {
            if (this._weaponProjectile == null)
                this._weaponProjectile = Resources.Load<GameObject>(WeaponProjectiles.KATANA_SLASH);
            if (this._zoneGo == null)
                this._zoneGo = Resources.Load<GameObject>(WeaponProjectiles.EVIL_RING);
            Vector3 playerPos = Owner.transform.position;
            float num1 = this.WeaponRange * Owner.GetPlayerStats.AreaSize.Value;
            float num2 = num1 * num1;
            int getEnemiesCount = EnemyManager.GetEnemiesCount;
            var list = new List<Monster>(getEnemiesCount);
            for (int index = 0; index < getEnemiesCount; ++index)
            {
                Monster enemy = EnemyManager.GetEnemy(index);
                if ((enemy.GetLinkedGameObject().transform.position - playerPos).sqrMagnitude <= num2)
                    list.Add(enemy);
            }

            if (list.Count == 0)
            {
                this.AttackCoolDownLeft = this.DelayBetweenCombo * 0.1f;
            }
            else
            {
                int numProjectiles = (int)Math.Max(1f, this.Projectile + Owner.GetPlayerStats.AdditionalProjectile.Value);
                float knockBackValue = 0.1f * Owner.GetPlayerStats.KnockBack.Value;
                for (int index1 = Math.Min(list.Count, numProjectiles) - 1; index1 > -1; --index1)
                {
                    int randomMonsterIdx = UnityEngine.Random.Range(0, list.Count);
                    Monster monster = list[randomMonsterIdx];
                    GameObject linkedObject = monster.GetLinkedGameObject();
                    Vector3 normalized = (linkedObject.transform.position - playerPos).normalized;

                    var proj = new KatanaVisual();
                    proj.PoolPrefabAssociated = _weaponProjectile;
                    ProjectileManager.SpawnProjectile(
                        proj.Cast<DefaultProjectilAI>(),
                        linkedObject.transform.position + DevilBlade.PROJECTILE_OFFSET,
                        Quaternion.Euler(0.0f, UnityEngine.Random.value * 360f, 0.0f)
                    );
                    proj.AuraSize = 1f;
                    proj.LifeTime = 0.25f;
                    proj.Awake();

                    DamageMultiplierData damageMultiplier = Owner.GetDamageMultiplier;
                    DamageInformation damageInfo = new DamageInformation();
                    damageInfo.DamageOwner = Owner.Cast<IEntity>();
                    damageInfo.DamageValue = this.Damage * damageMultiplier.DamageMultiplier;
                    damageInfo.Critical = damageMultiplier.Critical;
                    damageInfo.KnockBackValue = knockBackValue;
                    damageInfo.Direction = new Vector2(normalized.x, normalized.z);
                    damageInfo.DamageSource = WeaponType.Other;
                    var healthBefore = monster.Health.Value;
                    monster.TakeDamage(damageInfo);
                    var monsterTookDamage = monster.Health.Value < healthBefore;
                    this.ApplyModifier(Owner, monster.Cast<IEntity>(), damageInfo.DamageValue);

                    if (monsterTookDamage)
                    {
                        int instanceId = monster.GetLinkedGameObject().GetInstanceID();
                        if (this._zones.TryGetValue(instanceId, out var zone))
                        {
                            zone.size *= 1.25f;
                            zone.damage *= 1.25f;
                            zone.maxCoolDown *= 0.75f;
                            this._zones[instanceId] = zone;
                        }
                        else if (this._zones.Count < numProjectiles)
                        {
                            monster.GetLinkedGameObject().name = instanceId.ToString();
                            GameObject gameObject = Object.Instantiate(this._zoneGo,
                                linkedObject.transform.position + DevilBlade.ZONE_OFFSET, Quaternion.identity);
                            this._zones.Add(instanceId, new Zone { go = gameObject, size = 5.0f, maxCoolDown = 0.25f, damage = 0.5f });
                        }
                    }

                    list.RemoveAt(randomMonsterIdx);
                }
            }
        }

        public override void LevelUp()
        {
            this.WeaponRange += 0.5f;
            this.Damage += 1f / 400f;
            this.DelayBetweenCombo -= 0.025f;
            if (this._level % 4 != 0)
                return;
            ++this.Projectile;
        }
    }
}
