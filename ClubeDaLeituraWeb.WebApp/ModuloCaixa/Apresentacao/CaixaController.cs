using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Dominio;
using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeituraWeb.WebApp.ModuloCaixa.Apresentacao;

public class CaixaController : Controller
{
    private readonly IRepositorioCaixa repositorioCaixa;
    private readonly IRepositorioRevista repositorioRevista;

    public CaixaController(IRepositorioCaixa repositorioCaixa, IRepositorioRevista repositorioRevista)
    {
        this.repositorioCaixa = repositorioCaixa;
        this.repositorioRevista = repositorioRevista;
    }

    [HttpGet]
    public ActionResult Listar()
    {
        List<Caixa> caixas = repositorioCaixa.SelecionarTodos();

        List<ListarCaixasViewModel> listarVms = new List<ListarCaixasViewModel>();

        foreach (Caixa c in caixas)
        {
            ListarCaixasViewModel viewModel = new ListarCaixasViewModel(
                c.Id,
                c.Etiqueta,
                c.Cor,
                c.DiasDeEmprestimo
            );

            listarVms.Add(viewModel);
        }

        return View(listarVms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CadastrarCaixaViewModel cadastrarVm = new CadastrarCaixaViewModel(
            string.Empty,
            string.Empty,
            7
        );

        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarCaixaViewModel cadastrarVm)
    {
        if (!ModelState.IsValid)
            return View(cadastrarVm);

        List<Caixa> registros = repositorioCaixa.SelecionarTodos();

        bool etiquetaDuplicada = registros.Any(c =>
            c.Etiqueta.Equals(cadastrarVm.Etiqueta, StringComparison.OrdinalIgnoreCase)
        );

        if (etiquetaDuplicada)
        {
            ModelState.AddModelError(
                nameof(cadastrarVm.Etiqueta),
                "Não é possível cadastrar a mesma etiqueta duas vezes. Tente novamnte."
            );

            return View(cadastrarVm);
        }

        Caixa novaCaixa = new Caixa(
            cadastrarVm.Etiqueta,
            cadastrarVm.Cor,
            cadastrarVm.DiasDeEmprestimo
        );

        repositorioCaixa.Cadastrar(novaCaixa);

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(string id)
    {
        Caixa? caixa = repositorioCaixa.SelecionarPorId(id);

        if (caixa == null)
            return RedirectToAction(nameof(Listar));

        EditarCaixaViewModel editarVm = new EditarCaixaViewModel(
            id,
            caixa.Etiqueta,
            caixa.Cor,
            caixa.DiasDeEmprestimo
        );

        return View(editarVm);
    }

    [HttpPost]
    public ActionResult Editar(EditarCaixaViewModel editarVm)
    {
        if (!ModelState.IsValid)
            return View(editarVm);

        List<Caixa> caixas = repositorioCaixa.SelecionarTodos();

        bool etiquetaDuplicada = caixas.Any(c =>
            c.Id != editarVm.Id &&
            c.Etiqueta.Equals(editarVm.Etiqueta, StringComparison.OrdinalIgnoreCase)
        );

        if (etiquetaDuplicada)
        {
            ModelState.AddModelError(
                nameof(editarVm.Etiqueta),
                "Não é possível cadastrar a mesma etiqueta duas vezes. Tente novamnte."
            );

            return View(editarVm);
        }

        Caixa caixaAtualizada = new Caixa(
            editarVm.Etiqueta,
            editarVm.Cor,
            editarVm.DiasDeEmprestimo
        );

        repositorioCaixa.Editar(editarVm.Id, caixaAtualizada);

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(string id)
    {
        Caixa? caixa = repositorioCaixa.SelecionarPorId(id);

        if (caixa == null)
            return RedirectToAction(nameof(Listar));

        ExcluirCaixaViewModel excluirVm = new ExcluirCaixaViewModel(
            id,
            caixa.Etiqueta,
            caixa.Cor,
            caixa.DiasDeEmprestimo
        );

        return View(excluirVm);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirCaixaViewModel excluirVm)
    {
        Caixa? caixa = repositorioCaixa.SelecionarPorId(excluirVm.Id);

        if (caixa == null)
            return RedirectToAction(nameof(Listar));

        bool temRevistas = repositorioRevista
            .SelecionarTodos()
            .Any(r => r.Caixa != null && r.Caixa.Id == excluirVm.Id);

        if (temRevistas)
        {
            ModelState.AddModelError(string.Empty,
                "Não é possível excluir esta caixa porque há revistas vinculadas a ela.");
            return View(excluirVm);
        }

        repositorioCaixa.Excluir(excluirVm.Id);

        return RedirectToAction(nameof(Listar));
    }
}
