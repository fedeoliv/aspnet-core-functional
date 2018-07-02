using System;
using System.Data;
using System.Linq;
using CSharpFunctionalExtensions;
using NHibernate;

namespace CustomerManagement.Logic.Utils
{
    /// <summary>
    /// Represents a unit of work that performs CRUD transactions.
    /// </summary>
    public sealed class UnitOfWork : IDisposable
    {
        private readonly ISession _session;
        private readonly ITransaction _transaction;
        private bool _isAlive = true;
        private bool _isCommitted;

        public UnitOfWork()
        {
            _session = SessionFactory.OpenSession();
            _transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// Releases unmanaged resources of a transaction.
        /// </summary>
        public void Dispose()
        {
            if (!_isAlive)
            {
                return;
            }

            _isAlive = false;

            try
            {
                if (_isCommitted)
                {
                    _transaction.Commit();
                }
            }
            finally
            {
                _transaction.Dispose();
                _session.Dispose();
            }
        }

        /// <summary>
        /// Marks the unit of work as ready for dispose.
        /// </summary>
        public void Commit()
        {
            if (!_isAlive)
            {
                return;
            }

            _isCommitted = true;
        }

        /// <summary>
        /// Gets an entity by its ID on the database.
        /// </summary>
        /// <typeparam name="T">Entity class</typeparam>
        /// <param name="id">Entity ID</param>
        /// <returns>Entity associated by the ID or a null entity.</returns>
        internal Maybe<T> Get<T>(long id) where T : class
        {
            return _session.Get<T>(id);
        }

        /// <summary>
        /// Creates or updates an entity on the database.
        /// </summary>
        /// <typeparam name="T">Entity class</typeparam>
        /// <param name="entity">Entity instance</param>
        internal void SaveOrUpdate<T>(T entity)
        {
            _session.SaveOrUpdate(entity);
        }

        /// <summary>
        /// Deletes an entity on the database.
        /// </summary>
        /// <typeparam name="T">Entity class</typeparam>
        /// <param name="entity">Entity instance</param>
        internal void Delete<T>(T entity)
        {
            _session.Delete(entity);
        }

        /// <summary>
        /// Creates a new Linq for the entity class.
        /// </summary>
        /// <typeparam name="T">Entity class</typeparam>
        /// <returns>Returns a new Linq for the entity class</returns>
        internal IQueryable<T> Query<T>()
        {
            return _session.Query<T>();
        }
    }
}
