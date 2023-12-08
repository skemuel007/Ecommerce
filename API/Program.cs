
using API.Extensions;
using Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);


var app = builder.Build();
await app.ConfigureWebApplicationServices(builder.Configuration);

