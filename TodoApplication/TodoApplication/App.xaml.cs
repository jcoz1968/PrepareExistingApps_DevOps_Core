using System;
using System.Windows;
using System.Windows.Threading;
using TodoApplication.WpfLogging;

namespace TodoApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        const string TechErrorMsg = 
            "A technical error has occurred. Please try your process again, and " +
            "contact technical support if the problem persists.";

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                // set up a global exception handler for all unhandled exceptions
                Current.DispatcherUnhandledException += GlobalExceptionHandler; 
                AppDomain.CurrentDomain.UnhandledException += AppDomainExceptionHandler;

                base.OnStartup(e);                
            }
            catch (Exception ex)
            {
                WpfLogger.LogError("Something failed starting up the application!", ex);
                MessageBox.Show("Something failed starting up the application!", 
                    "Todos Application",MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Environment.Exit(9);
            }
        }

        private void AppDomainExceptionHandler(object sender, 
            UnhandledExceptionEventArgs e)
        {
            HandleUnhandledException(e.ExceptionObject as Exception);
        }

        static void GlobalExceptionHandler(object sender, 
            DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            HandleUnhandledException(e.Exception);
        }

        private static void HandleUnhandledException(Exception exception)
        {
            WpfLogger.LogError(exception.GetBaseException().Message, exception);
            MessageBox.Show(TechErrorMsg, "Error", MessageBoxButton.OK, 
                MessageBoxImage.Error);
        }
    }
}
