using ABO.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace ABO.Data
{
    public interface IDbContext
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : EntityBase;
        IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : Core.EntityBase, new();
        IList<TEntity> ExecuteSqlList<TEntity>(string commandText, params object[] parameters) where TEntity : Core.EntityBase, new();
        int ExecuteSql(string sql, params object[] parameters);
        int SaveChanges();
        void DetachEntity<TEntity>(TEntity entity) where TEntity : Core.EntityBase;
        IDbTransaction BeginDbTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
    }
}
