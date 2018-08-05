using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using TodoServiceLibrary.WcfLogging;

namespace TodoServiceLibrary
{
    [FloggingErrorHandler]
    public class Service : IService
    {
        string _connStr;
        public Service()
        {
            _connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"]
                .ConnectionString;
        }
        public List<ToDoItem> GetToDoList()
        {
            using (var db = new ToDoDbContext(_connStr))
                return db.ToDoItems.ToList();
        }
        public void UpdateItem(ToDoItem todo)
        {
            using (var db = new ToDoDbContext(_connStr))
            {
                var todoItem = db.ToDoItems.First(p => p.Id == todo.Id);
                todoItem.Completed = todo.Completed;
                todoItem.Item = todo.Item;
                db.SaveChanges();
            }
        }
    }
}