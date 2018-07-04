using CustomerManagement.Api.Models;
using CustomerManagement.Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.Api.Controllers.Common
{
    public class BaseController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public BaseController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected ObjectResult Error(string errorMessage)
        {
            return new BadRequestObjectResult(Envelope.Error(errorMessage));
        }
        
        protected new ObjectResult Ok()
        {
            _unitOfWork.Commit();
            return new OkObjectResult(Envelope.Ok());
        }

        protected ObjectResult Ok<T>(T result)
        {
            _unitOfWork.Commit();
            return new OkObjectResult(Envelope.Ok(result));
        }
    }
}
