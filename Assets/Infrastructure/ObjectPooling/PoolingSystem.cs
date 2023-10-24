// Copyright (c) ATHellboy (Alireza Tarahomi) Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using Infrastructure.Factory;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Infrastructure.ObjectPooling
{
    public class PoolingSystem : MonoBehaviour
    {
        [SerializeField] private PooledObjectStats[] _pooledObjectsStats;

        private const int YPOS = 9999;

        private IResourceFactory _resourceFactory;
        private Dictionary<PooledObjectStats, Queue<Transform>> _poolDictionary = new();
        private Dictionary<PooledObjectStats, List<Transform>> _despawnDictionary = new();

        [Inject]
        public void Construct(IResourceFactory resourceFactory)
        {
            _resourceFactory = resourceFactory;
        }

        void Awake()
        {
            CreateAll();
        }

        private Transform Create(Queue<Transform> queue, PooledObjectStats stats)
        {
            Transform pooledObject = _resourceFactory.Instantiate(stats.prefab, new Vector3(0, YPOS, 0), stats.parent);
            IPooledObject pooledObjectComponent = pooledObject.GetComponent<IPooledObject>();
            if (pooledObjectComponent != null)
                pooledObjectComponent.PooledObjectStats = stats;
            pooledObject.gameObject.SetActive(false);
            return pooledObject;
        }

        private void CreateAll()
        {
            for (int i = 0; i < _pooledObjectsStats.Length; i++)
            {
                PooledObjectStats stats = _pooledObjectsStats[i];
                Queue<Transform> queue = new();
                GameObject parent = new(stats.name + "s");
                parent.transform.SetParent(transform);
                stats.parent = parent.transform;

                for (int j = 0; j < _pooledObjectsStats[i].number; j++)
                {
                    Transform pooledObject = Create(queue, stats);
                    queue.Enqueue(pooledObject);
                }
                _poolDictionary.Add(_pooledObjectsStats[i], queue);
            }
        }

        private Transform Spawn(PooledObjectStats stats, Transform parent)
        {
            Queue<Transform> queue = _poolDictionary[stats];

            if (!_despawnDictionary.ContainsKey(stats))
            {
                List<Transform> tempList = new();
                _despawnDictionary.Add(stats, tempList);
            }
            List<Transform> list = _despawnDictionary[stats];

            if (queue.Count == 0)
            {
                queue.Enqueue(Create(queue, stats));

                if (stats.expandBy == ExpandBy.Doubling)
                {
                    for (int j = 0; j < stats.number - 1; j++)
                    {
                        queue.Enqueue(Create(queue, stats));
                    }
                }
            }

            Transform spawnedObject = queue.Dequeue();
            list.Add(spawnedObject);

            spawnedObject.gameObject.SetActive(true);
            if (parent != null)
            {
                spawnedObject.SetParent(parent);
            }

            return spawnedObject;
        }

        public Transform Spawn(PooledObjectStats stats, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            Transform spawnedObject = Spawn(stats, parent);

            spawnedObject.position = position;
            spawnedObject.rotation = rotation;

            return spawnedObject;
        }

        public void Despawn(PooledObjectStats stats, Transform despawnedObject)
        {
            Queue<Transform> queue = _poolDictionary[stats];
            List<Transform> list = _despawnDictionary[stats];

            if (despawnedObject.parent != stats.parent)
            {
                despawnedObject.SetParent(stats.parent);
            }
            despawnedObject.position = new Vector3(0, YPOS, 0);
            despawnedObject.rotation = Quaternion.identity;

            ResetValues(despawnedObject);
            despawnedObject.gameObject.SetActive(false);

            list.Remove(despawnedObject);
            queue.Enqueue(despawnedObject);
        }

        private void ResetValues(Transform @object)
        {
            IPooledObject pooledObjectComponent = @object.GetComponent<IPooledObject>();
            if (pooledObjectComponent != null)
            {
                @object.GetComponent<IPooledObject>().ResetValues();
            }
            else
            {
                Debug.LogWarning("Pooled object class should inherits IPooledObject to use Reset usage");
            }
        }

        public void DespawnAll(PooledObjectStats stats)
        {
            List<Transform> list = _despawnDictionary[stats];
            for (int i = list.Count - 1; i >= 0; i--)
            {
                Despawn(stats, list[i]);
            }
        }
    }
}