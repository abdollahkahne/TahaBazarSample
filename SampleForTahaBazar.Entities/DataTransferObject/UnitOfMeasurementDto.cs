namespace SampleForTahaBazar.Entities.DataTransferObject
{
    public class UnitOfMeasurementDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<SalesLineDto> SalesLines { get; set; } = new List<SalesLineDto>();

    }
}