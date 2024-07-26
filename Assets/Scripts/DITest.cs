﻿using BaCon;
using System;
using UnityEngine;
using static UnityEditor.Progress;

namespace Assets.Scripts
{
    public class DITest : MonoBehaviour
    {
        public MonoTestObject MonoTestObject;
        private DIContainer binder;

        private void Start()
        {
            var test1 = new TestClass1();
            binder = new DIContainer();

            binder.Bind<TestClass2, ITestClass1>().WithInjectionAction((c, i) => Debug.Log("1a")).WithTag("1").AsCashed().NonLazy();
            binder.Bind<TestClass2, ITestClass1>().WithInjectionAction((c, i) => Debug.Log("2a")).WithTag("2").AsCashed().NonLazy();
            binder.Bind<TestClass2, ITestClass1>().WithInjectionAction((c, i) => Debug.Log("3a")).WithTag("3").AsCashed().NonLazy();
            binder.Bind<TestClass2, ITestClass1>().WithInjectionAction((c, i) => Debug.Log("4a")).WithTag("4").AsCashed().NonLazy();

            //binder.BindInjectionMethod<TestClass2>((r, i) => i.Set("injected test2"));
            //binder.BindInjectionMethod<MonoTestObject>((r, i) => i.Construct());

            //binder.BindInstance<TestClass1, ITestClass1>(new TestClass1())
            //    .WithInjectionAction((c, i) => Debug.Log("uahahah"))
            //    .AsSingle()
            //    .WithTag("aaa")
            //    .NonLazy();

            binder.BuildContext();

            //var test2 = container.Resolve<TestClass2>("aaa");
            //container.ResolveForInstance(test2);
            //binder.InstantiateAndResolve(MonoTestObject);
            //Debug.Log(binder.Resolve<ITestClass1>("aaa").Name);
            //binder.ResolveAllHierarchy(MonoTestObject.gameObject);

            //Debug.Log("resolving");
            //var list = binder.ResolveAll<ITestClass1>();
            //var list2 = binder.ResolveAll<ITestClass1>();

            //foreach (var item in list)
            //{
            //    Debug.Log(item.Name);
            //}
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
