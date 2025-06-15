namespace BankingAPI.DTO
{
    public class RegistrarTransaccionDTO
    {
        public string? NumeroCuenta { get; set; }
        public string? Tipo { get; set; }  // "Deposito" o "Retiro"
        public decimal Monto { get; set; }
    }
}
