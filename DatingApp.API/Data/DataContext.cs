using DatingApp.API.models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options)
        {
                        
        }
        public DbSet<ValueModel> Values{get; set;}
        public DbSet<User> Users{get; set;}
    }
}