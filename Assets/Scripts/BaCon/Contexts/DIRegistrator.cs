﻿using UnityEngine;

namespace BaCon
{
    public abstract class DIRegistrator : MonoBehaviour
    {
        public abstract void RegisterEntries(IDIContainer container);
    }
}
