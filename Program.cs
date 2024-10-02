using ST10187895_CLDV6212_POE;
using ST10187895_CLDV6212_POE_PART1.Services;
namespace ST10187895_CLDV6212_POE_PART1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient();

            //builder.Services.AddSingleton<BlobService>();
            //builder.Services.AddSingleton<TableService>();
            //builder.Services.AddSingleton<QueueService>();
            //builder.Services.AddSingleton<FileService>();

            builder.Services.AddSingleton<StoreTableInfo>();
            builder.Services.AddSingleton<UploadBlob>();
            builder.Services.AddSingleton<UploadFile>();
            builder.Services.AddSingleton<ProcessQueueMessage>();

            var app = builder.Build();

            

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddHttpClient();
        //}
    }
}
