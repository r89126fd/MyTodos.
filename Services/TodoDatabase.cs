using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTodos.Services
{
    public class TodoDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public TodoDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            CreateTableAsync().ConfigureAwait(false); // Asegurar que se cree la tabla al inicializar
        }

        // Método para crear la tabla de forma asíncrona
        private async Task CreateTableAsync()
        {
            try
            {
                await _database.CreateTableAsync<TodoItem>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear la tabla: {ex.Message}");
            }
        }

        // Obtener todos los elementos de la tabla TodoItem
        public Task<List<TodoItem>> GetItemsAsync()
        {
            return _database.Table<TodoItem>().ToListAsync();
        }

        // Guardar o actualizar un elemento
        public Task<int> SaveItemAsync(TodoItem item)
        {
            if (item.Id != 0)
            {
                return _database.UpdateAsync(item);  // Actualizar si ya tiene Id
            }
            else
            {
                return _database.InsertAsync(item);  // Insertar si es un nuevo elemento
            }
        }

        // Eliminar un elemento
        public Task<int> DeleteItemAsync(TodoItem item)
        {
            return _database.DeleteAsync(item);
        }
    }

    // Clase TodoItem que representa la estructura de la tabla
    public class TodoItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCompleted { get; set; }
    }
}

