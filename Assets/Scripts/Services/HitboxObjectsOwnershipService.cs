using System.Collections.Generic;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Service
{
    public class HitboxObjectsOwnershipService : IOwnershipService
    {
        public List<GameObject> Objects { get; private set; } = new List<GameObject>();

        public void Add(GameObject GO)
        {
            Objects.Add(GO);
        }

        public void Remove(GameObject GO)
        {
            Objects.Remove(GO);
        }

        public bool Contains(GameObject GO)
        {
            if (Objects.Contains(GO))
            {
                return true;
            }
            return false;
        }
    }
}