using ClubeDaLeituraWeb.WebApp.ModuloAmigo.Dominio;
using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeituraWeb.WebApp.ModuloAmigo.Apresentacao;

public class AmigoController : Controller
{
    private readonly IRepositorioAmigo repositorioAmigo;

    public AmigoController(IRepositorioAmigo repositorioAmigo)
    {
        this.repositorioAmigo = repositorioAmigo;
    }

    [HttpGet]
    public ActionResult Listar()
    {
        List<Amigo> amigos = repositorioAmigo.SelecionarTodos();

        List<ListarAmigosViewModel> listarVms = new List<ListarAmigosViewModel>();

        foreach (Amigo a in amigos)
        {
            ListarAmigosViewModel viewModel = new ListarAmigosViewModel(
                a.Id,
                a.Nome,
                a.NomeResponsavel,
                a.Telefone
            );

            listarVms.Add(viewModel);
        }

        return View(listarVms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CadastrarAmigoViewModel cadastrarVm = new CadastrarAmigoViewModel(
            string.Empty,
            string.Empty,
            string.Empty
        );

        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarAmigoViewModel cadastrarVm)
    {
        List<Amigo> registros = repositorioAmigo.SelecionarTodos();

        bool nomeDuplicado = registros.Any(a =>
            a.Nome.Equals(cadastrarVm.Nome, StringComparison.OrdinalIgnoreCase)
        );

        bool telefoneDuplicado = registros.Any(a =>
           a.Telefone.Equals(cadastrarVm.Telefone, StringComparison.OrdinalIgnoreCase)
        );

        if (nomeDuplicado)
        {
            ModelState.AddModelError(
                nameof(cadastrarVm.Nome),
                "Já existe um amigo com este nome."
            );

        }

        if (telefoneDuplicado)
        {
            ModelState.AddModelError(
                nameof(cadastrarVm.Telefone),
                "Já existe um amigo com este telefone."
            );

        }

        if (!ModelState.IsValid)
            return View(cadastrarVm);

        Amigo novoAmigo = new Amigo(
            cadastrarVm.Nome,
            cadastrarVm.NomeResponsavel,
            cadastrarVm.Telefone
        );

        repositorioAmigo.Cadastrar(novoAmigo);

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(string id)
    {
        Amigo? amigo = repositorioAmigo.SelecionarPorId(id);

        if (amigo == null)
            return RedirectToAction(nameof(Listar));

        EditarAmigoViewModel editarVm = new EditarAmigoViewModel(
            id,
            amigo.Nome,
            amigo.NomeResponsavel,
            amigo.Telefone
        );

        return View(editarVm);
    }

    [HttpPost]
    public ActionResult Editar(EditarAmigoViewModel editarVm)
    {
        if (!ModelState.IsValid)
            return View(editarVm);

        Amigo amigoAtualizado = new Amigo(
            editarVm.Nome,
            editarVm.NomeResponsavel,
            editarVm.Telefone
        );

        repositorioAmigo.Editar(editarVm.Id, amigoAtualizado);

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(string id)
    {
        Amigo? amigo = repositorioAmigo.SelecionarPorId(id);

        if (amigo == null)
            return RedirectToAction(nameof(Listar));

        ExcluirAmigoViewModel excluirVm = new ExcluirAmigoViewModel(
            id,
            amigo.Nome,
            amigo.NomeResponsavel,
            amigo.Telefone
        );

        return View(excluirVm);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirAmigoViewModel excluirVm)
    {
        repositorioAmigo.Excluir(excluirVm.Id);

        return RedirectToAction(nameof(Listar));
    }
}
