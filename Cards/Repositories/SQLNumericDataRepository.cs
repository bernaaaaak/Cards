

using Cards.Data;
using Cards.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Cards.Repositories
{
    public class SQLNumericDataRepository : INumericDataRepository
    {
        private readonly CardsDBContext dbContext;

        public SQLNumericDataRepository(CardsDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<NumericData?> CreateAsync(NumericData numericData)
        {
            await dbContext.NumericData.AddAsync(numericData);
            await dbContext.SaveChangesAsync();
            return numericData;
        }

        public async Task<NumericData> DeleteAsync(int id)
        {
            return await dbContext.NumericData.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<NumericData>> GetAllAsync()
        {
            return await dbContext.NumericData.ToListAsync();
        }

        public async Task<NumericData?> GetByIdAsync(int id)
        {
            return await dbContext.NumericData.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<NumericData?> UpdateAsync(int id, NumericData numericData)
        {
            var existingNumericData = await dbContext.NumericData.FirstOrDefaultAsync(x =>x.Id == id);
            if(existingNumericData == null)
            {
                return null;
            }

            existingNumericData.Value = numericData.Value;
            existingNumericData.Name = numericData.Name;

            await dbContext.SaveChangesAsync();
            return existingNumericData;
        }
    }
}
