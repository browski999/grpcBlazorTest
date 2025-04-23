using Grpc.Net.Client;
using grpcBlazorTest.Components;
using GrpcConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddGrpcClient<Greeter.GreeterClient>(options =>
{
    options.Address = new Uri("https://afd-bob-endpoint-fuepb2fcfzgvc2c6.z03.azurefd.net");
    //options.Address = new Uri("https://con-bob-acr.braveglacier-2d105bbd.uksouth.azurecontainerapps.io");
}).ConfigureChannel((ServiceProvider, options) =>
{
    options.HttpHandler = new SocketsHttpHandler
    {
        EnableMultipleHttp2Connections = true,
        PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
        KeepAlivePingDelay = TimeSpan.FromSeconds(30),
        KeepAlivePingTimeout = TimeSpan.FromSeconds(30)
    };
    options.ServiceProvider = ServiceProvider;
});

builder.Services.AddServerSideBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
//app.MapBlazorHub();
//app.MapFallbackToPage("/_Host");

app.Run();
