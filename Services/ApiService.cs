using FatigaVisualApp.Models;
using System.Text;
using System.Text.Json;

namespace FatigaVisualApp.Services;

public class ApiService
{
    private readonly HttpClient _http;

    // En emulador Android usa 10.0.2.2 en vez de localhost
    private const string BaseUrl = "http://localhost:5062/api";

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiService()
    {
        // Permite certificados de desarrollo en el emulador
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (m, c, ch, e) => true
        };
        _http = new HttpClient(handler);
    }

    // ── USUARIOS ────────────────────────────────────────────
    public async Task<List<Usuario>?> GetUsuariosAsync()
    {
        var r = await _http.GetAsync($"{BaseUrl}/Usuarios");
        if (!r.IsSuccessStatusCode) return null;
        var body = await r.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Usuario>>(body, _jsonOptions);
    }

    public async Task<Usuario?> GetUsuarioAsync(int id)
    {
        var r = await _http.GetAsync($"{BaseUrl}/Usuarios/{id}");
        if (!r.IsSuccessStatusCode) return null;
        var body = await r.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Usuario>(body, _jsonOptions);
    }

    public async Task<Usuario?> CrearUsuarioAsync(Usuario usuario)
    {
        var json = JsonSerializer.Serialize(usuario);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var r = await _http.PostAsync($"{BaseUrl}/Usuarios", content);
        if (!r.IsSuccessStatusCode) return null;
        var body = await r.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Usuario>(body, _jsonOptions);
    }

    // ── EVALUACIONES ────────────────────────────────────────
    public async Task<List<Evaluacion>?> GetEvaluacionesAsync(int usuarioId)
    {
        var r = await _http.GetAsync($"{BaseUrl}/Evaluaciones/usuario/{usuarioId}");
        if (!r.IsSuccessStatusCode) return null;
        var body = await r.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Evaluacion>>(body, _jsonOptions);
    }

    public async Task<Evaluacion?> CrearEvaluacionAsync(Evaluacion evaluacion)
    {
        var json = JsonSerializer.Serialize(evaluacion);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var r = await _http.PostAsync($"{BaseUrl}/Evaluaciones", content);
        if (!r.IsSuccessStatusCode) return null;
        var body = await r.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Evaluacion>(body, _jsonOptions);
    }


    public async Task<List<Estadistica>?> GetEstadisticasAsync(int usuarioId)
    {
        var r = await _http.GetAsync($"{BaseUrl}/Estadisticas/usuario/{usuarioId}");
        if (!r.IsSuccessStatusCode) return null;
        var body = await r.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Estadistica>>(body, _jsonOptions);
    }

    // ── AUTH ────────────────────────────────────────────────
    public async Task<AuthResponse?> LoginAsync(string correo, string password)
    {
        var body = JsonSerializer.Serialize(new { correo, password });
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        var r = await _http.PostAsync($"{BaseUrl}/auth/login", content);
        if (!r.IsSuccessStatusCode) return null;
        var json = await r.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AuthResponse>(json, _jsonOptions);
    }

    public async Task<AuthResponse?> RegistroAsync(string nombre, string correo, string password)
    {
        var body = JsonSerializer.Serialize(new { nombre, correo, password });
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        var r = await _http.PostAsync($"{BaseUrl}/auth/registro", content);
        if (!r.IsSuccessStatusCode) return null;
        var json = await r.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AuthResponse>(json, _jsonOptions);
    }

    public async Task<AuthResponse?> LoginGoogleAsync(string idToken)
    {
        var body = JsonSerializer.Serialize(new { idToken });
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        var r = await _http.PostAsync($"{BaseUrl}/auth/google", content);
        if (!r.IsSuccessStatusCode) return null;
        var json = await r.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AuthResponse>(json, _jsonOptions);
    }

    public void SetAuthToken(string token)
    {
        _http.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }


}