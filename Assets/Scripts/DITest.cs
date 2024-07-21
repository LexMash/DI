using BaCon;
using UnityEngine;

namespace Assets.Scripts
{
    public class DITest : MonoBehaviour
    {
        private void Start()
        {
            var test1 = new TestClass1();

            var container = new DIContainer();

            container.Register<TestClass2, TestClass1>().WithTag("aaa").Bind();

            container.RegisterInstance<TestClass1, ITestClass1>(new TestClass1())
                .WithInjectionAction((c,i) => Debug.Log("uahahah"))
                .AsSingle()
                .WithTag("aaa")
                .NonLazy()
                .Bind();

            //Debug.Log(container.Resolve<TestClass1>().Name);
            //Debug.Log(container.Resolve<TestClass1>().Name);
            Debug.Log(container.Resolve<TestClass1>("aaa").Name);
        }
    }
    public class TestClass1 : ITestClass1
    {
        protected string name = "class1";
        public string Name => name;
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
