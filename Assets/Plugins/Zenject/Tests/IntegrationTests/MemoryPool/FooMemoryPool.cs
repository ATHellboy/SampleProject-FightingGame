using UnityEngine;

namespace Zenject.Tests.IntegrationTests.MemoryPool
{
    public class FooMemoryPool : MonoBehaviour
    {
        public class Pool : MonoMemoryPool<FooMemoryPool>
        {
        }
    }
}