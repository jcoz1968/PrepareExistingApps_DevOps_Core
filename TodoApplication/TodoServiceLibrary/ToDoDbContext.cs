using Flogging.Data.Entity;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;

namespace TodoServiceLibrary
{
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(string connectionString): base(connectionString)
        {
            DbInterception.Add(new FloggerEFInterceptor());
        }
        public DbSet<ToDoItem> ToDoItems { get; set; }
    }
}