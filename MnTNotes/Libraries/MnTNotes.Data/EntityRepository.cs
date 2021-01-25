using Microsoft.EntityFrameworkCore;
using MnTNotes.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MnTNotes.Data
{
    public class EntityRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : BaseEntity, IEntity, new()
        where TContext : DbContext, new()

    {
        private readonly TContext _dbcontext;

        public EntityRepository(TContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        #region Methods

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition</param>
        /// <returns>Entity</returns>
        public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbcontext.Set<TEntity>().SingleOrDefault(predicate);
        }

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition</param>
        /// <returns>Entities List</returns>
        public List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null
                ? _dbcontext.Set<TEntity>().ToList()
                : _dbcontext.Set<TEntity>().Where(predicate).ToList();
        }

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var addedEntity = _dbcontext.Entry(entity);
            addedEntity.State = EntityState.Added;
            _dbcontext.SaveChanges();
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _dbcontext.AddRange(entities);
            _dbcontext.SaveChanges();
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var updatedEntity = _dbcontext.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            _dbcontext.SaveChanges();
        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Update(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var deletedEntity = _dbcontext.Entry(entity);
            deletedEntity.State = EntityState.Deleted;
            _dbcontext.SaveChanges();
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition</param>
        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            _dbcontext.Remove(predicate);
            _dbcontext.SaveChanges();
        }

        #endregion Methods
    }
}