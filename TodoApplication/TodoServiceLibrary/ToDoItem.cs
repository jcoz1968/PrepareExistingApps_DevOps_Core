namespace TodoServiceLibrary
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Item { get; set; }
        public bool Completed { get; set; }
        public override string ToString()
        {
            return $"Id:{Id} -- Item:{Item} -- Completed:{Completed}";
        }
    }
}