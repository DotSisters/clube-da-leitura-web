using System.Text.Json.Serialization;
using ClubeDaLeituraWeb.WebApp.Compartilhado.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloAmigo.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Dominio;

namespace ClubeDaLeituraWeb.WebApp.ModuloEmprestimo.Dominio;

public class Emprestimo : EntidadeBase<Emprestimo>
{
    public Revista Revista { get; private set; } = null!;
    public Amigo Amigo { get; private set; } = null!;

    [JsonInclude]
    public StatusEmprestimo Status { get; private set; }

    [JsonInclude]
    public DateTime Abertura { get; private set; }
    public DateTime ConclusaoPrevista
    {
        get
        {
            int diasDeEmprestimo = Revista.Caixa.DiasDeEmprestimo;

            DateTime conclusao = Abertura.AddDays(diasDeEmprestimo);

            return conclusao;
        }
    }
    public bool EstaAtrasado
    {
        get
        {
            return Status == StatusEmprestimo.Aberto && DateTime.Now > ConclusaoPrevista;
        }
    }

    [JsonConstructor]
    public Emprestimo(Revista revista, Amigo amigo)
    {
        Revista = revista;
        Amigo = amigo;
    }

    public override List<string> Validar()
    {
        List<string> erros = new List<string>();

        if (Revista == null)
            erros.Add("O campo \"Revista\" deve ser preenchido.");

        if (Amigo == null)
            erros.Add("O campo \"Amigo\" deve ser preenchido.");

        if (Revista != null && Revista.Status != StatusRevista.Disponivel)
            erros.Add("A revista selecionada não está disponível.");

        return erros;
    }

    public override void Atualizar(Emprestimo entidadeAtualizada)
    {
        Revista = entidadeAtualizada.Revista;
        Amigo = entidadeAtualizada.Amigo;
        Status = entidadeAtualizada.Status;
        Abertura = entidadeAtualizada.Abertura;
    }

    public void Abrir()
    {
        Abertura = DateTime.Now;
        Status = StatusEmprestimo.Aberto;

        Revista.Emprestar();
    }

    public void Concluir()
    {
        Status = StatusEmprestimo.Concluido;
        Revista.Devolver();
    }
}
