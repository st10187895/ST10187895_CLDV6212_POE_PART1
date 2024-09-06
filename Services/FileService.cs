using Azure.Storage.Files.Shares;

namespace ST10187895_CLDV6212_POE_PART1.Services
{
    public class FileService
    {
        private readonly ShareServiceClient _shareServiceClient;

        public FileService(IConfiguration configuration)
        {
            _shareServiceClient = new ShareServiceClient(configuration["AzureStorage:ConnectionString"]);
        }
        public async Task UploadFileAsync(string shareName, string fileName, Stream content)
        {
            var shareClient = _shareServiceClient.GetShareClient(shareName);
            await shareClient.CreateIfNotExistsAsync();
            var directoryClient = shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(fileName);
            await fileClient.CreateAsync(content.Length);
            await fileClient.UploadAsync(content);
        }
    }
}
