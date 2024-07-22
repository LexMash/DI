# BaCon - LexMash edition
Original BaCon available here https://github.com/vavilichev/BaCon
BaCon is the simplest DIContainer for Unity without reflection with minimal functionality and injection through scene contexts.
# Features
- Instance registration
- Factory registration
- Tags support
- Single and Transient lifetime
- Method injection
- Resolving for instances, MonoBehaviour derived and gameobjects.  (If you have previously registered the injection method)
- Instance and resolving for nonMono, MonoBehaviour derived and gameobjects. (If you have previously registered the injection method)
- NonLazy
- Contexts

### Instance registration
```csharp
binder.BindInstance<InstanceType>(instance));
binder.BindInstance<Foo>(new Foo()));

binder.BindInstance<InstanceType, BindedType>(new Foo());
binder.BindInstance<Foo, IFoo>(new Foo());
```

### Factory registration
```csharp
binder.Bind<InstanceType>(Func(IDIResolver, newInstance) factory);
binder.Bind<Foo>((resolver) => new Foo(resolver.Resolve<FooDependency>());

binder.Bind<InstanceType, TargetType>(Func(IDIResolver, newInstance) factory);
binder.Bind<Foo, IFoo>((resolver) => new Foo(resolver.Resolve<FooDependency>());
```
### You can omit the factory, and then the type will use the default constructor
```csharp
binder.Bind<Foo>(); // var foo = r.Resolve<Foo>() == var foo = new Foo();
```
Please look at the example for DITestRegistration
```csharp
    [SerializeField] private MonoTestObject monoTestObject;

    public override void InstallBindings(IDIBinder binder)
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
```
