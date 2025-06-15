namespace BankingAPI.DTOs
{
    public class ResumenCuentaDTO
    {
        public string NumeroCuenta { get; set; } = string.Empty;
        public decimal SaldoFinal { get; set; }
        public List<ResumenTransaccionDTO> Transacciones { get; set; } = new();
    }
}
