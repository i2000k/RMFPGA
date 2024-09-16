using Microsoft.Extensions.Hosting;
using StandHostService.ApplicationServices;
using StandHostService.Services;
using StandHostService.WebSockets;
using WebSocketSharp.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//builder.Services.AddWindowsService();
//builder.Services.AddHostedService<ServiceA>();
builder.Services.AddSingleton<SerialPortService>();
builder.Services.AddScoped<ProcessFileService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

var wssv = new WebSocketServer(4680);

app.Lifetime.ApplicationStarted.Register(() =>
{
    app.Logger.LogInformation("Creating websocket...");
    wssv.AddWebSocketService<Camera>("/");
    app.Logger.LogInformation($"Starting webSocket on port: {wssv.Port}...");
    wssv.Start();
});

app.Lifetime.ApplicationStopping.Register(() =>
{
    app.Logger.LogInformation("Stopping webSocket...");
    wssv.Stop();
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseSwagger();
app.UseSwaggerUI();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();




