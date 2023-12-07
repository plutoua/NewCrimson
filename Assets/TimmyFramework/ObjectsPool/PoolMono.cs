using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TimmyFramework
{
    public class PoolMono<T> 
        where T : MonoBehaviour
    {
        public T Prefab { get; }
        public bool AutoExpand { get; set; }
        public Transform Container { get; }

        private List<T> _pool;

        public PoolMono(Transform container)
        {
            Container = container;
        }

        public PoolMono(Transform container, T prefab, int count) : this(container)
        {
            Prefab = prefab;
            CreatePool(count);
        }

        public void PreparePool(T prefab, int count)
        {
            CreatePool(count);
        }

        public bool HasFreeElement(out T element)
        {
            foreach (var item in _pool)
            {
                if (!item.gameObject.activeInHierarchy)
                {
                    element = item;
                    element.gameObject.SetActive(true);
                    return true;
                }
            }
            element = null;
            return false;
        }

        public T GetFreeElement()
        {
            if (HasFreeElement(out var element))
            {
                return element;
            }

            if (AutoExpand)
            {
                return CreateObject(true);
            }

            throw new Exception("Is no free elements");
        }

        private void CreatePool(int count)
        {
            _pool = new List<T>();
            for (int i = 0; i < count; i++)
            {
                CreateObject();
            }
        }

        private T CreateObject(bool isActiveByDefault = false)
        {
            var createdObject = Object.Instantiate(this.Prefab, this.Container);
            createdObject.gameObject.SetActive(isActiveByDefault);
            _pool.Add(createdObject);
            return createdObject;
        }
    }
}