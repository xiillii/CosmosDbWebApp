using System;
using CosmosDbWebApp.Models;

namespace CosmosDbWebApp.Services;

public interface ICosmosDbService
{
    Task<IEnumerable<Item>> GetItemsAsync(string queryString);
    Task<Item> GetItemAsync(string id);
    Task AddItemAsync(Item item);
    Task UpdateItemAsync(string id, Item item);
    Task DeleteItemAsync(string id);
}

