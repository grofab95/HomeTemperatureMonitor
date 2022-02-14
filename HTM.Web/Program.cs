using Grpc.Core;
using HTM.Communication.V1;
using HTM.Infrastructure.Logging;
using HTM.Web.Communication.Services;
using HTM.Web.Data;

SerilogHelper.AddSerilog();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var htmEventsService = new HtmEventsService();
builder.Services.AddSingleton<IHtmEventsService, HtmEventsService>(_ => htmEventsService);
builder.Services.AddSingleton<IHtmEventsInvoker, HtmEventsService>(_ => htmEventsService);

builder.Services.AddSingleton(sp =>
{
    var htmEventsCaller = sp.GetRequiredService<IHtmEventsInvoker>();
    return new Server
    {
        Services = { HTMEventsService.BindService(new HtmEventsServer(htmEventsCaller)) },
        Ports = { new ServerPort("localhost", 2005, ServerCredentials.Insecure) }
    };
});
builder.Services.AddHostedService<GrpcHostedService>();
//builder.Services.AddSingleton<IHostedService, GrpcHostedService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();