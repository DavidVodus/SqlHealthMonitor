using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Common;

using System.Data.Entity.Validation;
using System.Text;
using SqlHealthMonitor.DAL.Helpers;
using System.Data.Entity.Infrastructure;
using System.Security.Principal;
using System.Data.Entity.Core.Objects;

namespace SqlHealthMonitor.DAL.Repositories
{
    public abstract class RepositoryBase<T> : LoggerBase, IRepository<T>, IDisposable
   where T : class
    {
        protected DbContext _dbContext;
        private bool disposed = false;

        protected RepositoryBase(DbContext context)
        {
            _dbContext = context;
            SetTransactionTimeOut();
        }

        protected DbSet<T> Data => _dbContext.Set<T>();
        private IEnumerable<string> FindPkNames()
        {
            ObjectContext objectContext = ((IObjectContextAdapter)_dbContext).ObjectContext;
            ObjectSet<T> set = objectContext.CreateObjectSet<T>();
           return  set.EntitySet.ElementType
                                                        .KeyMembers
                                                        .Select(k => k.Name);
        }

        private void SetTransactionTimeOut()
        {
            //Get machineSettings session
            var machineSettings = (System.Transactions.Configuration.MachineSettingsSection)ConfigurationManager.GetSection("system.transactions/machineSettings");
            //Allow modifications
            var bReadOnly = (typeof(ConfigurationElement)).GetField("_bReadOnly", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            bReadOnly.SetValue(machineSettings, false);
            //Change max allowed timeout
            machineSettings.MaxTimeout = TimeSpan.MaxValue;
            ///TODO :tohle je divny dodelat ulozit kontext trebas do privatni prom tehle triy
        }
       

        public void Add(T entity,Expression<Func<T, object>> childrenToUnchanged = null)
        {

            if (childrenToUnchanged != null)
                foreach (MemberExpression argument in ((NewExpression)childrenToUnchanged.Body).Arguments)
                {
                  var childrenEntity=_dbContext.Entry<T>(entity).Member(argument.Member.Name);
                    childrenEntity.EntityEntry.State = EntityState.Unchanged;
                }
            Data.Add(entity);

        }

        public void Delete(T entity)
        {
            Data.Remove(entity);
        }

        public virtual IList<T> GetAll()
        {
            return Data.ToList<T>();
        }

        public IList<T> GetAll(Expression<Func<T, bool>> whereCondition)
        {
            return Data.Where(whereCondition).ToList<T>();
        }

        public T GetSingle(Expression<Func<T, bool>> whereCondition)
        {
            return Data.Where(whereCondition).FirstOrDefault<T>();
        }

        public void Attach(T entity)
        {
            Data.Attach(entity);
        }

        //private void Update(T entity, Action<string> compareProperty, Action<string> childProperty)
        //{
        //    var naviName = "Columns";
        //    var keyName = _dbContext.Db<T>().Properties.Where(x => x.IsPk).First().PropertyName; ;
        //    var key = typeof(T).GetProperty(keyName).GetValue(entity);
        //    T attachedEntity = _dbContext.Set<T>().Find(key);
        //    var entry = _dbContext.Entry<T>(attachedEntity);
        //    entry.CurrentValues.SetValues(entity);

        //    var dbChildrenCollection = entry.Collection(naviName);
        //    dbChildrenCollection.Load();

        //    var entityNav = _dbContext.Db(dbChildrenCollection.Query().ElementType).Properties.Where(x => x.IsNavigationProperty).First().PropertyName;
        //    var inMemoryChildrenCollection = entity.GetType().GetProperty(naviName).GetValue(entity) as IList;
        //    foreach (var item in dbChildrenCollection.Query())
        //    {
        //        var entryChild = _dbContext.Entry(item);
        //        var valueToCompare = entryChild.Entity.GetType().GetProperty("BindName").GetValue(entryChild.Entity);

        //        foreach (var t in inMemoryChildrenCollection)
        //        {
        //            var val = t.GetType().GetProperty("BindName").GetValue(t);
        //            if (val.ToString() == valueToCompare.ToString())
        //            {

        //                entryChild.CurrentValues.SetValues(t);
        //                entryChild.State = EntityState.Modified;
        //            }
        //        }

        //        //item.GetType().GetProperty(entityNav).SetValue(item,entry.Entity);
        //        //item.GetType().GetProperty(entityNav).SetValue(item, entry);
        //    }
        //    dbChildrenCollection.CurrentValue = entity.GetType().GetProperty(naviName).GetValue(entity);
        //    //test.EntityEntry.CurrentValues.SetValues(entity.GetType().GetProperty(naviName).GetValue(entity));

        //    //foreach (var property in test.EntityEntry.Property())
        //    //{

        //    //}
        //    //entry.Property(x => x).IsModified = true;
        //}

        public void Update(T entity, Expression<Func<T, object>> childrenToUpdate = null)
        {
          
            var keyName = FindPkNames().Single();
            var key = typeof(T).GetProperty(keyName).GetValue(entity);
            T attachedEntity = _dbContext.Set<T>().Find(key);
             var entry = _dbContext.Entry<T>(attachedEntity);
            //fill out entry with new values
            entry.CurrentValues.SetValues(entity);
          
       
            //load null navigation entity
            //TODO load only required entity
            foreach (var edmNavigationProperty in entry.GetNavigationProperties(_dbContext))
            {
                var entityNavigationProperty = entry.Entity.GetType().GetProperty(edmNavigationProperty.Name);
                bool isCollection = entityNavigationProperty.PropertyType.FullName.Contains("ICollection");
                if (isCollection)
                {
                    var collection = entry.Collection(edmNavigationProperty.Name);
                    if (!collection.IsLoaded)
                        collection.Load();
                }
                else
                    entry.Reference(edmNavigationProperty.Name).Load();

            }
            //update children entities
            //TODO update only entities with different values
            if (childrenToUpdate != null)
                foreach (MemberExpression argument in ((NewExpression)childrenToUpdate.Body).Arguments)
                {
                    var dbChildrenCollection = entry.Collection(argument.Member.Name);
                    if (!dbChildrenCollection.IsLoaded)
                        dbChildrenCollection.Load();

                    foreach (var item in dbChildrenCollection.Query())
                    {
                        var entryChild = _dbContext.Entry(item);
                        entryChild.State = EntityState.Modified;

                    }

                }


        }
        //public void Update(T entity)
        //{

        //    var keyName = _dbContext.Db<T>().Properties.First(x => x.IsPk).PropertyName;
        //    var key = typeof(T).GetProperty(keyName).GetValue(entity);
        //    T attachedEntity = _dbContext.Set<T>().Find(key);
        //    var entry = _dbContext.Entry<T>(attachedEntity);
        //    entry.CurrentValues.SetValues(entity);

        //    if (entry.State != EntityState.Added)
        //      entry.State = EntityState.Modified; // attach the entity

        //   var dbChildrenCollection = entry.Collection("Columns");
        //   dbChildrenCollection.Load();
        //    var tz = entry.Entity.GetType();
        //    var entityNav = _dbContext.Db(entry.Entity.GetType()).Properties;
        //    var inMemoryChildrenCollection = entity.GetType().GetProperty("Columns").GetValue(entity) as IList;
        //    foreach (var item in dbChildrenCollection.Query())
        //    {
        //        var entryChild = _dbContext.Entry(item);
        //     entryChild.State = EntityState.Modified;

        //    }

        //}

        public void Update(IList<T> entities, Expression<Func<T, object>> childrenToUpdate = null)
        {

            foreach (var entity in entities)
            {
                Update(entity, childrenToUpdate);
            }

        }

        public IQueryable<T> GetQueryable()
        {
            return Data.AsQueryable<T>();
        }

        public long Count()
        {
            return Data.LongCount<T>();
        }

        public long Count(Expression<Func<T, bool>> whereCondition)
        {
            return Data.Where(whereCondition).LongCount<T>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }

        /// <summary>
        /// Release memory of DBContext
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Save All entities in ObjectContext
        /// </summary>
        public void Save()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }
                throw new DbEntityValidationException(sb.ToString(), e);
            }
        }

    }

}

