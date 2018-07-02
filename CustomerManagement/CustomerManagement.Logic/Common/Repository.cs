using CSharpFunctionalExtensions;
using CustomerManagement.Logic.Utils;

namespace CustomerManagement.Logic.Common
{
    /// <summary>
    /// Represents a database repository.
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public class Repository<T> where T : Entity
    {
        protected readonly UnitOfWork _unitOfWork;

        protected Repository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets an entity by its ID.
        /// </summary>
        /// <param name="id">Entity ID attribute</param>
        /// <returns>Entity associated by the ID or a null entity.</returns>
        public Maybe<T> GetById(long id)
        {
            return _unitOfWork.Get<T>(id);
        }

        /// <summary>
        /// Saves or update an entity.
        /// </summary>
        /// <param name="entity">Entity class</param>
        public void Save(T entity)
        {
            _unitOfWork.SaveOrUpdate(entity);
        }
    }
}
