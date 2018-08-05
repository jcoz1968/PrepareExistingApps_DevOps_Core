using System;
using System.Collections.Generic;
using System.Windows;
using TodoApplication.TodoServiceReference;
using TodoApplication.WpfLogging;

namespace TodoApplication
{
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtUserNm.Text = Environment.UserName;
            txtMachineNm.Text = Environment.MachineName;            
        }

        private void btnAlert_Click(object sender, RoutedEventArgs e)
        {
            WpfLogger.LogUsage("Button clicked",
                new Dictionary<string, object> { { "moreStuff", "sample text" } });

            MessageBox.Show("Nicely done!");

            throw new Exception("Something really bad happened!!");
        }

        private void btnGet_Click(object sender, RoutedEventArgs e)
        {
            var tracker = new PerfTracker("getting all todos");

            var ItemList = new List<ToDoItem>();
            using (var svc = new ServiceClient())
                ItemList = svc.GetToDoList();
            LbTodoItems.ItemsSource = ItemList;

            tracker.Stop();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = LbTodoItems.SelectedItem as ToDoItem ??
                new ToDoItem
                {
                    Completed = chkCompleted.IsChecked.Value,
                    Id = 2,
                    Item = txtItemNote.Text
                };

            selectedItem.Item = txtItemNote.Text;
            selectedItem.Completed = chkCompleted.IsChecked.Value;

            WpfLogger.LogDiagnostic("Updating todo...", selectedItem);

            using (var svc = new ServiceClient())
                svc.UpdateItem(selectedItem);
        }
    }
}