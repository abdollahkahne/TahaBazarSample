using Microsoft.EntityFrameworkCore;
using SampleForTahaBazar.Entities;
using SampleForTahaBazar.Entities.DataTransferObject;
using SampleForTahaBazar.Entities.Models;

namespace SampleForTahaBazar.Services;
public class SaleService : ISaleService
{
    private readonly SaleDbContext _dbContext;

    public SaleService(SaleDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    /// <summary>
    /// Add invoice and its line to database
    /// </summary>
    /// <param name="invoiceDto">Invoice to Add</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Id of added invoice</returns>
    public async Task<int> CreateInvoiceAsync(InvoiceDto invoiceDto, CancellationToken cancellationToken = default)
    {
        var invoice = new Invoice()
        {
            InvoiceDate = invoiceDto.InvoiceDate,
            Description = invoiceDto.Description,
        };
        _dbContext.Invoices.Add(invoice);
        var lines = invoiceDto.SalesLines.Select(item => new SaleLine()
        {
            Invoice = invoice,
            // InvoiceId = invoice.Id,
            ProductId = item.ProductId,
            UnitOfMeasurementId = item.UnitOfMeasurementId,
            Count = item.Count,
            UnitPrice = item.UnitPrice,
            GrossTotalPrice = item.Count * item.UnitPrice,
            Discount = item.Discount,
            Taxes = item.Taxes,
            NetTotalPrice = (item.Count) * (item.UnitPrice) + item.Taxes - item.Discount,
        });

        _dbContext.SaleLines.AddRange(lines);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return invoice.Id;
    }

    public async Task<int> DeleteInvoiceAsync(int id, CancellationToken cancellationToken = default)
    {
        var invoice = _dbContext.Invoices.SingleOrDefault(i => i.Id == id);
        if (invoice is not null)
        {
            var lines = _dbContext.SaleLines.Where(s => s.Id == id);
            _dbContext.SaleLines.RemoveRange(lines);
            _dbContext.Invoices.Remove(invoice);
        }

        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<InvoiceDto> GetInvoiceAsync(int id, CancellationToken cancellationToken = default)
    {
        var invoice = _dbContext.Invoices.Where(i => i.Id == id)
        .Include(i => i.SaleLines).ThenInclude(s => s.UnitOfMeasurement)
        .Include(i => i.SaleLines).ThenInclude(s => s.Product)
        .Select(i => new InvoiceDto()
        {
            Id = i.Id,
            InvoiceDate = i.InvoiceDate,
            Description = i.Description,
            SalesLines = i.SaleLines.Select(s => new SalesLineDto
            {
                Id = s.Id,
                Product = new ProductDto() { Id = s.Product.Id, Name = s.Product.Name, },
                ProductId = s.Product.Id,
                UnitOfMeasurement = new UnitOfMeasurementDto() { Id = s.UnitOfMeasurement.Id, Name = s.UnitOfMeasurement.Name, },
                UnitOfMeasurementId = s.UnitOfMeasurementId,
                Count = s.Count,
                UnitPrice = s.UnitPrice,
                Taxes = s.Taxes,
                Discount = s.Discount,
            }),
        }).AsNoTracking()
        .SingleOrDefault();
        return Task.FromResult(invoice);
    }

    public Task<IQueryable<InvoiceDto>> GetInvoicesAsync(CancellationToken cancellationToken = default)
    {
        var invoices = _dbContext.Invoices
        .Include(i => i.SaleLines).ThenInclude(s => s.UnitOfMeasurement)
        .Include(i => i.SaleLines).ThenInclude(s => s.Product)
        .Select(i => new InvoiceDto()
        {
            Id = i.Id,
            InvoiceDate = i.InvoiceDate,
            Description = i.Description,
            SalesLines = i.SaleLines.Select(s => new SalesLineDto
            {
                Product = new ProductDto() { Id = s.Product.Id, Name = s.Product.Name, },
                UnitOfMeasurement = new UnitOfMeasurementDto() { Id = s.UnitOfMeasurement.Id, Name = s.UnitOfMeasurement.Name, },
                Count = s.Count,
                UnitPrice = s.UnitPrice,
                Taxes = s.Taxes,
                Discount = s.Discount,
            }),
        }).AsNoTracking().AsQueryable();
        return Task.FromResult(invoices);
    }

    public async Task UpdateInvoiceAsync(InvoiceDto invoiceDto, CancellationToken cancellationToken = default)
    {
        await SharpPad.Output.Dump(invoiceDto);
        var invoice = _dbContext.Invoices.Where(i => i.Id == invoiceDto.Id).Include(i => i.SaleLines).AsTracking().SingleOrDefault();
        if (invoice is not null)
        {
            invoice.Description = invoiceDto.Description;
            invoice.InvoiceDate = invoiceDto.InvoiceDate;
            _dbContext.Entry(invoice).State = EntityState.Modified;
            // consider user is not evil so he may not manually changed 
            foreach (var item in invoiceDto.SalesLines)
            {
                var line = invoice.SaleLines.Where(s => s.Id == item.Id).SingleOrDefault(); // since user may be evil and send an existed id from another invoice we should check only sale line from this invoice!
                // Item already does exist in the db (It may changed or deleted or unchanged)
                if (line is not null)
                {
                    _dbContext.Entry(line).State = item.InvoiceLineState switch
                    {

                        InvoiceLineState.Deleted => EntityState.Deleted,
                        InvoiceLineState.Updated => EntityState.Modified,
                        _ => EntityState.Detached,
                    };
                    if (item.InvoiceLineState == InvoiceLineState.Updated)
                    {
                        line.Count = item.Count;
                        line.Discount = item.Discount;
                        line.ProductId = item.ProductId;
                        line.UnitOfMeasurementId = item.UnitOfMeasurementId;
                        line.Taxes = item.Taxes;
                        line.UnitPrice = item.UnitPrice;
                    }
                }
                else
                {
                    // Item created new so we should insert it in database (if it is not deleted just after addition!!)
                    if (item.InvoiceLineState != InvoiceLineState.Deleted)
                    {
                        var saleLine = new SaleLine()
                        {
                            Invoice = invoice,
                            ProductId = item.ProductId,
                            UnitOfMeasurementId = item.UnitOfMeasurementId,
                            UnitPrice = item.UnitPrice,
                            Count = item.Count,
                            Discount = item.Discount,
                            Taxes = item.Taxes,
                        };
                        _dbContext.SaleLines.Add(saleLine);
                    }

                }
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
