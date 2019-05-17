using ABO.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSS = ABO.Core.Domain.DSS;

namespace ABO.Tests.Data
{
    [TestFixture]
    public class DSSDataContextTests : TestSuiteBase
    {
        [Test]
        public void ExecuteStoreProcedureInDSS()
        {
            var dssContext = new DSSDataContext();
            var distributors = dssContext.ExecuteStoredProcedureList<DSS.Distributor>("usp_tblDistributor_GetAll", new object[] { });

            Assert.GreaterOrEqual(distributors.Count, 0);

            Console.WriteLine("Record count: " + distributors.Count);
            PrintDSSDistributors(distributors);
        }
    }
}
