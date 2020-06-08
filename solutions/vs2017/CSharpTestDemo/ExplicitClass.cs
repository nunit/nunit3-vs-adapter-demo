using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NUnitTestDemo
{
    [Explicit]
    public class ExplicitClass
    {
        [Test]
        public void ThisIsIndirectlyExplicit()
        {
            Assert.Pass();
        }
    }
}
