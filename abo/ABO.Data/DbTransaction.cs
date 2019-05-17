using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace ABO.Data
{
    public class DbTransaction : IDbTransaction
    {
        private DbContextTransaction _dbContextTransaction;

        public DbTransaction(DbContextTransaction dbContextTransaction)
        {
            _dbContextTransaction = dbContextTransaction;
        }

        #region IDbTransaction Members

        public void Commit()
        {
            _dbContextTransaction.Commit();
        }

        public void Rollback()
        {
            _dbContextTransaction.Rollback();
        }

        public void Dispose()
        {
            _dbContextTransaction.Dispose();
        }

        #endregion
    }
}
