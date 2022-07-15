using LEN.Services.Identidade.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityConfiguration(builder.Configuration);

builder.Services.AddApiConfiguration();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddServices();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseApiConfiguration();
app.UseAuthConfiguration();

app.Run();
