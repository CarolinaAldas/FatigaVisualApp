using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FatigaVisualApp.Models;
using FatigaVisualApp.Services;
using System.Text.Json;

namespace FatigaVisualApp.ViewModels;

public partial class QuizViewModel : ObservableObject
{
    private readonly ApiService _api;

    public QuizViewModel(ApiService api)
    {
        _api = api;
        CargarPreguntas();
    }

    [ObservableProperty]
    List<Pregunta> preguntas = [];

    [ObservableProperty]
    Pregunta preguntaActual = new();

    [ObservableProperty]
    int indicePregunta = 0;

    [ObservableProperty]
    double progreso = 0;

    [ObservableProperty]
    string textoPregunta = string.Empty;

    [ObservableProperty]
    List<string> opciones = [];

    [ObservableProperty]
    string opcionSeleccionada = string.Empty;

    [ObservableProperty]
    bool cargando = false;

    [ObservableProperty]
    string mensaje = string.Empty;

    private void CargarPreguntas()
    {
        Preguntas =
        [
            new() {
                Texto = "¿Cuántas horas al día pasas frente a pantallas?",
                Opciones = ["Menos de 4 horas", "Entre 4 y 8 horas", "Entre 8 y 12 horas", "Más de 12 horas"]
            },
            new() {
                Texto = "¿Con qué frecuencia sientes ardor o picazón en los ojos?",
                Opciones = ["Nunca", "Ocasionalmente", "Frecuentemente", "Siempre"]
            },
            new() {
                Texto = "¿Realizas pausas durante el uso de pantallas?",
                Opciones = ["Sí, cada 20 minutos", "Cada hora", "Raramente", "No hago pausas"]
            },
            new() {
                Texto = "¿Sientes visión borrosa al final del día?",
                Opciones = ["Nunca", "A veces", "Frecuentemente", "Siempre"]
            },
            new() {
                Texto = "¿Tienes dolores de cabeza relacionados al uso de pantallas?",
                Opciones = ["Nunca", "Ocasionalmente", "Frecuentemente", "Siempre"]
            },
            new() {
                Texto = "¿A qué distancia usas tus dispositivos?",
                Opciones = ["Distancia correcta +50cm", "Un poco cerca 30-50cm", "Muy cerca -30cm", "Varía mucho"]
            },
            new() {
                Texto = "¿Usas el modo nocturno o filtro de luz azul?",
                Opciones = ["Siempre", "A veces", "Raramente", "Nunca"]
            },
        ];

        MostrarPregunta();
    }

    private void MostrarPregunta()
    {
        if (IndicePregunta >= Preguntas.Count) return;
        PreguntaActual = Preguntas[IndicePregunta];
        TextoPregunta = PreguntaActual.Texto;
        Opciones = PreguntaActual.Opciones;
        OpcionSeleccionada = string.Empty;
        Progreso = (double)(IndicePregunta + 1) / Preguntas.Count;
        Mensaje = $"Pregunta {IndicePregunta + 1} de {Preguntas.Count}";
    }

    [RelayCommand]
    void SeleccionarOpcion(string opcion)
    {
        OpcionSeleccionada = opcion;
        int idx = Opciones.IndexOf(opcion);
        PreguntaActual.RespuestaSeleccionada = idx;
    }

    [RelayCommand]
    async Task Siguiente()
    {
        if (string.IsNullOrEmpty(OpcionSeleccionada))
        {
            await Shell.Current.DisplayAlert("Atención",
                "Selecciona una opción para continuar", "OK");
            return;
        }

        if (IndicePregunta < Preguntas.Count - 1)
        {
            IndicePregunta++;
            MostrarPregunta();
        }
        else
        {
            await FinalizarCuestionario();
        }
    }

    [RelayCommand]
    void Anterior()
    {
        if (IndicePregunta > 0)
        {
            IndicePregunta--;
            MostrarPregunta();
        }
    }

    private async Task FinalizarCuestionario()
    {
        Cargando = true;

        // Calcular índice de fatiga (0-100)
        int total = Preguntas.Sum(p => p.RespuestaSeleccionada);
        int maximo = Preguntas.Count * 3;
        int indice = (int)((double)total / maximo * 100);

        string nivel = indice switch
        {
            <= 33 => "bajo",
            <= 66 => "medio",
            _ => "alto"
        };

        var respuestas = Preguntas.Select((p, i) => new
        {
            pregunta = i + 1,
            respuesta = p.RespuestaSeleccionada
        }).ToList();

        var evaluacion = new Evaluacion
        {
            UsuarioId = Preferences.Get("usuarioId", 0),
            IndiceFatiga = indice,
            Nivel = nivel,
            RespuestasJson = JsonSerializer.Serialize(respuestas),
            Fecha = DateTime.UtcNow
        };

        var resultado = await _api.CrearEvaluacionAsync(evaluacion);

        Cargando = false;

        if (resultado is not null)
        {
            await Shell.Current.DisplayAlert(
                "Evaluación completada",
                $"Tu índice de fatiga es: {indice}/100\nNivel: {nivel}",
                "Ver resultados");
            await Shell.Current.GoToAsync("//HomeView");
        }
        else
        {
            await Shell.Current.DisplayAlert("Error",
                "No se pudo guardar la evaluación", "OK");
        }
    }
}