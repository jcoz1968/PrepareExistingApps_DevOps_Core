using System.Collections.Generic;
using System.ServiceModel;

namespace TodoServiceLibrary
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        List<ToDoItem> GetToDoList();

        [OperationContract]
        void UpdateItem(ToDoItem todo);
    }
}