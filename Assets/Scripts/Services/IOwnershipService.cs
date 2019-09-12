using AlirezaTarahomi.FightingGame.Character;
using System.Collections.Generic;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Service
{
    public interface IOwnershipService
    {
        List<GameObject> HitboxObjects { get; }

        void Add(GameObject GO);
        void Remove(GameObject GO);
        bool Contains(GameObject GO);
    }
}