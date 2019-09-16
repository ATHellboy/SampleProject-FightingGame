// Copyright (c) ATHellboy (Alireza Tarahomi) Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using UnityEngine;

namespace Infrastructure.ObjectPooling
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