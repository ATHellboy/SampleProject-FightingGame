// Copyright (c) ATHellboy (Alireza Tarahomi) Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using UnityEngine;

namespace Infrastructure.ObjectPooling
{
    public interface IPooledObject
    {
        PooledObjectStats PooledObjectStats { get; set; }

        void ReInitialize();
    }
}