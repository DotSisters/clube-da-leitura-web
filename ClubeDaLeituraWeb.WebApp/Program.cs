using ClubeDaLeituraWeb.WebApp.Compartilhado.Infra.Arquivos;
using ClubeDaLeituraWeb.WebApp.ModuloAmigo.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloAmigo.Infra;
using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Infra;
using ClubeDaLeituraWeb.WebApp.ModuloEmprestimo.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloEmprestimo.Infra;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Infra;

var builder = WebApplication.CreateBuilder(args);

// Configuração de Serviços
builder.Services.AddScoped(_ =>
{
    ContextoJson contextoJson = new ContextoJson();

    contextoJson.Carregar();

    return contextoJson;
});

builder.Services.AddScoped<IRepositorioCaixa, RepositorioCaixaEmArquivo>();

builder.Services.AddScoped<IRepositorioAmigo, RepositorioAmigoEmArquivo>();

builder.Services.AddScoped<IRepositorioRevista, RepositorioRevistaEmArquivo>();

builder.Services.AddScoped<IRepositorioEmprestimo, RepositorioEmprestimoEmArquivo>();


builder.Services.AddControllersWithViews().AddRazorOptions(options =>
{
    // Resetar a configuração padrão do MVC
    options.ViewLocationFormats.Clear();

    // Views dos módulos: /ModuloCaixa/Apresentacao/Views/Listar.cshtml
    options.ViewLocationFormats.Add("/Modulo{1}/Apresentacao/Views/{0}.cshtml");

    // Views compartilhadas: /Compartilhado/Apresentacao/Views/_Layout.cshtml
    options.ViewLocationFormats.Add("/Compartilhado/Apresentacao/Views/{0}.cshtml");
});

var app = builder.Build();

// Configuração de Middlewares
app.UseStaticFiles();

app.UseRouting();
app.MapDefaultControllerRoute();

// Execução do App
app.Run();
