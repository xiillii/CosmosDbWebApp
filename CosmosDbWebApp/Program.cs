

using CosmosDbWebApp.Services;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<ICosmosDbService>(InitializeCosmosClientInstanceAsync(builder.Configuration.GetSection("CosmosToDoList")).GetAwaiter().GetResult());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Item}/{action=Index}/{id?}");

app.Run();


async Task<CosmosDbService> InitializeCosmosClientInstanceAsync(IConfigurationSection configurationSection)
{
    var databaseName = configurationSection.GetSection("DatabaseName").Value;
    var containerName = configurationSection.GetSection("ContainerName").Value;
    var account = configurationSection.GetSection("Account").Value;
    var key = configurationSection.GetSection("Key").Value;

    var client = new CosmosClient(account, key);
    var cosmosDbService = new CosmosDbService(client, databaseName, containerName);
    var database = await client.CreateDatabaseIfNotExistsAsync(databaseName);

    await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

    return cosmosDbService;
}