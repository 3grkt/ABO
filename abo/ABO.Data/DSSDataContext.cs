using ABO.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace ABO.Data
{
    public partial class DSSDataContext : DbContext, IDbContext
    {
        static DSSDataContext()
        {
            // Register method to get object type
            EntityBase.GetObjectTypeMethod = new Func<EntityBase, Type>((entity) => System.Data.Entity.Core.Objects.ObjectContext.GetObjectType(entity.GetType()));
        }

        public DSSDataContext()
            : base("Name=DSSDataContext")
        {
        }

        #region IDbContext Members

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : EntityBase
        {
            throw new NotImplementedException();
        }

        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : EntityBase, new()
        {
            //add parameters to command
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not support parameter type");

                    if (p.Value == null)
                        p.Value = DBNull.Value;

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        //output parameter
                        commandText += " output";
                    }
                }
            }

            var result = this.Database.SqlQuery<TEntity>(commandText, parameters).ToList();

            return result;
        }

        public int ExecuteSql(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public void DetachEntity<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            throw new NotImplementedException();
        }

        public IDbTransaction BeginDbTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            throw new NotImplementedException();
        }

        public IList<TEntity> ExecuteSqlList<TEntity>(string commandText, params object[] parameters) where TEntity : EntityBase, new()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
