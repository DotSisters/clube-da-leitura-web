using System.Text.Json;
using System.Text.Json.Serialization;
using ClubeDaLeituraWeb.WebApp.ModuloAmigo.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloEmprestimo.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Dominio;

namespace ClubeDaLeituraWeb.WebApp.Compartilhado.Infra.Arquivos;

public sealed class ContextoJson
{
    public List<Caixa> Caixas { get; set; } = new();
    public List<Amigo> Amigos { get; set; } = new();
    public List<Revista> Revistas { get; set; } = new();
    public List<Emprestimo> Emprestimos { get; set; } = new();
    private readonly string _caminhoArquivo;

    public ContextoJson()
    {
        string caminhoAppData = Environment
            .GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        string caminhoDiretorio = Path.Combine(caminhoAppData, "ClubeDaLeituraWeb");

        Directory.CreateDirectory(caminhoDiretorio);

        _caminhoArquivo = Path.Combine(caminhoDiretorio, "dados.json");
    }

    public void Salvar()
    {
        JsonSerializerOptions opcoesJson = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            // ReferenceHandler = ReferenceHandler.Preserve
        };

        string jsonString = JsonSerializer.Serialize(this, opcoesJson);

        File.WriteAllText(_caminhoArquivo, jsonString);
    }

    public void Carregar()
    {
        if (!File.Exists(_caminhoArquivo))
            return;

        string jsonString = File.ReadAllText(_caminhoArquivo);

        JsonSerializerOptions opcoesJson = new JsonSerializerOptions();
        opcoesJson.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        // opcoesJson.ReferenceHandler = ReferenceHandler.Preserve;

        ContextoJson? contextoSalvo = JsonSerializer
            .Deserialize<ContextoJson>(jsonString, opcoesJson);

        if (contextoSalvo == null)
            return;

        Caixas = contextoSalvo.Caixas;

        Amigos = contextoSalvo.Amigos;

        Revistas = contextoSalvo.Revistas;
        Emprestimos = contextoSalvo.Emprestimos;
    }
}
