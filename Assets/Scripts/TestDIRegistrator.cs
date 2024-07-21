using Assets.Scripts;
using BaCon;
using UnityEngine;

public class TestDIRegistrator : DIRegistrator
{
    [SerializeField] private MonoTestObject monoTestObject;

    public override void RegisterEntries(IDIContainer container)
    {
        container.Register<TestClass1>().WithInjectionAction((c,i) => i.SetInterface(c.Resolve<ITestClass1>())).WithTag("aaa");

        container.RegisterInstance(monoTestObject).WithInjectionAction((c, i) => i.Construct()).NonLazy();

        container.RegisterInstance<TestClass2, ITestClass1>(new TestClass2())
            .WithInjectionAction((c, i) => Debug.Log("uahahah"))
            .AsSingle()
            .WithTag("aaa")
            .NonLazy();
    }
}
