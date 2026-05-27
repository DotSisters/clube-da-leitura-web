using ClubeDaLeituraWeb.WebApp.Compartilhado.Infra.Arquivos;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Dominio;

namespace ClubeDaLeituraWeb.WebApp.ModuloRevista.Infra;

public class RepositorioRevistaEmArquivo(ContextoJson contexto) : RepositorioBaseEmArquivo<Revista>(contexto), IRepositorioRevista
{
    protected override List<Revista> CarregarRegistros()
    {
        return contexto.Revistas;
    }
}
