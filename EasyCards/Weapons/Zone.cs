namespace EasyCards.Weapons
{
    using UnityEngine;

    internal struct Zone
    {
        public GameObject go { get; init; }

        public float size { get; set; }

        public float currentSize { get; set; }

        public float coolDown { get; set; }

        public float maxCoolDown { get; set; }

        public float damage { get; set; }
    }
}
