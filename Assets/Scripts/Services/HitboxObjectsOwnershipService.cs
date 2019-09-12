using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace AlirezaTarahomi.FightingGame.Service
{
    public class HitboxObjectsOwnershipService : IOwnershipService
    {
        public List<GameObject> HitboxObjects { get; private set; } = new List<GameObject>();

        public void Add(GameObject GO)
        {
            HitboxObjects.Add(GO);
        }

        public void Remove(GameObject GO)
        {
            Observable.FromCoroutine(_ => WaitForFrameToRemove(GO)).Subscribe();
        }

        public bool Contains(GameObject GO)
        {
            if (HitboxObjects.Contains(GO))
            {
                return true;
            }
            return false;
        }

        IEnumerator WaitForFrameToRemove(GameObject GO)
        {
            yield return null;
            HitboxObjects.Remove(GO);
        }
    }
}