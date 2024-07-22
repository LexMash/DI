using UnityEngine;

namespace BaCon
{
    public abstract class DIRegistrator : MonoBehaviour
    {
        public abstract void BindEntries(IDIBinder container);
    }
}
