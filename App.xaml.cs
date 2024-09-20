using MyTodos.Services;
using System.IO;
using System;

namespace MyTodos
{
    public partial class App : Application
    {
        static TodoDatabase database;

        // Propiedad estática para acceder a la base de datos
        public static TodoDatabase Database
        {
            get
            {
                if (database == null)
                {
                    // Ruta de la base de datos, utilizando la carpeta local de la aplicación
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyTodos.db3");
                    database = new TodoDatabase(dbPath);
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            // Establecer la página principal con la vista principal y su ViewModel
            MainPage = new MainPage(new ViewModel.MainViewModel());
        }
    }
}
