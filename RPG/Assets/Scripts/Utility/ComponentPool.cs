using UnityEngine;
using UnityEngine.Pool;

namespace Utility
{
    public class ComponentPool<T> where T : Component
    {
        private readonly T prefab;
        private readonly Transform parent;

        private readonly ObjectPool<T> pool;

        public ComponentPool(T prefab, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;

            pool = new ObjectPool<T>(Create, OnGet, OnRelease);
        }

        private T Create()
        {
            return Object.Instantiate(prefab, parent);
        }

        private void OnGet(T instance)
        {
            instance.gameObject.SetActive(true);
        }

        private void OnRelease(T instance)
        {
            instance.gameObject.SetActive(false);
        }

        public T Get()
        {
            return pool.Get();
        }

        public void Release(T instance)
        {
            pool.Release(instance);
        }
    }
}