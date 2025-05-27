namespace Dominio.EstructuraJson
{
    public class Factura
    {
        public decimal? saldoAceptado { get; set; }
        public string? fechaVencimientoPago { get; set; }
        public required IdFactura idFactura { get; set; }
        public string? cuitComprador { get; set; }
        public string? cbuComprador { get; set; }
    };

    public class IdFactura
    {
        public string? cuitEmisor { get; set; }
        public string? cbuEmisor { get; set; }
    }
}
