using ClubeDaLeituraWeb.WebApp.Compartilhado.Infra.Arquivos;
using ClubeDaLeituraWeb.WebApp.ModuloAmigo.Dominio;

namespace ClubeDaLeituraWeb.WebApp.ModuloAmigo.Infra;

public class RepositorioAmigoEmArquivo(ContextoJson contexto) : RepositorioBaseEmArquivo<Amigo>(contexto), IRepositorioAmigo
{
    protected override List<Amigo> CarregarRegistros()
    {
        return contexto.Amigos;
    }
}
