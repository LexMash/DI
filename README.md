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

### Instance and resolving for MonoBehaviour derived and gameobjects.
Once you register a method for a class, you can use instance creation and resolving for that mono-derived class.
```csharp
//inside factory - method create
public IMono Create(){
    return resolver.InstantiateAndResolve<Mono>(gameobject);
}

public IMono Create(){
    return resolver.InstantiateAndResolve<Mono>(MonoPrefab) //prefab with Mono Component
}
```

### Resolving for all hierarchy of components gameobject (if use API Compatibility Level .Net Framework)
A monoderived class must implement the IInjectable interface.
You must specify a tag if the injection method is registered with a tag. If it is not present, then you must return an string.Empty.
After all you can use methods ResolveAllHierarchy
```csharp
public class MonoDerived : MonoBehaviour, IInjectable{
    public string Tag => "myTag"
}

ResolveAllHierarchy<MonoDerived>(monoDerivedInstance); //generic version
ResolveAllHierarchy(monoDerivedInstance.gameObject); // for gameobjects
```

### Non Lazy
##### If a class is created with the NonLazy mark, it is automatically marked as a singleton!
By default, all objects are created on request. If you want the object to be created immediately after registering all dependencies, then use the NonLazy method
```csharp
binder.Bind<Foo>().NonLazy();
binder.BindInstance<Mono, IMono>().WithInjectionAction((resolver, instance) => instance.Construct(resolver.Resolve<IMonoDependency>)).NonLazy();
```

### Contexts
There are two contexts - the project context and the scene context. There can be one context for each scene.
The project context is created by default and should not be modified in any way except for adding DIInstaller derived.

### Resolving for collections of cashed instances
You can get a list of objects of a particular class or interface if you use the AsCashed method during registration
```csharp
binder.Bind<TestClass1, ITestClass1>().WithTag("1").AsCashed().NonLazy();
binder.Bind<TestClass2, ITestClass1>().WithInjectionAction((c, i) => Debug.Log("2a")).AsCashed().WithTag("2");
binder.Bind<TestClass3, ITestClass1>().WithTag("3").AsCashed().WithInjectionAction((resolver, instance) => instance.Construct(resolver.Resolve<TestClass2));
binder.Bind<TestClass4, ITestClass1>((resolver) => new TestClass4(resolver.Resolve<TestClass1>()).WithTag("4").AsCashed();

//after
binder.Bind<TestClassHandler>((resolver) => new TestClassHandler(resolver.ResolveAll<ITestClass1>()));
```
A read-only list of all registered objects with the given interface or class will be injected.
#### If a class is created with the AsCashed mark, it is automatically marked as a singleton!
#### Be sure to use tag to distinguish objects. Otherwise, there will be a registration error.
