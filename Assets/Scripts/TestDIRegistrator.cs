using Assets.Scripts;
using BaCon;
using UnityEngine;

public class TestDIRegistrator : DIRegistrator
{
    [SerializeField] private MonoTestObject monoTestObject;

    public override void RegisterEntries(IDIBinder binder)
    {
        binder.Bind<TestClass1>().WithInjectionAction((r,i) => i.SetInterface(r.Resolve<ITestClass1>())).WithTag("aaa");

        binder.BindInstance(monoTestObject).WithInjectionAction((c, i) => i.Construct()).NonLazy();

        binder.BindInstance<TestClass2, ITestClass1>(new TestClass2())
            .WithInjectionAction((c, i) => Debug.Log("uahahah"))
            .AsSingle()
            .WithTag("aaa")
            .NonLazy();
    }
}
