using BankManagement.Api.Endpoints.AccountEndpoints;
using BankManagement.Api.Endpoints.TransactionEndpoints;
using BankManagement.Application;
using BankManagement.Domain;
using BankManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDomain();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapAccountEndpoints();
app.MapTransferEndpoints();

app.Run();