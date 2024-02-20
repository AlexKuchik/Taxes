using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Taxes;
using Taxes.Application;
using Taxes.Infrastructure;
using Taxes.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDatabaseMigration();
}

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();