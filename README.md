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
- Instance and resolving for MonoBehaviour derived and gameobjects. (If you have previously registered the injection method)
- Resolving for all hierarchy of components gameobject (if use API Compatibility Level .Net Framework)
- NonLazy
- Contexts
- Disposables pattern for instances marked as single (all cashed objects marks like single)
- Resolving for collections of cashed instances

# Install and use
1. Add to your project all folder BaCon.
2. Place ProjectContext into folder Resources.
3. Place on your scenes object with SceneContext scripts
4. Add in lists in Project and Scene Contexts scripts derived from DIInstaller and implements method InstallBindings.
5. Use binder inside method InstallBindings for bind your dependencies.

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
##### You can omit the factory, and then the type will use the default constructor
```csharp
binder.Bind<Foo>(); // var foo = r.Resolve<Foo>() == var foo = new Foo();
```

### Tags
To add a tag, use this method
```csharp
binder.Bind<Foo>().WithTag("mybeautifultag");
```

### Lifetime
Uses lifetime by default is transient.
If you want to use the lifetime of a single use this method
To add a tag, use this method
```csharp
binder.Bind<Foo, IFoo>().AsSingle();
```

### Method injection (post instantiate action)
For a MonoBehaviour derivative, this method is the primary method for dependency injection, but you can use it for other classes as well.
```csharp
//mono
binder.BindInstance<Mono, IMono>().WithInjectionAction(Action<IDIResolver, TCurrent> injectAction); //sintax
binder.BindInstance<Mono, IMono>().WithInjectionAction((resolver, instance) => instance.Construct(resolver.Resolve<IMonoDependency>));

//nonMono
binder.BindInstance<Foo, IFoo>().WithInjectionAction((resolver, instance) => instance.Construct(resolver.Resolve<IFooDependency)); //without factory
binder.BindInstance<Foo, IFoo>((resolver) => new Foo(resolver.Resolve<FooDependency1>()).WithInjectionAction((resolver, instance) => instance.Construct(resolver.Resolve<FooDependency2>));
```
#### Important thing! For dependency injection, the instance type is always used, not the class under which it is registered.
After that, this method/action is registered in the resolver and you can use it for instances of the same type.
And you can use it in factories if you inject it inside as a dependency like that
```csharp
//register mono instance with injection method
binder.BindInstance<Mono, IMono>(myMono).WithInjectionAction((resolver, instance) => instance.Construct(resolver.Resolve<IMonoDependency));
//register the factory and inject the resolver as a dependency
binder.Bind<MonoFactory>((resolver) => new FooFactory(resolver); 

//inside factory - method create
public IMono Create(){
    var instance = Instantiate<Mono>(prefab);
    resolver.ResolveForInstance<Mono>(instance); //this method will be executed and will throw the dependency "(resolver, instance) => instance.Construct(resolver.Resolve<IMonoDependency)"
    return instance;
}
```

##### You can register methods separately from factories and object instances.
```csharp
binder.BindInjectionMethod<Mono>("tag1", (resolver, instance) => instance.Construct(resolver.Resolve<IMonoDependency1));
binder.BindInjectionMethod<Mono>("tag2", (resolver, instance) => instance.Construct(resolver.Resolve<IMonoDependency2));
binder.BindInjectionMethod<Mono>((resolver, instance) => instance.Construct(resolver.Resolve<IMonoDependency3));
```
