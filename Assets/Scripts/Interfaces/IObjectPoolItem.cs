using System;
using Synthesize.ObjectPoolSystem;
using UnityEngine;

namespace Synthesize.Interfaces
{
    public interface IObjectPoolItem
    {
        void SetObjectPool<T>(ObjectPool pool, T comp) where T : Component;

        void Release();
    }
}