using BaCon;
using UnityEngine;

namespace Assets.Scripts
{
    public class MonoTestObject : MonoBehaviour, IInjectable
    {
        public string Tag => null;

        public void Construct()
        {
            Debug.Log("Construct");
        }
    }
}
