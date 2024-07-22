using UnityEngine;

namespace BaCon
{
    public abstract class DIInstaller : MonoBehaviour
    {
        public abstract void InstallBindings(IDIBinder binder);
    }
}
