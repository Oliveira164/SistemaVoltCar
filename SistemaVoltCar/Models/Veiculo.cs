namespace SistemaVoltCar.Models
{
    public class Veiculo
    {
        public int IdVeiculo { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public int Ano { get; set; }
        public decimal Valor { get; set; }

        public int IdFornecedor { get; set; }
    }
}
