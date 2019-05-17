using ABO.Core;
using ABO.Core.Domain;
using ABO.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace ABO.Data
{
    public partial class ABODataContext : DbContext, IDbContext
    {
        static ABODataContext()
        {
            Database.SetInitializer<ABODataContext>(null);

            // Register method to get object type
            EntityBase.GetObjectTypeMethod = new Func<EntityBase, Type>((entity) => System.Data.Entity.Core.Objects.ObjectContext.GetObjectType(entity.GetType()));
        }

        public ABODataContext()
            : base("Name=ABODataContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<DataLog> DataLogs { get; set; }
        public DbSet<DataPurge> DataPurges { get; set; }
        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<DistributorLetter> DistributorLetters { get; set; }
        public DbSet<DistributorLog> DistributorLogs { get; set; }
        public DbSet<DistributorUpdate> DistributorUpdates { get; set; }
        public DbSet<DistributorUpdateType> DistributorUpdateTypes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProfileBox> ProfileBoxes { get; set; }
        public DbSet<ProfileScan> ProfileScans { get; set; }
        public DbSet<ProfileType> ProfileTypes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DataLogMap());
            modelBuilder.Configurations.Add(new DataPurgeMap());
            modelBuilder.Configurations.Add(new DistributorMap());
            modelBuilder.Configurations.Add(new DistributorLetterMap());
            modelBuilder.Configurations.Add(new DistributorLogMap());
            modelBuilder.Configurations.Add(new DistributorUpdateMap());
            modelBuilder.Configurations.Add(new DistributorUpdateTypeMap());
            modelBuilder.Configurations.Add(new LocationMap());
            modelBuilder.Configurations.Add(new PermissionMap());
            modelBuilder.Configurations.Add(new ProfileMap());
            modelBuilder.Configurations.Add(new ProfileBoxMap());
            modelBuilder.Configurations.Add(new ProfileScanMap());
            modelBuilder.Configurations.Add(new ProfileTypeMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new StatusMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new WarehouseMap());
        }

        #region IDbContext Members

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : Core.EntityBase
        {
            return base.Set<TEntity>();
        }

        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Entities</returns>
        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : Core.EntityBase, new()
        {
            string spCommand = BuildCommandText(commandText, parameters);
            return ExecuteSqlList<TEntity>(spCommand, parameters);
        }

        public IList<TEntity> ExecuteSqlList<TEntity>(string commandText, params object[] parameters) where TEntity : Core.EntityBase, new()
        {
            var result = this.Database.SqlQuery<TEntity>(commandText, parameters).ToList();

            //performance hack applied as described here - http://www.nopcommerce.com/boards/t/25483/fix-very-important-speed-improvement.aspx
            bool acd = this.Configuration.AutoDetectChangesEnabled;
            try
            {
                this.Configuration.AutoDetectChangesEnabled = false;

                for (int i = 0; i < result.Count; i++)
                    result[i] = AttachEntityToContext(result[i]);
            }
            finally
            {
                this.Configuration.AutoDetectChangesEnabled = acd;
            }

            return result;
        }

        public int ExecuteSql(string sql, params object[] parameters)
        {
            return this.Database.ExecuteSqlCommand(sql, parameters);
        }

        private static string BuildCommandText(string commandText, object[] parameters)
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
            return commandText;
        }

        public void DetachEntity<TEntity>(TEntity entity) where TEntity : Core.EntityBase
        {
            #region Retrieve navigation properties and mark them as 'detached'
            //var type = typeof(TEntity);
            //var collectionProperties = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public).Where(x => x.PropertyType.Name == typeof(ICollection<>).Name);
            //foreach (var collectionProp in collectionProperties)
            //{
            //    this.Entry(entity).Collection(collectionProp.Name).EntityEntry.State = EntityState.Detached;
            //} 
            #endregion

            if (this.Entry<TEntity>(entity).State == EntityState.Added)
            {
                this.Set<TEntity>().Remove(entity);
            }
            else
            {
                var context = ((IObjectContextAdapter)this).ObjectContext;
                context.Refresh(System.Data.Entity.Core.Objects.RefreshMode.StoreWins, entity);
            }
        }

        public IDbTransaction BeginDbTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            return new DbTransaction(this.Database.BeginTransaction(isolationLevel));
        }

        #endregion

        /// <summary>
        /// Attach an entity to the context or return an already attached entity (if it was already attached)
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Attached entity</returns>
        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : Core.EntityBase, new()
        {
            ////little hack here until Entity Framework really supports stored procedures
            ////otherwise, navigation properties of loaded entities are not loaded until an entity is attached to the context
            //var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
            //if (alreadyAttached == null)
            //{
            //    //attach new entity
            //    Set<TEntity>().Attach(entity);
            //    return entity;
            //}
            //else
            //{
            //    //entity is already loaded.
            //    return alreadyAttached;
            //}

            //Set<TEntity>().Attach(entity);
            return entity;
        }

    }
}

