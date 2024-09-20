using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using MyTodos.Services;
using System.Threading.Tasks;

namespace MyTodos.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<TodoItem> items = new ObservableCollection<TodoItem>(); // Inicialización directa

        [ObservableProperty]
        string text;

        public MainViewModel()
        {
            LoadItemsAsync().ConfigureAwait(false); // Cargar elementos al iniciar el ViewModel
        }

        // Método para cargar las tareas desde la base de datos
        public async Task LoadItemsAsync()
        {
            try
            {
                var todoItems = await App.Database.GetItemsAsync();
                if (todoItems != null)
                {
                    Items = new ObservableCollection<TodoItem>(todoItems);
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine($"Error cargando los elementos: {ex.Message}");
            }
        }

        // Comando para añadir una nueva tarea
        [RelayCommand]
        public async Task AddAsync()
        {
            if (string.IsNullOrWhiteSpace(Text))
                return;

            var newItem = new TodoItem { Text = Text, IsCompleted = false };

            try
            {
                await App.Database.SaveItemAsync(newItem);
                Items.Add(newItem); // Añadir el nuevo ítem a la lista observable
                Text = string.Empty; // Limpiar el texto de entrada
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine($"Error al añadir el elemento: {ex.Message}");
            }
        }

        // Comando para eliminar una tarea
        [RelayCommand]
        public async Task DeleteAsync(TodoItem item)
        {
            if (item == null || !Items.Contains(item))
                return;

            try
            {
                await App.Database.DeleteItemAsync(item);
                Items.Remove(item); // Eliminar de la colección observable
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine($"Error al eliminar el elemento: {ex.Message}");
            }
        }

        // Comando para editar una tarea
        [RelayCommand]
        public void EditItem(TodoItem item)
        {
            if (item == null)
                return;

            Text = item.Text; // Asigna el texto a la propiedad de entrada
            Items.Remove(item); // Eliminar temporalmente de la lista para su edición
        }
    }
}

