using Microsoft.EntityFrameworkCore;


namespace angel1953_backend.Models
{
    public class Db : DbContext
    {
        public Db(DbContextOptions<Db> options)
            : base(options)
        {
        }

        public DbSet<School> School { get; set; }  
    }
}
