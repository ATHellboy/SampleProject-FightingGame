// Copyright (c) ATHellboy (Alireza Tarahomi) Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using UnityEngine;
using Zenject;

namespace Infrastructure.Factory
{
    // TODO: More option for instantiating like Unity
    public class ZenjectResourceFactory : IResourceFactory
    {
        private readonly DiContainer _container;

        public ZenjectResourceFactory(DiContainer container)
        {
            _container = container;
        }

        public Object Instantiate(Object @object)
        {
            Object instance = Object.Instantiate(@object);
            _container.Inject(instance);
            return instance;
        }

        public Transform Instantiate(Transform transform)
        {
            Transform obj = _container.InstantiatePrefab(transform).transform;
            return obj;
        }

        public Transform Instantiate(Transform transform, Transform parent)
        {
            Transform obj = _container.InstantiatePrefab(transform, parent).transform;
            return obj;
        }

        public Transform Instantiate(Transform transform, Vector3 position)
        {
            Transform obj = _container.InstantiatePrefab(transform).transform;
            obj.transform.position = position;
            return obj;
        }

        public Transform Instantiate(Transform transform, Vector3 position, Transform parent)
        {
            Transform obj = _container.InstantiatePrefab(transform, parent).transform;
            obj.transform.position = position;
            return obj;
        }

        public Transform Instantiate(Transform transform, Vector3 position, Quaternion rotation, Transform parent)
        {
            Transform obj = _container.InstantiatePrefab(transform, parent).transform;
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj;
        }
    }
}