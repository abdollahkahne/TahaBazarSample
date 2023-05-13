using SampleForTahaBazar.Entities.DataTransferObject;

namespace SampleForTahaBazar.Services;

public interface ISaleService {
     Task<int> CreateInvoiceAsync(InvoiceDto invoice,CancellationToken cancellationToken = default);
     Task<IQueryable<InvoiceDto>> GetInvoicesAsync(CancellationToken cancellationToken = default);
     Task UpdateInvoiceAsync(InvoiceDto invoice,CancellationToken cancellationToken = default);
     Task<int> DeleteInvoiceAsync(int id,CancellationToken cancellationToken = default);
     Task<InvoiceDto> GetInvoiceAsync(int id,CancellationToken cancellationToken = default);

}
