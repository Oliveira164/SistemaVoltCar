//Importando as bibliotecas
using MySql.Data.MySqlClient;
using SistemaVoltCar.Models;
using System.Data;

namespace SistemaVoltCar.Repositorio
{
    public class FornecedorRepositorio(IConfiguration configuration)
    {
        //Criando um campo para armazenar a string de conexão
        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");

        //Método para cadastrar fornecedor
        public void CadastrarFornecedor(Fornecedor fornecedor)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                //Cria um comando SQL para inserir um novo dado na tabela "Fornecedor"
                cmd.CommandText = "INSERT INTO FORNECEDOR (Nome, CNPJ, Telefone) VALUES (@nome, @cnpj, @telefone)";
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = fornecedor.Nome;
                cmd.Parameters.Add("@cnpj", MySqlDbType.Int64).Value = fornecedor.CNPJ;
                cmd.Parameters.Add("@telefone", MySqlDbType.Decimal).Value = fornecedor.Telefone;
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        //Método para buscar fornecedor pelo CNPJ
        public Fornecedor ObterFornecedor(int Codigo)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new("SELECT * FROM Fornecedor WHERE IdFornecedor = @codigo", conexao);
                cmd.Parameters.AddWithValue("@codigo", Codigo);

                // Cria um adaptador de dados (não utilizado diretamente para ExecuteReader)
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                // Declara um leitor de dados do MySQL
                MySqlDataReader dr;
                // Cria um novo objeto Fornecedor para armazenar os resultados
                Fornecedor fornecedor = new Fornecedor();

                /* Executa o comando SQL e retorna um objeto MySqlDataReader para ler os resultados
                CommandBehavior.CloseConnection garante que a conexão seja fechada quando o DataReader for fechado*/
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                //Lê os resultados linha por linha
                while (dr.Read())
                {
                    fornecedor.IdFornecedor = Convert.ToInt32(dr["IdFornecedor"]);
                    fornecedor.Nome = dr["Nome"].ToString();
                    fornecedor.CNPJ = Convert.ToInt64(dr["CNPJ"]);
                    fornecedor.Telefone = Convert.ToDecimal(dr["Telefone"]);
                }
                return fornecedor;
            }
        }

        //Método para listar todos os fornecedores
        public IEnumerable<Fornecedor> TodosFornecedores()
        {
            List<Fornecedor> FornecedorList = new List<Fornecedor>();

            using(var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new("SELECT * FROM Fornecedor", conexao);

                // Cria um adaptador de dados para preencher um DataTable com os resultados da consulta
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                // Cria um novo DataTable
                DataTable dt = new DataTable();
                // metodo fill- Preenche o DataTable com os dados retornados pela consulta
                da.Fill(dt);
                // Fecha explicitamente a conexão com o banco de dados 
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    FornecedorList.Add(
                        new Fornecedor
                        {
                            IdFornecedor = Convert.ToInt32(dr["IdFornecedor"]),
                            Nome = dr["Nome"].ToString(),
                            CNPJ = Convert.ToInt64(dr["CNPJ"]),
                            Telefone = Convert.ToDecimal(dr["Telefone"])
                        });
                }
                return FornecedorList;
            }
        }

        //Método para editar os dados do fornecedor
        public bool EditarFornecedor(Fornecedor fornecedor)
        {
            try
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();
                    // Cria um novo comando SQL para atualizar dados na tabela 'Fornecedor' com base no código do fornecedor
                    MySqlCommand cmd = new MySqlCommand("UPDATE FORNECEDOR SET Nome = @nome, CNPJ = @cnpj, Telefone = @telefone WHERE IdFornecedor = @idFornecedor", conexao);

                    cmd.Parameters.Add("@idFornecedor", MySqlDbType.Int32).Value = fornecedor.IdFornecedor;
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = fornecedor.Nome;
                    cmd.Parameters.Add("@cnpj", MySqlDbType.Int64).Value = fornecedor.CNPJ;
                    cmd.Parameters.Add("@telefone", MySqlDbType.Decimal).Value = fornecedor.Telefone;

                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao atualizar produto: {ex.Message}");
                return false;
            }
        }

        //Método para excluir um fornecedor
        public void Excluir(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM Fornecedor WHERE IdFornecedor=@codigo", conexao);

                cmd.Parameters.AddWithValue("@codigo", Id);

                // Executa o comando SQL de exclusão e retorna o número de linhas afetadas
                int i = cmd.ExecuteNonQuery();

                conexao.Close();
            }
        }
    }
}
