using System.Data;
using System.Data.SqlClient;
using CustomerManagement.Logic.Utils;
using Microsoft.Extensions.Configuration;

namespace CustomerManagement.Tests.Integration
{
    public abstract class TestBase
    {
        protected TestBase()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json").Build();

            var conn = config.GetConnectionString("CustomerManagementDatabase");
            
            ClearDatabase(conn);
            Initer.Init(conn);
        }

        private void ClearDatabase(string connectionString)
        {
            string query = 
                "DELETE FROM dbo.Customer;" + 
                "UPDATE dbo.Ids SET NextHigh = 0";

            using (var cnn = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand(query, cnn)
                {
                    CommandType = CommandType.Text
                };

                cnn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
