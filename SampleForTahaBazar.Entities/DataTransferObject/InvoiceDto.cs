using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleForTahaBazar.Entities.DataTransferObject
{
    public class InvoiceDto : IValidatableObject
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTimeOffset InvoiceDate { get; set; }
        public IEnumerable<SalesLineDto> SalesLines { get; set; } = new List<SalesLineDto>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResult = new List<ValidationResult>();
            try
            {
                var salesLinesDictionary = SalesLines.ToDictionary(s => s.ProductId);
                if (salesLinesDictionary.Count < SalesLines.Count())
                {
                    validationResult.Add(new ValidationResult("There is a repetitive product in the list of items"));
                }
            }
            catch (System.Exception ex)
            {

                validationResult.Add(new ValidationResult(ex.Message));
            }
            return validationResult;
        }
    }
}