using Microsoft.EntityFrameworkCore;
using webapi_learning.Data;
using webapi_learning.Helpers;
using webapi_learning.Helpers.Core;
using webapi_learning.Middlewares;
using webapi_learning.Models.Core;
using webapi_learning.Models.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<IShirtRepository, ShirtRepository>();
// add db context to the di container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShirtStoreManagements"))
);
// registering custom generic logger
builder.Services.AddSingleton(typeof(ICustomLogger<>), typeof(CustomLogger<>));

var app = builder.Build();

app.MapControllers();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.Run();
