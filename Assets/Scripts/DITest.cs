using BaCon;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class DITest : MonoBehaviour
    {
        public MonoTestObject MonoTestObject;
        private DIContainer binder;

        private void Start()
        {
            binder = new DIContainer();

            binder.Bind<TestClass2, ITestClass1>().WithInjectionAction((c, i) => Debug.Log("1a")).WithTag("1").AsCashed().NonLazy();
            binder.Bind<TestClass2, ITestClass1>().WithInjectionAction((c, i) => Debug.Log("2a")).WithTag("2").AsCashed().NonLazy();
            binder.Bind<TestClass2, ITestClass1>().WithInjectionAction((c, i) => Debug.Log("3a")).WithTag("3").AsCashed().NonLazy();
            binder.Bind<TestClass2, ITestClass1>().WithInjectionAction((c, i) => Debug.Log("4a")).WithTag("4").AsCashed().NonLazy();

            binder.BuildContext();
        }

        private void OnDestroy()
        {
            binder.Dispose();
        }
    }

    public class TestClass1 : ITestClass1
    {
        protected string name = "class1";
        public string Name => name;

        public TestClass1()
        {
        }

        public void SetDepenecy(ITestClass1 testClassI)
        {
            Debug.Log(testClassI.Name);
        }
    }

    public interface ITestClass1
    {
        string Name { get; }
    }

    public class TestClass2 : TestClass1, IDisposable
    {
        public TestClass2()
        {
            name = "class2";
            counter++;
            Debug.Log(counter);
        }

        public static int counter;

        public void Set(string name)
        {
            this.name = name;        
        }

        public void Dispose()
        {
            Debug.Log("dispose" + counter);
        }
    }
}
