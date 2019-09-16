// Copyright (c) ATHellboy (Alireza Tarahomi) Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using UnityEngine;
using Zenject;

namespace Infrastructure.Factory
{
    public interface IResourceFactory
    {
        Object Instantiate(Object @object);
        Transform Instantiate(Transform transform);
        Transform Instantiate(Transform transform, Transform parent);
        Transform Instantiate(Transform transform, Vector3 position);
        Transform Instantiate(Transform transform, Vector3 position, Transform parent);
        Transform Instantiate(Transform transform, Vector3 position, Quaternion rotation, Transform parent);
    }
}