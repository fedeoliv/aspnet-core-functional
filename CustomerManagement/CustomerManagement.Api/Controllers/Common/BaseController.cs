using CustomerManagement.Api.Models;
using CustomerManagement.Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.Api.Controllers.Common
{
    // A custom base class for controllers to standardize HTTP responses
    public class BaseController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public BaseController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Returns a Bad Request response (HTTP 400) with an error message.
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        /// <returns>HTTP 400 (Bad Request) response.</returns>
        protected ObjectResult Error(string errorMessage)
        {
            return new BadRequestObjectResult(Envelope.Error(errorMessage));
        }

        /// <summary>
        /// Returns an Ok response (HTTP 200).
        /// </summary>
        /// <returns>HTTP 200 (Ok) response.</returns>
        protected new ObjectResult Ok()
        {
            _unitOfWork.Commit();
            return new OkObjectResult(Envelope.Ok());
        }

        /// <summary>
        /// Returns an Ok response (HTTP 200) with an object in JSON format.
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="result">Generic object instance</param>
        /// <returns>HTTP 200 (Ok) response.</returns>
        protected ObjectResult Ok<T>(T result)
        {
            _unitOfWork.Commit();
            return new OkObjectResult(Envelope.Ok(result));
        }
    }
}
