using Microsoft.AspNetCore.Mvc;
using ST10187895_CLDV6212_POE;
using ST10187895_CLDV6212_POE_PART1.Models;
using Azure.Storage.Blobs;
using ST10187895_CLDV6212_POE_PART1.Services;
using System.Diagnostics;
using System.Threading.Tasks;




namespace ST10187895_CLDV6212_POE_PART1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {

        //private readonly BlobService _blobService;
        //private readonly TableService _tableService;
        //private readonly QueueService _queueService;
        //private readonly FileService _fileService;
        private readonly ILogger<HomeController> _logger;

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IConfiguration _configuration;

        public HomeController(IHttpClientFactory httpClientFactory, ILogger<HomeController> logger, IConfiguration configuration) // Inject IConfiguration

        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration; // Set configuration
        }

        private readonly UploadBlob _uploadBlob;
        private readonly StoreTableInfo _storeTableInfo;
        private readonly ProcessQueueMessage _processQueueMessage;
        private readonly UploadFile _uploadFile;

        public HomeController(UploadBlob uploadBlob,StoreTableInfo storeTableInfo, ProcessQueueMessage processQueueMessage, UploadFile uploadFile)
        {
            _uploadBlob = uploadBlob;
            _storeTableInfo = storeTableInfo;
            _processQueueMessage = processQueueMessage;
            _uploadFile = uploadFile;
        }

        //public HomeController(BlobService blobService, TableService tableService, QueueService queueService, FileService fileService)
        //{
        //    _blobService = blobService;
        //    _tableService = tableService;
        //    _queueService = queueService;
        //    _fileService = fileService;


        //}

        //[HttpPost]
        //public async Task<IActionResult> StoreTableInfo([FromBody] CustomerProfile profile)
        //{
        //    try
        //    {
        //        var result = await StoreTableInfo(profile);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file != null)
            {
                try 
                {
                    using var httpClient = _httpClientFactory.CreateClient();
                    using var stream = file.OpenReadStream();
                    var content = new StreamContent(stream);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

                    string url = _configuration["AzureFunctions:UploadBlobUrl"];
                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _logger.LogError($"Error submitting image: {response.ReasonPhrase}");

                        _logger.LogError($"Response content: {await response.Content.ReadAsStringAsync()}");

                    }

                }
                catch(Exception ex)
                {
                    _logger.LogError($"Exception occurred while submitting image: {ex.Message}");
                }
            }

            else
            {
                _logger.LogError("No image file provided");

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> StoreTableInfo(CustomerProfile profile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using var httpClient = _httpClientFactory.CreateClient();

                    // Prepare the request URI with query parameters
                    var requestUri = $"https://st10187895cldv6212poe.azurewebsites.net/api/StoreTableInfo?code=cA8EUgobet_OAXAQY_68Iw5tH1sQIR0VeodIg0F0V_kUAzFuiWaYJA%3D%3D";

                    // Send an HTTP POST request to your Azure Function
                    var response = await httpClient.PostAsync(requestUri, null);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _logger.LogError($"Error submitting client info: {response.ReasonPhrase}");
                        _logger.LogError($"Response content: {await response.Content.ReadAsStringAsync()}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception occurred while submitting client info: {ex.Message}");
                }
            }

            return View("Index", profile);
        }
        [HttpPost]

        public async Task<IActionResult> UploadFile(IFormFile file)

        {

            if (file != null)

            {

                try

                {

                    using var httpClient = _httpClientFactory.CreateClient();
                    using var stream = file.OpenReadStream();
                    var content = new StreamContent(stream);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

                    string url = _configuration["AzureFunctions:UploadFileUrl"];
                    url += $"&fileName={file.FileName}";

                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }

                    else
                    {
                        _logger.LogError($"Error uploading contract: {response.ReasonPhrase}");
                        _logger.LogError($"Response content: {await response.Content.ReadAsStringAsync()}");
                    }

                }

                catch (Exception ex)

                {
                    _logger.LogError($"Exception occurred while uploading contract: {ex.Message}");
                }
            }
            else
            {
                _logger.LogError("No contract file provided");
            }

            return View("Index");

        }

        [HttpPost]

        public async Task<IActionResult> ProcessQueueMessage(string orderId)
        {
            if (!string.IsNullOrEmpty(orderId))
            {
                try
                {
                    using var httpClient = _httpClientFactory.CreateClient();

                    string url = _configuration["AzureFunctions:ProcessQueueMessageUrl"];

                    url += $"&message=Processing order {orderId}";

                    var response = await httpClient.PostAsync(url, null);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _logger.LogError($"Error processing order: {response.ReasonPhrase}");
                        _logger.LogError($"Response content: {await response.Content.ReadAsStringAsync()}");
                    }
                }

                catch (Exception ex)
                {
                    _logger.LogError($"Exception occurred while processing order {orderId}: {ex.Message}");
                }
            }
            else
            {
                _logger.LogError("Order ID is null or empty");
            }
            return View("Index");
        }

            //[HttpPost]
            //public async Task<IActionResult> UploadImage(IFormFile file)
            //{
            //    if (file != null)
            //    {
            //        using var stream = file.OpenReadStream();
            //        await _blobService.UploadBlobAsync("product-images", file.FileName, stream);
            //    }
            //    return RedirectToAction("Index");
            //}


            //[HttpPost]
            //public async Task<IActionResult> AddCustomerProfile(CustomerProfile profile)
            //{
            //    if (ModelState.IsValid)
            //    {
            //        await _tableService.AddEntityAsync(profile);
            //    }
            //    return RedirectToAction("Index");
            //}


            //[HttpPost]


            //public async Task<IActionResult> ProcessOrder(string orderId)
            //{
            //    await _queueService.SendMessageAsync("order-processing", $"Processing order {orderId}");
            //    return RedirectToAction("Index");
            //}
            //[HttpPost]
            //public async Task<IActionResult> UploadContract(IFormFile file)
            //{
            //    if (file != null)
            //    {
            //        using var stream = file.OpenReadStream();
            //        await _fileService.UploadFileAsync("contracts-logs", file.FileName, stream);
            //    }
            //    return RedirectToAction("Index");
            //}

            public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AddCustomerProfile()
        {
            return View();
        }

        public IActionResult UploadProductInfo()
        {
            return View();
        }

        public IActionResult ProcessOrder()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
