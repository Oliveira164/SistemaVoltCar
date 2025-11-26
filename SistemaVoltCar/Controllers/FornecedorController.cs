//Importando as bibliotecas
using Microsoft.AspNetCore.Mvc;
using SistemaVoltCar.Models;
using SistemaVoltCar.Repositorio;

namespace SistemaVoltCar.Controllers
{
    public class FornecedorController : Controller
    {
        private readonly FornecedorRepositorio _fornecedorRepositorio;

        public FornecedorController(FornecedorRepositorio fornecedorRepositorio)
        {
            _fornecedorRepositorio = fornecedorRepositorio;
        }
        public IActionResult Index()
        {
            /* Retorna a View padrão associada a esta Action,
             passando como modelo a lista de todos os fornecedores obtida do repositório.*/
            return View(_fornecedorRepositorio.TodosFornecedores());
        }

        public IActionResult CadastrarFornecedor()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarFornecedor(Fornecedor fornecedor)
        {
            /* O parâmetro 'fornecedor' recebe os dados enviados pelo formulário,
            que são automaticamente mapeados para as propriedades da classe Fornecedor.
            Chama o método no repositório para cadastrar o novo fornecedor no sistema.*/
            _fornecedorRepositorio.CadastrarFornecedor(fornecedor);

            //redireciona para pagina Index 'nameof(Index)' garante que o nome da Action seja usado corretamente,
            return RedirectToAction(nameof(Index));
        }

        /* Action para exibir o formulário de edição de um fornecedor específico (via Requisição GET)
         Este método recebe o 'id' do fornecedor a ser editado como parâmetro.*/
        public IActionResult EditarFornecedor(int id)
        {
            // Obtém o fornecedor específico do repositório usando o ID fornecido.
            var fornecedor = _fornecedorRepositorio.ObterFornecedor(id);

            // Verifica se o fornecedor foi encontrado. É uma boa prática tratar casos onde o ID é inválido.
            if (fornecedor == null)
            {
                // Você pode retornar um NotFound (código de status 404) ou outra resposta apropriada.
                return NotFound();
            }

            // Retorna a View associada a esta Action (EditarFornecedor.cshtml),
            return View(fornecedor);
        }

        // Carrega a liista de Fornecedor que envia a alteração(post)

        [HttpPost]
        [ValidateAntiForgeryToken] // Essencial para segurança contra ataques CSRF
        /*[Bind] para especificar explicitamente quais propriedades do objeto Fornecedor podem ser vinculadas a partir dos dados do formulário.
        Isso é uma boa prática de segurança para evitar o overposting (onde um usuário malicioso pode enviar dados para propriedades
        que você não pretendia que fossem alteradas)*/
        public IActionResult EditarFornecedor(int id, [Bind("IdFornecedor,Nome,CNPJ,Telefone")] Fornecedor fornecedor)
        {
            // Verifica se o ID fornecido na rota corresponde ao ID do fornecedor no modelo.
            if (id != fornecedor.IdFornecedor)
            {
                return BadRequest(); // Retorna um erro 400 se os IDs não corresponderem.
            }
            if (ModelState.IsValid)
            {
                //try /catch = tratamento de erros 
                try
                {
                    // Verifica se o fornecedor com o Codigo fornecido existe no repositório.
                    if (_fornecedorRepositorio.EditarFornecedor(fornecedor))
                    {
                        //redireciona para a pagina index quando alterar
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {
                    // Adiciona um erro ao ModelState para exibir na View.
                    ModelState.AddModelError("", "Ocorreu um erro ao Editar.");
                    // Retorna a View com o modelo para exibir a mensagem de erro e os dados do formulário.
                    return View(fornecedor);
                }
            }
            // Se o ModelState não for válido, retorna a View com os erros de validação.
            return View(fornecedor);
        }

        public IActionResult ExcluirFornecedor(int id)
        {
            // Obtém o fornecedor específico do repositório usando o Codigo fornecido.
            _fornecedorRepositorio.Excluir(id);
            // Retorna a View de confirmação de exclusão, passando o fornecedor como modelo.
            return RedirectToAction(nameof(Index));
        }
    }
}
