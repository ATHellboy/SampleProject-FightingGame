using System.Collections.Generic;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Service
{
    public interface IOwnershipService
    {
        List<GameObject> Objects { get; }

        void Add(GameObject GO);
        void Remove(GameObject GO);
        bool Contains(GameObject GO);
    }
}