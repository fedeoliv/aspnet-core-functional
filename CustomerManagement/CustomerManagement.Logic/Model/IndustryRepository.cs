using System.Linq;
using CSharpFunctionalExtensions;
using CustomerManagement.Logic.Common;
using CustomerManagement.Logic.Utils;

namespace CustomerManagement.Logic.Model
{
    public class IndustryRepository : Repository<Industry>
    {
        public IndustryRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public Maybe<Industry> GetByName(string name)
        {
            return _unitOfWork.Query<Industry>()
                .SingleOrDefault(x => x.Name == name);
        }
    }
}
