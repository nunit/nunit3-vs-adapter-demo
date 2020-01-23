using System;
using NUnit.Framework;

#if NETCOREAPP1_1 || NETCOREAPP2_2
namespace NUnitCoreTestDemo
#else
namespace NUnitTestDemo
#endif
{
    public abstract class InheritedTestBaseClass
    {
        [Test]
        public void TestInBaseClass()
        {
        }
    }

    public class InheritedTestDerivedClass : InheritedTestBaseClass
    {
    }
}
