using System;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmShowcase.Performance
{
    public interface IPoolable
    {
        void OnRented();
        void OnReturned();
    }

    /// <summary>
    /// A small, type-safe component pool. The pool owns activation state, while the component
    /// owns visual and gameplay reset logic through IPoolable.
    /// </summary>
    public sealed class ComponentPool<T> where T : Component, IPoolable
    {
        private readonly T prefab;
        private readonly Transform inactiveParent;
        private readonly Stack<T> available = new Stack<T>();
        private readonly HashSet<T> rented = new HashSet<T>();

        public ComponentPool(T prefab, Transform inactiveParent, int warmCount = 0)
        {
            this.prefab = prefab != null ? prefab : throw new ArgumentNullException(nameof(prefab));
            this.inactiveParent = inactiveParent;

            for (int index = 0; index < warmCount; index++)
            {
                available.Push(CreateInstance());
            }
        }

        public int AvailableCount => available.Count;
        public int RentedCount => rented.Count;

        public T Rent(Transform parent = null)
        {
            T instance = available.Count > 0 ? available.Pop() : CreateInstance();
            rented.Add(instance);
            instance.transform.SetParent(parent, false);
            instance.gameObject.SetActive(true);
            instance.OnRented();
            return instance;
        }

        public bool Return(T instance)
        {
            if (instance == null || !rented.Remove(instance))
            {
                return false;
            }

            instance.OnReturned();
            instance.gameObject.SetActive(false);
            instance.transform.SetParent(inactiveParent, false);
            available.Push(instance);
            return true;
        }

        private T CreateInstance()
        {
            T instance = UnityEngine.Object.Instantiate(prefab, inactiveParent);
            instance.gameObject.SetActive(false);
            return instance;
        }
    }
}
