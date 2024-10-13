using Microsoft.EntityFrameworkCore;
using Social_App.API.Models.Identity;

namespace Social_App.API.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
