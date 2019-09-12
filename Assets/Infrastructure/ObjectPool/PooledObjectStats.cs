using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.ObjectPool
{
    [CreateAssetMenu(menuName = "PooledObject")]
    public class PooledObjectStats : ScriptableObject
    {
        public Transform prefab;
        public int number;
        public ExpandBy expandBy = ExpandBy.Doubling;

        [HideInInspector] public Transform parent;
    }

    public enum ExpandBy
    {
        OneAtATime,
        Doubling
    }
}