using Assets.Scripts;
using BaCon;
using UnityEngine;

public class TestDIRegistrator : DIInstaller
{
    [SerializeField] private MonoTestFactory factory;

    public override void InstallBindings(IDIBinder binder)
    {
        binder.Bind<TestClass1>()
            .WithInjectionAction((resolver, instance) => instance.SetDepenecy(resolver.Resolve<ITestClass1>()))
            .WithTag("aaa");

        binder.BindInjectionMethod<MonoTestObject>((r, instance) => instance.Construct());

        binder.BindInstance<TestClass2, ITestClass1>(new TestClass2())
            .WithInjectionAction((r, i) => Debug.Log("uahahah"))
            .AsSingle()
            .WithTag("aaa");
            //NonLazy();

        binder.BindInstance(factory).WithInjectionAction((resolver, instance) => instance.Construct(resolver)).NonLazy();
    }
}
