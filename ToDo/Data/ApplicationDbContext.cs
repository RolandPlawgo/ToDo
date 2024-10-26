using Microsoft.EntityFrameworkCore;
using ToDo.Models;

namespace ToDo.Data
{
    public class ApplicationDbContext : DbContext
    {
        //private string ConnectionString
        //{
        //    get
        //    {
        //        return $"Server={System.Environment.MachineName};Port=5432;Database=ToDo;Integrated Security=true;";
        //    }
        //}
        //private const string connectionString = "Server=.;Port=5432;Database=ToDo;Integrated Security=true;";
        public DbSet<ToDoModel> ToDos { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql(connectionString);
        //}
    }
}
