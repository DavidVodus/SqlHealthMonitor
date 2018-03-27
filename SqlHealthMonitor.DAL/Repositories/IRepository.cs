using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SqlHealthMonitor.DAL.Repositories
{
    public interface IRepository<T>
    {
        /// <summary> 
        /// Add entity to the repository 
        /// </summary> 
        /// <param name="entity">the entity to add</param> 
        /// <returns>The added entity</returns> 
        void Add(T entity);

        void Attach(T entity);

        /// <summary>
        /// Count using a filer
        /// </summary>
        long Count(Expression<Func<T, bool>> whereCondition);

        /// <summary>
        /// All item count
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        long Count();

        /// <summary> 
        /// Mark entity to be deleted within the repository 
        /// </summary> 
        /// <param name="entity">The entity to delete</param> 
        void Delete(T entity);

        /// <summary> 
        /// Load the entities using a linq expression filter
        /// </summary> 
        /// <typeparam name="T">the entity type to load</typeparam> 
        /// <param name="whereCondition">where condition</param> 
        /// <returns>the loaded entity</returns> 
        IList<T> GetAll(Expression<Func<T, bool>> whereCondition);

        /// <summary>
        /// Get all the element of this repository
        /// </summary>
        /// <returns></returns>
        IList<T> GetAll();

      
        IQueryable<T> GetQueryable();

     
        T GetSingle(Expression<Func<T, bool>> whereCondition);


        /// <summary>
        /// Saves All Entities in ObjectContext
        /// </summary>
        void Save();

        /// <summary>
        /// Updates List of entities and optionaly children entities.
        /// <para />Inside it attaches entities,loads null navigation properties,optionaly sets state to modify on all children entities
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="childrenToUpdate">name of Collection properties ,example:x => new { x.Columns,x.Columns2 }</param>
        void Update(IList<T> entities, Expression<Func<T, object>> childrenToUpdate = null);
        /// <summary>
        /// Updates entity and optionaly children entities.
        /// <para />Inside it attaches entities,loads null navigation properties,optionaly sets state to modify on all children entities
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="childrenToUpdate">name of Collection properties,example:x => new { x.Columns,x.Columns2 }</param>
        void Update(T entity, Expression<Func<T, object>> childrenToUpdate=null);
    }
}
