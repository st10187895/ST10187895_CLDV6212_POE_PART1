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

        private readonly HttpClient _httpClient;

        public TableService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> AddEntityAsync()
        {
            var functionUrl = "https://st10187895cldv6212poe.azurewebsites.net/api/StoreTableInfo?code=cA8EUgobet_OAXAQY_68Iw5tH1sQIR0VeodIg0F0V_kUAzFuiWaYJA%3D%3D";
            HttpResponseMessage response = await _httpClient.GetAsync(functionUrl);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            throw new Exception("Failed to call Azure function.");
        }
    }
}
