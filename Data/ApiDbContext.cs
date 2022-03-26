using Microsoft.EntityFrameworkCore;
using SampleWebapi.Models;

namespace SampleWebapi.Data
{
    public class ApiDbContext : DbContext
    {
        public virtual DbSet<ItemData> Items {get;set;}

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
            
        }
    }
}