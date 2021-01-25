using MnTNotes.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MnTNotes.Data
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        #region Methods

        /// <summary>
        /// Get entity by predicate
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition</param>
        /// <returns>Entity</returns>
        TEntity Get(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Get entities by predicate
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition</param>
        /// <returns>Entities</returns>
        List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Insert(TEntity entity);

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Insert(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(TEntity entity);

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Update(IEnumerable<TEntity> entities);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Delete(IEnumerable<TEntity> entities);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition</param>
        void Delete(Expression<Func<TEntity, bool>> predicate);

        #endregion Methods
    }
}