namespace CustomerManagement.Api.Models
{
    /// <summary>
    /// Model used to create update a customer.
    /// </summary>
    public class UpdateCustomerModel
    {
        public long Id { get; set; }
        public string Industry { get; set; }
    }
}
