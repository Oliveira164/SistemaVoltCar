//Importando as bibliotecas 
using Microsoft.AspNetCore.Mvc;
using SistemaVoltCar.Models;
using SistemaVoltCar.Repositorio;

namespace SistemaVoltCar.Controllers
{
    public class VeiculoController : Controller
    {
        private readonly VeiculoRepositorio _veiculoRepositorio;

        public VeiculoController(VeiculoRepositorio veiculoRepositorio)
        {
            _veiculoRepositorio = veiculoRepositorio;
        }
        public IActionResult Index()
        {
            /* Retorna a View padrão associada a esta Action,
             passando como modelo a lista de todos os veículos obtida do repositório.*/
            return View(_veiculoRepositorio.TodosVeiculos());
        }

        public IActionResult CadastrarVeiculo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarVeiculo(Veiculo veiculo)
        {
            _veiculoRepositorio.CadastrarVeiculo(veiculo);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult EditarVeiculo(int id)
        {
            var veiculo = _veiculoRepositorio.ObterVeiculo(id);

            if (veiculo == null)
            {
                return NotFound();
            }

            return View(veiculo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarVeiculo(int id, [Bind("IdVeiculo,Modelo,Marca,Ano,Valor")] Veiculo veiculo)
        {
            // Verifica se o ID fornecido na rota corresponde ao ID do veiculo no modelo.
            if (id != veiculo.IdVeiculo)
            {
                return BadRequest(); // Retorna um erro 400 se os IDs não corresponderem.
            }
            if (ModelState.IsValid)
            {
                //try /catch = tratamento de erros 
                try
                {
                    // Verifica se o veiculo com o Codigo fornecido existe no repositório.
                    if (_veiculoRepositorio.EditarVeiculo(veiculo))
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
                    return View(veiculo);
                }
            }
            // Se o ModelState não for válido, retorna a View com os erros de validação.
            return View(veiculo);
        }

        public IActionResult ExcluirVeiculo(int id)
        {
            // Obtém o produto específico do repositório usando o Codigo fornecido.
            _veiculoRepositorio.ExcluirVeiculo(id);
            // Retorna a View de confirmação de exclusão, passando o produto como modelo.
            return RedirectToAction(nameof(Index));
        }
    }
}
