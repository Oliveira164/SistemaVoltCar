//Importando as bibliotecas
using MySql.Data.MySqlClient;
using SistemaVoltCar.Models;
using System.Data;

namespace SistemaVoltCar.Repositorio
{
    public class VeiculoRepositorio(IConfiguration configuration)
    {
        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");

        //Método cadastrar veículo
        public void CadastrarVeiculo(Veiculo veiculo)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();

                //Cria um novo comando SQL para inserir dados na tabela 'cliente'
                cmd.CommandText = "INSERT INTO Veiculo (Modelo, Marca, Ano, Valor) VALUES (@modelo, @marca, @ano, @valor)";

                //Adicionando os parâmetros 
                cmd.Parameters.Add("@modelo", MySqlDbType.VarChar).Value = veiculo.Modelo;
                cmd.Parameters.Add("@marca", MySqlDbType.VarChar).Value = veiculo.Marca;
                cmd.Parameters.Add("@ano", MySqlDbType.Int32).Value = veiculo.Ano;
                cmd.Parameters.Add("@Valor", MySqlDbType.Decimal).Value = veiculo.Valor;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        //Método para buscar um veículo pelo seu código
        public Veiculo ObterVeiculo(int Codigo)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Veiculo where IdVeiculo = @codigo", conexao);

                // Adiciona um parâmetro para o código a ser buscado, definindo seu tipo e valor
                cmd.Parameters.AddWithValue("@codigo", Codigo);

                // Cria um adaptador de dados (não utilizado diretamente para ExecuteReader)
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                MySqlDataReader dr;

                Veiculo veiculo = new Veiculo();

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    veiculo.IdVeiculo = Convert.ToInt32(dr["IdVeiculo"]);
                    veiculo.Modelo = (string)(dr["Modelo"]);
                    veiculo.Marca = (string)(dr["Marca"]);
                    veiculo.Ano = Convert.ToInt32(dr["Ano"]);
                    veiculo.Valor = Convert.ToDecimal(dr["Valor"]);
                }
                return veiculo;
            }
        }

        //Método para listar todos os veículos
        public IEnumerable<Veiculo> TodosVeiculos()
        {
            // Cria uma nova lista para armazenar os objetos Veiculo
            List<Veiculo> VeiculoList = new List<Veiculo>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                // Cria um novo comando SQL para selecionar todos os registros da tabela 'Veiculo'
                MySqlCommand cmd = new MySqlCommand("SELECT * from Veiculo", conexao);

                // Cria um adaptador de dados para preencher um DataTable com os resultados da consulta 
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                // Cria um novo DataTable
                DataTable dt = new DataTable();
                // metodo fill- Preenche o DataTable com os dados retornados pela consulta
                da.Fill(dt);

                conexao.Close();

                // Interage sobre cada linha(DataRow) do DataTable
                foreach (DataRow dr in dt.Rows)
                {
                    //Cria um novo objeto Veiculo e preenche suas propriedades com os valores da linha atual
                    VeiculoList.Add(
                        new Veiculo { 
                          IdVeiculo = Convert.ToInt32(dr["IdVeiculo"]),
                          Modelo = ((string)dr["Modelo"]),
                          Marca = ((string)dr["Marca"]),
                          Ano = Convert.ToInt32(dr["Ano"]),
                          Valor = Convert.ToDecimal(dr["Valor"]),
                        }
                     );
                }
                return VeiculoList;
            }
        }

        //Método para editar os dados de um veículo
        public bool EditarVeiculo(Veiculo veiculo)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();
                    // Cria um novo comando SQL para atualizar dados na tabela 'produto' com base no código
                    MySqlCommand cmd = new MySqlCommand("UPDATE Veiculo SET Modelo=@modelo, Marca=@marca, Ano=@ano, Valor=@valor WHERE IdVeiculo=@codigo", conexao);

                    cmd.Parameters.Add("@codigo", MySqlDbType.Int32).Value = veiculo.IdVeiculo;
                    cmd.Parameters.Add("@modelo", MySqlDbType.VarChar).Value = veiculo.Modelo;
                    cmd.Parameters.Add("@marca", MySqlDbType.VarChar).Value = veiculo.Marca;
                    cmd.Parameters.Add("@ano", MySqlDbType.Int32).Value = veiculo.Ano;
                    cmd.Parameters.Add("@valor", MySqlDbType.Decimal).Value = veiculo.Valor;
                    // Executa o comando SQL de atualização e retorna o número de linhas afetadas
                    //executa e verifica se a alteração foi realizada
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0; // Retorna true se ao menos uma linha foi atualizada
                }
            }
            catch (MySqlException ex)
            {
                // Logar a exceção (usar um framework de logging como NLog ou Serilog)
                Console.WriteLine($"Erro ao atualizar veículo: {ex.Message}");
                return false; // Retorna false em caso de erro
            }
        }

        //Método para excluir um veículo
        public void ExcluirVeiculo(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL
                conexao.Open();

                // Cria um novo comando SQL para deletar um registro da tabela 'Produto' com base no código
                MySqlCommand cmd = new MySqlCommand("DELETE FROM Veiculo where IdVeiculo=@codigo", conexao);

                // Adiciona um parâmetro para o código a ser excluído, definindo seu tipo e valor
                cmd.Parameters.AddWithValue("@codigo", Id);

                // Executa o comando SQL de exclusão e retorna o número de linhas afetadas
                int i = cmd.ExecuteNonQuery();

                conexao.Close(); // Fecha  a conexão com o banco de dados
            }
        }
    }
}
