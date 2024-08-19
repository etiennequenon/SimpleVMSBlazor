using SimpleVMSBlazor.Components;
using SimpleVMSBlazor.Data;
using SimpleVMSBlazor.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

var options = new WebApplicationOptions
{
    ContentRootPath = AppContext.BaseDirectory
};

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSqlite<CameraContext>("Data Source=Cameras.db");
builder.Services.AddScoped<CameraService>();
builder.Services.AddSingleton<CameraState>();
builder.Services.AddDirectoryBrowser();



builder.Services.AddHostedService<VideoRecordingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = new FileExtensionContentTypeProvider
    {
        Mappings =
        {
            [".m3u8"] = "application/vnd.apple.mpegurl",
            [".ts"] = "video/mp2t"
        }
    }
});
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
