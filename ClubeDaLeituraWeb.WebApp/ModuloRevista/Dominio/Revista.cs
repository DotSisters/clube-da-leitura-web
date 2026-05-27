using ClubeDaLeituraWeb.WebApp.Compartilhado.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Dominio;

namespace ClubeDaLeituraWeb.WebApp.ModuloRevista.Dominio;

public class Revista : EntidadeBase<Revista>
{
    public string Titulo { get; set; } = string.Empty;
    public int NumeroEdicao { get; set; }
    public int AnoPublicacao { get; set; }
    public Caixa Caixa { get; set; } = null!;
    public StatusRevista Status { get; set; } = StatusRevista.Disponivel;

    public Revista() { }

    public Revista(string titulo, int numeroEdicao, int anoPublicacao, Caixa caixa)
    {
        Titulo = titulo;
        NumeroEdicao = numeroEdicao;
        AnoPublicacao = anoPublicacao;
        Caixa = caixa;
        Status = StatusRevista.Disponivel;
    }

    public void Emprestar()
    {
        Status = StatusRevista.Emprestada;
    }

    public void Devolver()
    {
        Status = StatusRevista.Disponivel;
    }

    public override List<string> Validar()
    {
        List<string> erros = new List<string>();

        if (string.IsNullOrWhiteSpace(Titulo))
            erros.Add("O campo \"Título\" é obrigatório.");

        else if (Titulo.Length < 2 || Titulo.Length > 100)
            erros.Add("O campo \"Título\" deve conter entre 2 e 100 caracteres.");

        if (NumeroEdicao < 0)
            erros.Add("O campo \"Número da Edição\" deve conter um valor igual ou maior que 0.");

        int anoAtual = DateTime.Now.Year;

        if (AnoPublicacao < 1 || AnoPublicacao > anoAtual)
            erros.Add("O campo \"Ano de Publicação\" deve conter uma data válida.");

        if (Caixa == null)
            erros.Add("O campo \"Caixa\" deve conter uma caixa válida.");

        return erros;
    }

    public override void Atualizar(Revista entidadeAtualizada)
    {
        Titulo = entidadeAtualizada.Titulo;
        NumeroEdicao = entidadeAtualizada.NumeroEdicao;
        AnoPublicacao = entidadeAtualizada.AnoPublicacao;
        Caixa = entidadeAtualizada.Caixa;
        Status = entidadeAtualizada.Status;
    }
}
