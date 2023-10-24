// Copyright (c) ATHellboy (Alireza Tarahomi) Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Factory
{
    public class VContainerResourceFactory : IResourceFactory
    {
        private readonly IObjectResolver _container;

        public VContainerResourceFactory(IObjectResolver container)
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
            Transform obj = _container.Instantiate(transform).transform;
            return obj;
        }

        public Transform Instantiate(Transform transform, Transform parent)
        {
            Transform obj = _container.Instantiate(transform, parent).transform;
            return obj;
        }

        public Transform Instantiate(Transform transform, Vector3 position)
        {
            Transform obj = _container.Instantiate(transform, position, Quaternion.identity).transform;
            return obj;
        }

        public Transform Instantiate(Transform transform, Vector3 position, Transform parent)
        {
            Transform obj = _container.Instantiate(transform, position, Quaternion.identity, parent).transform;
            return obj;
        }

        public Transform Instantiate(Transform transform, Vector3 position, Quaternion rotation, Transform parent)
        {
            Transform obj = _container.Instantiate(transform, position, rotation, parent).transform;
            return obj;
        }
    }
}