using GSM.Services;
using GSM.Data;
using GSM;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Setup.ConfigureServices(builder.Services);
//using var scope = builder.Services.BuildServiceProvider().CreateScope();
//Setup.ConfigureServices2(scope.ServiceProvider);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHostedService, DatabaseUpdater>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();


app.Run();
