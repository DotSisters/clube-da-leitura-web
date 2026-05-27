using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Dominio;
using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeituraWeb.WebApp.ModuloRevista.Apresentacao;

public class RevistaController : Controller
{
    private readonly IRepositorioRevista repositorioRevista;
    private readonly IRepositorioCaixa repositorioCaixa;

    public RevistaController(IRepositorioRevista repositorioRevista, IRepositorioCaixa repositorioCaixa)
    {
        this.repositorioRevista = repositorioRevista;
        this.repositorioCaixa = repositorioCaixa;
    }

    [HttpGet]
    public ActionResult Listar()
    {
        List<Revista> revistas = repositorioRevista.SelecionarTodos();

        List<ListarRevistasViewModel> listarVms = new List<ListarRevistasViewModel>();

        foreach (Revista revista in revistas)
        {
            ListarRevistasViewModel viewModel = new ListarRevistasViewModel(
            revista.Id,
            revista.Titulo,
            revista.NumeroEdicao,
            revista.AnoPublicacao,
            revista.Caixa.Etiqueta,
            revista.Status.ToString()
        );

            listarVms.Add(viewModel);
        }

        return View(listarVms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CarregarCaixas();

        CadastrarRevistaViewModel cadastrarVm = new CadastrarRevistaViewModel(
            string.Empty,
            0,
            0,
            string.Empty
        );

        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarRevistaViewModel cadastrarVm)
    {
Caixa? caixaSelecionada = repositorioCaixa.SelecionarPorId(cadastrarVm.CaixaId);

    if (caixaSelecionada == null)
    {
        ModelState.AddModelError(
            nameof(cadastrarVm.CaixaId),
            "A caixa selecionada é inválida."
        );
    }

    List<Revista> registros = repositorioRevista.SelecionarTodos();

    bool tituloDuplicado = registros.Any(r =>
        r.Titulo.Equals(cadastrarVm.Titulo, StringComparison.OrdinalIgnoreCase)
    );

    bool edicaoDuplicada = registros.Any(r =>
        r.NumeroEdicao == cadastrarVm.NumeroEdicao
    );

    if (tituloDuplicado)
    {
        ModelState.AddModelError(
            nameof(cadastrarVm.Titulo),
            "Já existe uma revista com este título."
        );
    }

    if (edicaoDuplicada)
    {
        ModelState.AddModelError(
            nameof(cadastrarVm.NumeroEdicao),
            "Já existe uma revista com este número de edição."
        );
    }

    if (!ModelState.IsValid)
    {
        CarregarCaixas();
        return View(cadastrarVm);
    }

    Revista novaRevista = new Revista(
        cadastrarVm.Titulo,
        cadastrarVm.NumeroEdicao,
        cadastrarVm.AnoPublicacao,
        caixaSelecionada!
    );

    repositorioRevista.Cadastrar(novaRevista);

    return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(string id)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(id);

        if (revista == null)
            return RedirectToAction(nameof(Listar));

        CarregarCaixas();

        EditarRevistaViewModel editarVm = new EditarRevistaViewModel(
            revista.Id,
            revista.Titulo,
            revista.NumeroEdicao,
            revista.AnoPublicacao,
            revista.Caixa.Id
        );

        return View(editarVm);
    }

    [HttpPost]
    public ActionResult Editar(EditarRevistaViewModel editarVm)
    {
        Caixa? caixaSelecionada = repositorioCaixa.SelecionarPorId(editarVm.CaixaId);

        if (caixaSelecionada == null)
        {
            ModelState.AddModelError(
                nameof(editarVm.CaixaId),
                "A caixa selecionada é inválida."
            );
        }

        if (!ModelState.IsValid)
        {
            CarregarCaixas();

            return View(editarVm);
        }

        Revista revistaAtualizada = new Revista(
            editarVm.Titulo,
            editarVm.NumeroEdicao,
            editarVm.AnoPublicacao,
            caixaSelecionada!
        );

        repositorioRevista.Editar(editarVm.Id, revistaAtualizada);

        return RedirectToAction(nameof(Listar));
    }


    [HttpGet]
    public ActionResult Excluir(string id)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(id);

        if (revista == null)
            return RedirectToAction(nameof(Listar));

        ExcluirRevistaViewModel excluirVm = new ExcluirRevistaViewModel(
            revista.Id,
            revista.Titulo,
            revista.NumeroEdicao,
            revista.AnoPublicacao,
            revista.Caixa.Etiqueta
        );

        return View(excluirVm);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirRevistaViewModel excluirVm)
    {
        repositorioRevista.Excluir(excluirVm.Id);

        return RedirectToAction(nameof(Listar));
    }

    private void CarregarCaixas()
    {
        List<Caixa> caixas = repositorioCaixa.SelecionarTodos();

        ViewBag.Caixas = caixas;
    }
}
