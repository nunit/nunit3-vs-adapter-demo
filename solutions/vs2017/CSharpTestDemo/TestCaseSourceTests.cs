using System.Collections;
using NUnit.Framework;

namespace NUnitTestDemo
{
    [TestFixture]
    public class TestCaseSourceTests
    {
        [TestCaseSource(typeof(MyDataClass), "TestCases")]
        public int DivideTest(int n, int d)
        {
            return n / d;
        }
    }

    public class MyDataClass
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(12, 3).Returns(4);
                yield return new TestCaseData(12, 2).Returns(6);
                yield return new TestCaseData(12, 4).Returns(3);
            }
        }
    }


    [TestFixtureSource(typeof(MyFixtureData), "FixtureParms")]
    public class ParameterizedTestFixture
    {
        private string eq1;
        private string eq2;
        private string neq;

        public ParameterizedTestFixture(string eq1, string eq2, string neq)
        {
            this.eq1 = eq1;
            this.eq2 = eq2;
            this.neq = neq;
        }

        public ParameterizedTestFixture(string eq1, string eq2)
            : this(eq1, eq2, null) { }

        public ParameterizedTestFixture(int eq1, int eq2, int neq)
        {
            this.eq1 = eq1.ToString();
            this.eq2 = eq2.ToString();
            this.neq = neq.ToString();
        }

        [Test]
        public void TestEquality()
        {
            Assert.AreEqual(eq1, eq2);
            if (eq1 != null && eq2 != null)
                Assert.AreEqual(eq1.GetHashCode(), eq2.GetHashCode());
        }

        [Test]
        public void TestInequality()
        {
            Assert.AreNotEqual(eq1, neq);
            if (eq1 != null && neq != null)
                Assert.AreNotEqual(eq1.GetHashCode(), neq.GetHashCode());
        }
    }


    public class MyFixtureData
    {
        public static IEnumerable FixtureParms
        {
            get
            {
                yield return new TestFixtureData("hello", "hello", "goodbye");
                yield return new TestFixtureData("zip", "zip");
                yield return new TestFixtureData(42, 42, 99);
            }
        }
    }
}
