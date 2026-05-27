using System.ComponentModel.DataAnnotations;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Dominio;

namespace ClubeDaLeituraWeb.WebApp.ModuloRevista.Apresentacao;

public record ListarRevistasViewModel(
    string Id,
    string Titulo,
    int NumeroEdicao,
    int AnoPublicacao,
    string Caixa,
    string Status
);

public record CadastrarRevistaViewModel(
    [Required(ErrorMessage = "O campo \"Título\" deve ser preenchido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo \"Título\" deve conter entre 2 e 100 caracteres.")]
    string Titulo,

    [Range(0, int.MaxValue, ErrorMessage = "O campo \"Número da Edição\" deve conter um valor igual ou maior que 0.")]
    int NumeroEdicao,

    [Range(1, 9999, ErrorMessage = "O campo \"Ano de Publicação\" deve conter uma data válida.")]
    int AnoPublicacao,

    [Required(ErrorMessage = "O campo \"Caixa\" deve ser preenchido.")]
    string CaixaId
);

public record EditarRevistaViewModel(
    string Id,

    [Required(ErrorMessage = "O campo \"Título\" deve ser preenchido.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo \"Título\" deve conter entre 2 e 100 caracteres.")]
    string Titulo,

    [Range(0, int.MaxValue, ErrorMessage = "O campo \"Número da Edição\" deve conter um valor igual ou maior que 0.")]
    int NumeroEdicao,

    [Range(1, 9999, ErrorMessage = "O campo \"Ano de Publicação\" deve conter uma data válida.")]
    int AnoPublicacao,

    [Required(ErrorMessage = "O campo \"Caixa\" deve ser preenchido.")]
    string CaixaId
);

public record ExcluirRevistaViewModel();
