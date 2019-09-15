using UnityEngine;

namespace Infrastructure.ObjectPool
{
    public interface IPooledObject
    {
        PooledObjectStats PooledObjectStats { get; set; }

        void ReInitialize();
    }
}