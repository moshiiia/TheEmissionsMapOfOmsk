using Microsoft.VisualStudio.TestTools.UnitTesting;
using MainModel.DataProviders.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MainModel.DataProviders.EntityFramework.Tests
{
    [TestClass()]
    public class EfDbContextTests
    {
        [TestMethod()]
        public void EfDbContextTest()
        {
            EfDbContext context = new(EfProvider.SqLite); 
            Assert.IsTrue(File.Exists(@"C:\Users\Root\source\repos\TheEmisssionsMapOfOmsk\src\Data\Map.db"));
            Assert.IsTrue((new FileInfo(@"C:\Users\Root\source\repos\TheEmisssionsMapOfOmsk\src\Data\Map.db"))
                .Length > 0);
        }
    }
}