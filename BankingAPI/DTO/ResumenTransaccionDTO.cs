namespace BankingAPI.DTOs
{
    public class ResumenTransaccionDTO
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string Fecha { get; set; } = string.Empty;
        public decimal SaldoPosterior { get; set; }
    }
}