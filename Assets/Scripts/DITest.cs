using BaCon;
using UnityEngine;

namespace Assets.Scripts
{
    public class DITest : MonoBehaviour
    {
        public MonoTestObject MonoTestObject;

        private void Start()
        {
            var test1 = new TestClass1();

            var container = new DIContainer();

            container.Bind<TestClass2>().WithTag("aaa");

            container.BindInjectionMethod<TestClass2>((c, i) => i.Set("injected test2"));
            container.BindInjectionMethod<MonoTestObject>((c, i) => i.Construct());

            container.BindInstance<TestClass1, ITestClass1>(new TestClass1())
                .WithInjectionAction((c, i) => Debug.Log("uahahah"))
                .AsSingle()
                .WithTag("aaa")
                .NonLazy();

            container.BuildDomain();
            //var test2 = container.Resolve<TestClass2>("aaa");
            //container.ResolveForInstance(test2);
            container.InstantiateAndResolve(MonoTestObject);
            var test2 = container.InstantiateAndResolve<TestClass2>();

            Debug.Log(test2.Name);
            Debug.Log(container.Resolve<ITestClass1>("aaa").Name);
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

    public class TestClass2 : TestClass1
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
    }
}
