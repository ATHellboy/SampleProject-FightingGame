using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject.Internal;

namespace Zenject.Tests.IntegrationTests.MemoryPool
{
    [TestFixture]
    public class TestMemoryPool : ZenjectIntegrationTestFixture
    {
        [UnityTest]
        public IEnumerator TestDestroyAnInactiveItemInPool()
        {
            PreInstall();
            Container.BindMemoryPool<FooMemoryPool, FooMemoryPool.Pool>()
                .FromNewComponentOnNewGameObject();
            PostInstall();

            var pool = Container.Resolve<FooMemoryPool.Pool>();
            FooMemoryPool f1 = pool.Spawn();
            FooMemoryPool f2 = pool.Spawn();
            FooMemoryPool f3 = pool.Spawn();

            Assert.IsFalse(ZenUtilInternal.IsNull(f1));
            Assert.IsFalse(ZenUtilInternal.IsNull(f2));
            Assert.IsFalse(ZenUtilInternal.IsNull(f3));

            pool.Despawn(f1);
            pool.Despawn(f2);
            pool.Despawn(f3);

            yield return null;
            GameObject.Destroy(f1.gameObject);
            GameObject.Destroy(f2.gameObject);
            yield return null;

            f1 = pool.Spawn();
            f2 = pool.Spawn();
            f3 = pool.Spawn();

            Assert.IsFalse(ZenUtilInternal.IsNull(f1));
            Assert.IsFalse(ZenUtilInternal.IsNull(f2));
            Assert.IsFalse(ZenUtilInternal.IsNull(f3));
        }
    }
}