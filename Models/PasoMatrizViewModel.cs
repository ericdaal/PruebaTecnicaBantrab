using System.Text.Json;

namespace Nueva_carpeta.Models
{
    public class PasoMatrizViewModel
    {
        public int[,]? Matriz { get; set; }
        public int PasoActual { get; set; }
        public int Total { get; set; }
        public string? Titulo { get; set; }
        public bool BloquearSiguiente { get; set; } = false;

        // Serializar la matriz para TempData
        public string SerializarMatriz()
        {
            return JsonSerializer.Serialize(Matriz);
        }

        public static int[,] DeserializarMatriz(string json)
        {
            return JsonSerializer.Deserialize<int[,]>(json) ?? new int[0, 0];
        }
    }
}