using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MainModel.DataProviders.EntityFramework.Tests;

[TestClass()]
public class EfDbContextTests
{
    [TestMethod()]
    public void EfDbContextTest()
    {
        EfDbContext context = new(EfProvider.SqLite);

        //.Points.UpdateAsync();

        //Assert.IsTrue(true);
        //Assert.IsTrue((new FileInfo(@"C:\Users\Root\source\repos\TheEmisssionsMapOfOmsk\src\Data\Map.db"))
        //    .Length > 0);
    }
}