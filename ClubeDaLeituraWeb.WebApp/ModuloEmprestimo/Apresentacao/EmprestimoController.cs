using Microsoft.AspNetCore.Mvc;
using ClubeDaLeituraWeb.WebApp.ModuloAmigo.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloEmprestimo.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Dominio;
namespace ClubeDaLeituraWeb.WebApp.ModuloEmprestimo.Apresentacao;

public class EmprestimoController : Controller
{
    private readonly IRepositorioEmprestimo repositorioEmprestimo;
    private readonly IRepositorioAmigo repositorioAmigo;
    private readonly IRepositorioRevista repositorioRevista;

    public EmprestimoController(
        IRepositorioEmprestimo repositorioEmprestimo,
        IRepositorioAmigo repositorioAmigo,
        IRepositorioRevista repositorioRevista
    )
    {
        this.repositorioEmprestimo = repositorioEmprestimo;
        this.repositorioAmigo = repositorioAmigo;
        this.repositorioRevista = repositorioRevista;
    }

    [HttpGet]
    public ActionResult Listar()
    {

        List<Emprestimo> emprestimos = repositorioEmprestimo.SelecionarTodos();

        List<ListarEmprestimosViewModel> listarVms = new List<ListarEmprestimosViewModel>();

        foreach (Emprestimo e in emprestimos)
        {
            string status = e.EstaAtrasado ? "Atrasado" : e.Status.ToString();

            ListarEmprestimosViewModel viewModel = new ListarEmprestimosViewModel(
                e.Id,
                e.Amigo.Nome,
                e.Revista.Titulo,
                e.Abertura,
                e.ConclusaoPrevista,
                status,
                e.EstaAtrasado
            );

            listarVms.Add(viewModel);
        }

        return View(listarVms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CarregarAmigos();
        CarregarRevistas();

        CadastrarEmprestimoViewModel cadastrarVm = new CadastrarEmprestimoViewModel(
            null!,
            string.Empty
        );

        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarEmprestimoViewModel cadastrarVm)
    {
        Amigo? amigo = repositorioAmigo.SelecionarPorId(cadastrarVm.AmigoId);
        Revista? revista = repositorioRevista.SelecionarPorId(cadastrarVm.RevistaId);

        if (amigo == null)
        {
            ModelState.AddModelError(
                nameof(cadastrarVm.AmigoId),
                "O amigo selecionado não é válido."
            );
        }

        if (revista == null)
        {
            ModelState.AddModelError(
                nameof(cadastrarVm.RevistaId),
                "A revista selecionada não é válida."
            );
        }

        bool amigoEmprestimoAtivo = repositorioEmprestimo
            .SelecionarTodos()
            .Any(e =>
                e.Amigo.Id == cadastrarVm.AmigoId &&
                e.Status == StatusEmprestimo.Aberto
            );

        if (amigoEmprestimoAtivo)
        {
            ModelState.AddModelError(
                nameof(cadastrarVm.AmigoId),
                "Não é possível cadastrar um empréstimo para esse amigo. Amigo com empréstimo ativo."
            );
        }

        if (revista != null && revista.Status != StatusRevista.Disponivel)
        {
            ModelState.AddModelError(
                nameof(cadastrarVm.RevistaId),
                "Revista indiponível. Não é possível cadastrar um empréstimo para essa revista."
            );
        }

        if (!ModelState.IsValid)
        {
            CarregarAmigos();
            CarregarRevistas();

            return View(cadastrarVm);
        }

        Emprestimo novoEmprestimo = new Emprestimo(
            revista!,
            amigo!
        );

        novoEmprestimo.Abrir();

        repositorioEmprestimo.Cadastrar(novoEmprestimo);
        repositorioRevista.Editar(revista!.Id, revista);

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Devolver(string id)
    {
        Emprestimo? emprestimo = repositorioEmprestimo.SelecionarPorId(id);

        if (emprestimo == null)
            return RedirectToAction(nameof(Listar));

        DevolverEmprestimoViewModel devolverVm = new DevolverEmprestimoViewModel(
            emprestimo.Id,
            emprestimo.Amigo.Nome,
            emprestimo.Revista.Titulo,
            emprestimo.Abertura,
            emprestimo.ConclusaoPrevista
        );

        return View(devolverVm);
    }

    [HttpPost]
    public ActionResult Devolver(DevolverEmprestimoViewModel devolverVm)
    {
        Emprestimo? emprestimo = repositorioEmprestimo.SelecionarPorId(devolverVm.Id);

        if (emprestimo == null)
            return RedirectToAction(nameof(Listar));

        emprestimo.Concluir();

        repositorioEmprestimo.Editar(emprestimo.Id, emprestimo);
        repositorioRevista.Editar(emprestimo.Revista.Id, emprestimo.Revista);

        return RedirectToAction(nameof(Listar));
    }

    private void CarregarAmigos()
    {
        List<Amigo> amigos = repositorioAmigo.SelecionarTodos();

        ViewBag.Amigos = amigos;
    }

    private void CarregarRevistas()
    {
        List<Revista> revistas = repositorioRevista
            .SelecionarTodos()
            .Where(r => r.Status == StatusRevista.Disponivel)
            .ToList();

        ViewBag.Revistas = revistas;
    }
}
