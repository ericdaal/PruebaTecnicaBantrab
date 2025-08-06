using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nueva_carpeta.Models;

namespace Nueva_carpeta.Controllers
{
    public class MatrizController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            // Inicializa matriz 10x3 y paso 1
            var matriz = new int[10, 3];
            for (int i = 0; i < 10; i++)
            {
                matriz[i, 0] = i + 1;
                matriz[i, 1] = (i + 1) * 2;
            }

            var viewModel = new PasoMatrizViewModel
            {
                Matriz = matriz,
                PasoActual = 1,
                Total = 0,
                Titulo = "Paso 1: Inicializar matriz 10x3"
            };

            GuardarEnTempData(viewModel);
            return View("Paso", viewModel);
        }

        [HttpPost]
        public IActionResult SiguientePaso()
        {
            var viewModel = ObtenerDesdeTempData();
            if (viewModel.Matriz != null)
            {
                switch (viewModel.PasoActual)
                {
                    case 1:
                        // Llenar columna 3 con suma de columnas 1 y 2
                        for (int i = 0; i < viewModel.Matriz.GetLength(0); i++)
                            viewModel.Matriz[i, 2] = viewModel.Matriz[i, 0] + viewModel.Matriz[i, 1];

                        viewModel.Titulo = "Paso 2: Llenar columna 3 con suma de 1 y 2";
                        break;

                    case 2:
                        // Calcular total de columna 3
                        int total = 0;
                        for (int i = 0; i < viewModel.Matriz.GetLength(0); i++)
                            total += viewModel.Matriz[i, 2];
                        viewModel.Titulo = $"Paso 3: Total de columna 3= {total}";
                        break;
                    case 3:
                        // Redimensionar matriz a 12x4
                        int[,] nuevaMatriz = new int[12, 4];

                        for (int i = 0; i < viewModel.Matriz.GetLength(0); i++) // 10 filas
                        {
                            for (int j = 0; j < viewModel.Matriz.GetLength(1); j++) // 3 columnas
                            {
                                nuevaMatriz[i, j] = viewModel.Matriz[i, j];
                            }
                        }
                        viewModel.Matriz = nuevaMatriz;
                        viewModel.Titulo = "Paso 4: Redimensionar matriz a 12x4";
                        break;
                    case 4:
                        // Trasladar columna 1 de filas 1 y 2 a filas 11 y 12
                        viewModel.Matriz[10, 0] = viewModel.Matriz[0, 0]; // fila 11 ← fila 1
                        viewModel.Matriz[11, 0] = viewModel.Matriz[1, 0]; // fila 12 ← fila 2

                        viewModel.Titulo = "Paso 5: Trasladar columna 1 de filas 1 y 2 a filas 11 y 12";
                        break;
                    case 5:
                        // Calcular total de columna 2 en filas 1 y 2 (índices 0 y 1)
                        int totalColumna2 = viewModel.Matriz[0, 1] + viewModel.Matriz[1, 1];

                        viewModel.Total = totalColumna2;
                        viewModel.Titulo = $"Paso 6: Total columna 2 (filas 1 y 2) = {totalColumna2}";
                        break;
                    case 6:
                        // Eliminar valores (poner en 0) de la columna 2 en filas 1 y 2
                        viewModel.Matriz[0, 1] = 0;
                        viewModel.Matriz[1, 1] = 0;
                        viewModel.Titulo = "Paso 7: Eliminar valores de columna 2 en filas 1 y 2";
                        break;
                    case 7:
                        // Dividir el total anterior entre 5 (filas 3 a 7 = índices 2 a 6)
                        int valorDividido = viewModel.Total / 5;

                        for (int i = 2; i <= 6; i++) // filas 3 a 7
                        {
                            viewModel.Matriz[i, 3] = valorDividido; // columna 4
                        }

                        viewModel.Titulo = $"Paso 8: Distribuir {viewModel.Total} en partes iguales en columna 4 (filas 3 a 7)";
                        break;
                    case 8:
                        // Recalcular columna 3 como la suma de columnas 1, 2 y 4
                        for (int i = 0; i < viewModel.Matriz.GetLength(0); i++)
                        {
                            viewModel.Matriz[i, 2] = viewModel.Matriz[i, 0] + viewModel.Matriz[i, 1] + viewModel.Matriz[i, 3];
                        }

                        viewModel.Titulo = "Paso 9: Recalcular columna 3 = columna 1 + columna 2 + columna 4";
                        break;
                    case 9:
                        // Calcular total final de la columna 3
                        int totalFinal = 0;

                        for (int i = 0; i < viewModel.Matriz.GetLength(0); i++)
                        {
                            totalFinal += viewModel.Matriz[i, 2];
                        }

                        viewModel.Total = totalFinal;
                        viewModel.Titulo = $"Paso 10: Total final de columna 3 = {totalFinal}";
                        break;
                    case 10: // Finalizar
                        viewModel.Titulo = "";
                        viewModel.Matriz = null; // Borra la matriz para no mostrarla
                        viewModel.BloquearSiguiente = true;
                        break;

                    default:
                        viewModel.Titulo = "Fin";
                        break;
                }
            }
            else
            {
                viewModel.Titulo = "No hay matriz cargada. Por favor, inicia el proceso desde el principio.";
            }
            viewModel.PasoActual++;
            GuardarEnTempData(viewModel);
            return View("Paso", viewModel);
        }

        // Manejo de TempData usando JSON
        private void GuardarEnTempData(PasoMatrizViewModel viewModel)
        {
            TempData["matriz"] = JsonConvert.SerializeObject(viewModel.Matriz);
            TempData["paso"] = viewModel.PasoActual;
            TempData["total"] = viewModel.Total;
            TempData["titulo"] = viewModel.Titulo;
            TempData.Keep();
        }

        private PasoMatrizViewModel ObtenerDesdeTempData()
        {
            return new PasoMatrizViewModel
            {
                Matriz = JsonConvert.DeserializeObject<int[,]>(TempData["matriz"]?.ToString() ?? "") ?? new int[0, 0],
                PasoActual = int.Parse(TempData["paso"]?.ToString() ?? "1"),
                Total = int.Parse(TempData["total"]?.ToString() ?? "0"),
                Titulo = TempData["titulo"]?.ToString()
            };
        }
    }
}
