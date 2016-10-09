using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor.Tests
{
    [TestClass()]
    public class ComponentHelperTests
    {
        int i = 0;
        public void f1()
        {
            i = 100;
        }

        static int si = 0;
        public static void f2()
        {
            si = 100;
        }

        [TestMethod()]
        public void getDynamicFuncTest1()
        {
            var f = ComponentHelper.getDynamicFunc(this, "f1");
            Assert.IsTrue(f != null);
            f();
            Assert.IsTrue(i == 100);
        }

        [TestMethod()]
        public void getDynamicFuncTest2()
        {
            var f = ComponentHelper.getDynamicFunc(this, "f2", false);
            Assert.IsTrue(f != null);
            f();
            Assert.IsTrue(si == 100);
        }
    }

}