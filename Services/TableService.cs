using Azure.Data.Tables;
using ST10187895_CLDV6212_POE_PART1.Models;

namespace ST10187895_CLDV6212_POE_PART1.Services
{
    public class TableService
    {
        private readonly TableClient _tableClient;

        public TableService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureStorage:ConnectionString"];
            var serviceClient = new TableServiceClient(connectionString);
            _tableClient = serviceClient.GetTableClient("CustomerProfiles");
            _tableClient.CreateIfNotExists();
        }
        public async Task AddEntityAsync(CustomerProfile profile)
        {
            await _tableClient.AddEntityAsync(profile);
        }
    }
}
