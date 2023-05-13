namespace SampleForTahaBazar.Entities.Models;

public class Product:BasicEntity
{
    public string Name { get; set; }
    public virtual ICollection<SaleLine> SaleLines {get;}=new List<SaleLine>();

}
