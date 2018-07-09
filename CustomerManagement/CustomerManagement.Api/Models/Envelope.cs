using System;

namespace CustomerManagement.Api.Models
{
    /// <summary>
    /// Base response model for HTTP requests.
    /// </summary>
    /// <typeparam name="T">Result object</typeparam>
    public class Envelope<T>
    {
        public T Result { get; }
        public string ErrorMessage { get; }
        public DateTime TimeGenerated { get; }

        protected internal Envelope(T result, string errorMessage)
        {
            Result = result;
            ErrorMessage = errorMessage;
            TimeGenerated = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Default response model for all HTTP requests.
    /// </summary>
    public class Envelope : Envelope<string>
    {
        protected Envelope(string errorMessage) : base(string.Empty, errorMessage)
        {
        }

        /// <summary>
        /// Returns a HTTP response model with result object.
        /// </summary>
        /// <typeparam name="T">Result object</typeparam>
        /// <param name="result">Result instance</param>
        /// <returns>HTTP response model</returns>
        public static Envelope<T> Ok<T>(T result)
        {
            return new Envelope<T>(result, string.Empty);
        }

        /// <summary>
        /// Returns a HTTP response model with empty result object.
        /// </summary>
        /// <returns>HTTP response model</returns>
        public static Envelope Ok()
        {
            return new Envelope(string.Empty);
        }

        /// <summary>
        /// Returns a HTTP response model with an error message.
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        /// <returns>HTTP response model</returns>
        public static Envelope Error(string errorMessage)
        {
            return new Envelope(errorMessage);
        }
    }
}
