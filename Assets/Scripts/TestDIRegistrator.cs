using Assets.Scripts;
using BaCon;
using UnityEngine;

public class TestDIRegistrator : DIRegistrator
{
    [SerializeField] private MonoTestObject monoTestObject;

    public override void BindEntries(IDIBinder binder)
    {
        binder.Bind<TestClass1>()
            .WithInjectionAction((resolver, instance) => instance.SetDepenecy(resolver.Resolve<ITestClass1>()))
            .WithTag("aaa");

        binder.BindInstance(monoTestObject)
            .WithInjectionAction((r, instance) => instance.Construct())
            .NonLazy();

        binder.BindInstance<TestClass2, ITestClass1>(new TestClass2())
            .WithInjectionAction((r, i) => Debug.Log("uahahah"))
            .AsSingle()
            .WithTag("aaa")
            .NonLazy();
    }
}
