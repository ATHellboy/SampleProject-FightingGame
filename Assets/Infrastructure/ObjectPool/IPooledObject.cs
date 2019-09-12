using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.ObjectPool
{
    public interface IPooledObject
    {
        PooledObjectStats PooledObjectStats { get; set; }

        void ReInitialize();
    }
}