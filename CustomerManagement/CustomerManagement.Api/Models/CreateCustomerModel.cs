namespace CustomerManagement.Api.Models
{
    /// <summary>
    /// Model used to create a new customer.
    /// </summary>
    public class CreateCustomerModel
    {
        public string Name { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }
        public string Industry { get; set; }
    }
}
