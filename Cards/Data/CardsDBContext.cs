using Microsoft.EntityFrameworkCore;
using Cards.Models.Domain;

namespace Cards.Data
{
    public class CardsDBContext : DbContext //to inherit from dbcontext class
    {
        public CardsDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        //This property will create collection inside database
        public DbSet<NumericData> NumericData { get; set; }
        public DbSet<CostsData> CostsData { get; set; }

    }
}
