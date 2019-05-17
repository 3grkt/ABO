using ABO.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ABO.Data
{
    public class SqlServerDataProvider : IDataProvider
    {
        public DbParameter CreateParameter()
        {
            return new SqlParameter();
        }

        public DbParameter CreateParameter(string name, object value, DbType dataType, ParameterDirection direction = ParameterDirection.Input)
        {
            var parameter = new SqlParameter(name, value);
            parameter.DbType = dataType;
            parameter.Direction = direction;
            return parameter;
        }

        public DateTime? AddDays(DateTime? date, int? dateToAdd)
        {
            return DbFunctions.AddDays(date, dateToAdd);
        }

        public DateTime? AddMonths(DateTime? date, int? monthsToAdd)
        {
            return DbFunctions.AddMonths(date, monthsToAdd);
        }

        public DateTime? AddYears(DateTime? date, int? yearsToAdd)
        {
            return DbFunctions.AddYears(date, yearsToAdd);
        }

    }
}
