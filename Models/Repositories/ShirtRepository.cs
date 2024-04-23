using Microsoft.EntityFrameworkCore;
using webapi_learning.Data;
using webapi_learning.Models.Core;

namespace webapi_learning.Models.Repositories
{
    public class ShirtRepository: IShirtRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ShirtRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddShirt(Shirt shirt)
        {
            _dbContext.Shirts.Add(shirt);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteShirt(int id)
        {
            var shirt = _dbContext.Shirts.FirstOrDefault(_ =>  _.Id == id);
            if (shirt != null)
            {
                _dbContext.Shirts.Remove(shirt);
            }

           await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            var exists = await _dbContext.Shirts.AnyAsync(_ => _.Id == id);

            return exists;
        }

        public async Task<Shirt?> GetShirtById(int? id)
        {
            return await _dbContext.Shirts.FirstOrDefaultAsync(shirt => shirt.Id == id);
        }

        public async Task<Shirt?> GetShirtByProperties(string? Model, string? Gender, int? Size)
        {
            return await _dbContext.Shirts
                .Where(s => !string.IsNullOrWhiteSpace(Model) &&
                            !string.IsNullOrWhiteSpace(s.Model) &&
                            s.Model.ToUpper() == Model.ToUpper() &&
                            !string.IsNullOrWhiteSpace(Gender) &&
                            !string.IsNullOrWhiteSpace(s.Gender) &&
                            s.Gender.ToUpper() == Gender.ToUpper() &&
                            Size.HasValue && s.Size.HasValue &&
                            Size.Value == s.Size.Value)
                .FirstOrDefaultAsync();

        }

        public async Task<List<Shirt>> GetShirts()
        {
            return await _dbContext.Shirts.ToListAsync();
        }

        public async Task<(List<Shirt>, int)> PagedGetShirts(int pageNumber, int pageSize)
        {
            int totalCount = await _dbContext.Shirts.CountAsync();
            var shirts = await _dbContext.Shirts.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(s => s.Id) 
                .ToListAsync();

            return(shirts, totalCount);
        }

        public async Task UpdateShirt(Shirt shirt)
        {
            var dbShirt = await _dbContext.Shirts.FirstAsync(_ => _.Id == shirt.Id);

            dbShirt.Model = shirt.Model;
            dbShirt.Gender = shirt.Gender;
            dbShirt.Size = shirt.Size;

            await _dbContext.SaveChangesAsync();
        }
    }
}
