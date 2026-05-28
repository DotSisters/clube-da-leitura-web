using System.ComponentModel.DataAnnotations;
using ClubeDaLeituraWeb.WebApp.ModuloAmigo.Dominio;

namespace ClubeDaLeituraWeb.WebApp.ModuloEmprestimo.Apresentacao;

public record ListarEmprestimosViewModel(
    string Id,
    string Amigo,
    string Revista,
    DateTime Abertura,
    DateTime ConclusaoPrevista,
    string Status,
    bool EstaAtrasado
);

public record CadastrarEmprestimoViewModel(
    [Required(ErrorMessage = "O campo \"Amigo\" deve ser preenchido.")]
    String AmigoId,

    [Required(ErrorMessage = "O campo \"Revista\" deve ser preenchido.")]
    string RevistaId

);

public record DevolverEmprestimoViewModel(
    string Id,
    string Amigo,
    string Revista,
    DateTime Abertura,
    DateTime ConclusaoPrevista
);
