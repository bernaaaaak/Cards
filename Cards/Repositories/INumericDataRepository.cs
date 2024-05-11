using Cards.Models.Domain;
namespace Cards.Repositories
{
    public interface INumericDataRepository
    {
        //All methods/operations that are being operated on a database by using DBCONTEXT needs to be through a repository.
        Task<List<NumericData>> GetAllAsync();

        Task<NumericData?> GetByIdAsync(int id);

        Task<NumericData> CreateAsync(NumericData numericData);

        Task<NumericData?> UpdateAsync(int id, NumericData numericData);

        Task<NumericData?> DeleteAsync(int id);



    }
}
