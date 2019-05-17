using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using ABO.Core;
using ABO.Core.Domain.WTA;
using ABO.Data.Mapping.WTA;

namespace ABO.Data
{
    public class WTADataContext: DbContext, IDbContext
    {
        static WTADataContext()
        {
            // Register method to get object type
            EntityBase.GetObjectTypeMethod = new Func<EntityBase, Type>((entity) => System.Data.Entity.Core.Objects.ObjectContext.GetObjectType(entity.GetType()));
        }
        public WTADataContext(): base("Name=WTADataContext")
        {
            
        }

        public virtual DbSet<ProspectAvatar> ProspectAvatars { get; set; }

        public WTADataContext(DbConnection connection, bool ownContext) : base(connection, ownContext)
        {
            
        }

        // public DbSet<Actor> Actors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new tblProspectAvatarMap());
        }

        public IDbSet<TEntity> Set<TEntity>() where TEntity : EntityBase
        {
            return base.Set<TEntity>();
        }

        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : EntityBase, new()
        {
            throw new NotImplementedException();
        }

        public IList<TEntity> ExecuteSqlList<TEntity>(string commandText, params object[] parameters) where TEntity : EntityBase, new()
        {
            try
            {
                // var resultString = Database.SqlQuery<string>("select first_name from actor", parameters).ToList();
                var result = Database.SqlQuery<TEntity>("select actor_id, last_name, first_name, last_update from actor", parameters).ToList();


                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            
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
               return new DbTransaction(this.Database.BeginTransaction(isolationLevel));
        }
    }
}
