namespace webapi_learning.Models.Core
{
    public interface IShirtRepository
    {
        Task<bool> Exists(int id);
        Task<Shirt?> GetShirtById(int? id);
        Task<List<Shirt>> GetShirts();
        Task<Shirt?> GetShirtByProperties(string? Model, string? Gender, int? Size);

        Task AddShirt(Shirt shirt);

        Task UpdateShirt(Shirt shirt);

        Task DeleteShirt(int id);

        Task<(List<Shirt>, int)> PagedGetShirts(int pageNumber, int pageSize);
    }
}
