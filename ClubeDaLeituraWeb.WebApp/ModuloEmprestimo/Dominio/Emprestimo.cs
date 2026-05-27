using System.Security.Cryptography;
using ClubeDaLeituraWeb.WebApp.ModuloAmigo.Dominio;

namespace ClubeDaLeituraWeb.WebApp.ModuloEmprestimo.Dominio;
public sealed class Emprestimo
{
    public string Id { get; private set; } = string.Empty;
    // public Revista Revista { get; private set; }
    public Amigo Amigo { get; private set; }
    public StatusEmprestimo Status { get; private set; }
    public DateTime Abertura { get; private set; }

    // public DateTime ConclusaoPrevista
    // {
    //     get
    //     {
    //         // // int diasDeEmprestimo = Revista.Caixa.DiasDeEmprestimo;

    //         // DateTime conclusao = Abertura.AddDays(diasDeEmprestimo);

    //         // return conclusao;
    //     }
    // }

    // public bool EstaAtrasado
    // {
    //     get
    //     {
    //         return Status == StatusEmprestimo.Aberto && DateTime.Now > ConclusaoPrevista;
    //     }
    // }
    public Emprestimo(Amigo amigo)
    {
        Id = Convert
                .ToHexString(RandomNumberGenerator.GetBytes(20))
                .ToLower()
                .Substring(0, 7);

        // Revista = revista;
        Amigo = amigo;
    }

    // public void Abrir()
    // {
    //     Abertura = DateTime.Now;
    //     Status = StatusEmprestimo.Aberto;

    //     Revista.Emprestar();
    //     Amigo.AdicionarEmprestimo(this);
    // }

    // public void Concluir()
    // {
    //     Status = StatusEmprestimo.Concluido;
    //     Revista.Devolver();
    // }
}
