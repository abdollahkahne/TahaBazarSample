namespace SampleForTahaBazar.Entities.DataTransferObject
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<SalesLineDto> SalesLines { get; set; } = new List<SalesLineDto>();

    }
}