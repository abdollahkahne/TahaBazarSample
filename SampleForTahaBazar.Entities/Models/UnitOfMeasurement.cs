namespace SampleForTahaBazar.Entities.Models;

public class UnitOfMeasurement:BasicEntity {

    public string Name {get;set;}

    public ICollection<SaleLine> SaleLines {get;}=new List<SaleLine>();
} 