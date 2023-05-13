namespace SampleForTahaBazar.Entities.DataTransferObject
{
    public class SalesLineDto
    {
        public int Id { get; set; }
        public ProductDto? Product { get; set; }
        public int ProductId { get; set; }
        public UnitOfMeasurementDto? UnitOfMeasurement { get; set; }
        public int UnitOfMeasurementId { get; set; }
        public InvoiceDto? Invoice { get; set; }
        public int InvoiceId { get; set; }
        public int Count { get; set; }
        public double UnitPrice { get; set; }
        public double Taxes { get; set; }
        public double Discount { get; set; }
        public double GrossTotalPrice => UnitPrice * Count;
        public double NetTotalPrice => GrossTotalPrice - Discount + Taxes;
        public InvoiceLineState InvoiceLineState { get; set; } = InvoiceLineState.UnChanged;
    }
    public enum InvoiceLineState
    {
        Created = 1,
        Deleted = 2,
        Updated = 4,
        UnChanged = 8,
    }
}