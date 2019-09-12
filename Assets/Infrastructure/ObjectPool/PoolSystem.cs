﻿using Infrastructure.Factory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Infrastructure.ObjectPool
{
    public class PoolSystem : MonoBehaviour
    {
        private const int YPOS = 9999;

        private PooledObjectStats[] _pooledObjectsStats;
        private IResourceFactory _resourceFactory;
        private Dictionary<PooledObjectStats, Queue<Transform>> _poolDictionary = new Dictionary<PooledObjectStats, Queue<Transform>>();

        // TODO: You may replace it with something better
        private Canvas _canvas;

        [Inject]
        public void Construct(IResourceFactory resourceFactory, Canvas canvas)
        {
            _resourceFactory = resourceFactory;
            _canvas = canvas;
        }

        void Awake()
        {
            _pooledObjectsStats = Resources.LoadAll<PooledObjectStats>("");

            CreateAll();
        }

        private Transform Create(Queue<Transform> queue, PooledObjectStats stats, int index)
        {
            Transform pooledObject = _resourceFactory.Instantiate(stats.prefab, new Vector3(0, YPOS, 0), stats.parent);
            pooledObject.name += " " + index;
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
                Queue<Transform> queue = new Queue<Transform>();
                GameObject parent;

                if (stats.prefab.GetComponent<RectTransform>() == null)
                {
                    parent = new GameObject(stats.name + "s");
                    parent.transform.SetParent(transform);
                }
                else
                {
                    parent = new GameObject(stats.name + "s", typeof(RectTransform));
                    parent.transform.SetParent(_canvas.transform);
                    ResetRectTransform(parent);
                }
                stats.parent = parent.transform;

                for (int j = 0; j < _pooledObjectsStats[i].number; j++)
                {
                    Transform pooledObject = Create(queue, stats, j);
                    queue.Enqueue(pooledObject);
                }
                _poolDictionary.Add(_pooledObjectsStats[i], queue);
            }
        }

        public Transform Spawn(PooledObjectStats stats)
        {
            Queue<Transform> queue = _poolDictionary[stats];

            Transform spawnedObject = queue.Dequeue();

            if (spawnedObject == null)
            {
                spawnedObject = Create(queue, stats, 1);

                if (stats.expandBy == ExpandBy.Doubling)
                {
                    for (int j = 0; j < stats.number - 1; j++)
                    {
                        Transform pooledObject = Create(queue, stats, j);
                        queue.Enqueue(pooledObject);
                    }
                }
            }

            spawnedObject.gameObject.SetActive(true);

            StartCoroutine(ReInitializeAtTheEndofFrame(spawnedObject));

            return spawnedObject;
        }

        public Transform Spawn(PooledObjectStats stats, Vector3 position, Quaternion rotation)
        {
            Transform spawnedObject = Spawn(stats);

            spawnedObject.position = position;
            spawnedObject.rotation = rotation;

            return spawnedObject;
        }

        // TODO: It may placeholder
        IEnumerator ReInitializeAtTheEndofFrame(Transform spawnedObject)
        {
            yield return new WaitForEndOfFrame();
            IPooledObject pooledObjectComponent = spawnedObject.GetComponent<IPooledObject>();
            if (pooledObjectComponent != null)
            {
                spawnedObject.GetComponent<IPooledObject>().ReInitialize();
            }
            else
            {
                Debug.LogWarning("Pooled object class should inherits IPooledObject to use ReInitialize usage");
            }
        }

        public void Despawn(PooledObjectStats stats, Transform despawnedObject)
        {
            Queue<Transform> queue = _poolDictionary[stats];

            despawnedObject.position = new Vector3(0, YPOS, 0);
            despawnedObject.rotation = Quaternion.identity;
            queue.Enqueue(despawnedObject);
            despawnedObject.gameObject.SetActive(false);
        }

        private void ResetRectTransform(GameObject @object)
        {
            RectTransform rectTransform = @object.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.localScale = Vector3.one;
        }
    }
}