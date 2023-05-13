using SampleForTahaBazar.Entities;
using SampleForTahaBazar.Entities.Models;

namespace SampleForTahaBazar.Services
{
    public interface IUnitOfMeasurementService
    {
        IEnumerable<UnitOfMeasurement> GetUoMs();
        Task<int> AddUoM(UnitOfMeasurement unitOfMeasurement);
    }
    public class UnitOfMeasurementService : IUnitOfMeasurementService
    {
        private readonly SaleDbContext _dbContext;

        public UnitOfMeasurementService(SaleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddUoM(UnitOfMeasurement unitOfMeasurement)
        {
            _dbContext.Add(unitOfMeasurement);
            return await _dbContext.SaveChangesAsync();

        }

        public IEnumerable<UnitOfMeasurement> GetUoMs()
        {
            return _dbContext.UnitOfMeasurements.ToList();
        }
    }
}