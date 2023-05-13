using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SampleForTahaBazar.Entities;
using SampleForTahaBazar.Entities.DataTransferObject;
using SampleForTahaBazar.Entities.Models;

namespace SampleForTahaBazar.Tests.Services
{
    public class SaleServiceTests:IDisposable
    {
        private readonly SaleDbContext _saleDbContext;
        public SaleServiceTests()
        {
            var dbContextOptionsBuilder=new DbContextOptionsBuilder<SaleDbContext>();
            dbContextOptionsBuilder.UseInMemoryDatabase("InMemory");
            _saleDbContext=new SaleDbContext(dbContextOptionsBuilder.Options);
            _saleDbContext.Database.EnsureCreated();

        }
        [Fact]
        public async Task CreateInvoiceAsync_Always_ShouldCreateInvoice() {
            // Arrange
            _saleDbContext.Database.EnsureDeleted();
            _saleDbContext.Database.EnsureCreated();
            var sut=new SampleForTahaBazar.Services.SaleService(_saleDbContext);
            var invoice=new InvoiceDto() {
                Description="This is a sample for invoice",
                InvoiceDate=DateTimeOffset.UtcNow,
                SalesLines=new List<SalesLineDto>(),
            };

            // Act
            var id=await sut.CreateInvoiceAsync(invoice);

            // Assert
            Assert.NotEqual(id,default(int));

        }
        [Fact]
        public async Task CreateInvoiceAsync_IfHasAnyLine_ShouldCreateLines() {
            // Arrange
            _saleDbContext.Database.EnsureDeleted();
            _saleDbContext.Database.EnsureCreated();
            var sut=new SampleForTahaBazar.Services.SaleService(_saleDbContext);
            await AddOnesampleProductToDatabaseAsync();
            await AddOnesampleUnitOfMeasurementToDatabaseAsync();
            var invoice=new InvoiceDto() {
                Description="This is a sample for invoice",
                InvoiceDate=DateTimeOffset.UtcNow,
                SalesLines=new List<SalesLineDto>() {
                    new SalesLineDto() {
                        Product=new ProductDto() {
                            Id=1
                        },
                        UnitOfMeasurement=new UnitOfMeasurementDto {
                            Id=1,
                        },
                        Count=10,
                        UnitPrice=12.5,
                        Taxes=2,
                        Discount=5,
                    }
                },
            };

            // Act
            var id=await sut.CreateInvoiceAsync(invoice);
            var actual=_saleDbContext.SaleLines.Where(s=>s.InvoiceId==id).Count();
            
            // Arrange
            Assert.Equal(1,actual);


        }
        [Fact]
        public async Task DeleteInvoiceAsync_IfInvoiceExist_ShouldDeleteInvoiceItself() {
            // Arrange 
            _saleDbContext.Database.EnsureDeleted();
            _saleDbContext.Database.EnsureCreated();
            var sut=new SampleForTahaBazar.Services.SaleService(_saleDbContext);
            await AddOneSampleInvoiceAsync();

            // Act
            await sut.DeleteInvoiceAsync(1);
            var invoice=_saleDbContext.Invoices.SingleOrDefault(i=>i.Id==1);

            // Assert
            Assert.Null(invoice);
        }
        [Fact]
         public async Task DeleteInvoiceAsync_IfInvoiceExist_ShouldDeleteProductLines() {
            // Arrange 
            _saleDbContext.Database.EnsureDeleted();
            _saleDbContext.Database.EnsureCreated();
            var sut=new SampleForTahaBazar.Services.SaleService(_saleDbContext);
            await AddOneSampleInvoiceAsync();

            // Act
            await sut.DeleteInvoiceAsync(1);
            var actual=_saleDbContext.SaleLines.Where(s=>s.Id==1).Count();

            // Assert
            Assert.Equal(0,actual);
         }

        [Fact]
        public async Task GetInvoiceAsync_IfInvoiceExists_ShouldReturnInvoiceItself() {
            // arrange
            _saleDbContext.Database.EnsureDeleted();
            _saleDbContext.Database.EnsureCreated();
            await AddOneSampleInvoiceAsync();
            var sut=new SampleForTahaBazar.Services.SaleService(_saleDbContext);

            // Act
            var invoice=await sut.GetInvoiceAsync(1);

            // Assert
            Assert.NotNull(invoice);            
            Assert.Equal(1,invoice.Id);

        }

        [Fact]
        public async Task GetInvoiceAsync_IfInvoiceExists_ShouldReturnSalesLine() {
            // arrange
            _saleDbContext.Database.EnsureDeleted();
            _saleDbContext.Database.EnsureCreated();
            await AddOneSampleInvoiceAsync();
            var sut=new SampleForTahaBazar.Services.SaleService(_saleDbContext);

            // Act
            var invoice=await sut.GetInvoiceAsync(1);
            var actual=invoice.SalesLines.Count();

            // Assert          
            Assert.Equal(1,actual);

        }
        [Fact]
        public async Task GetInvoiceAsync_IfInvoiceExists_ShouldReturnProductDetails() {
            // arrange
            _saleDbContext.Database.EnsureDeleted();
            _saleDbContext.Database.EnsureCreated();
            await AddOneSampleInvoiceAsync();
            var sut=new SampleForTahaBazar.Services.SaleService(_saleDbContext);

            // Act
            var invoice=await sut.GetInvoiceAsync(1);
            var actual=invoice.SalesLines.SingleOrDefault()?.Product.Name;

            // Assert          
            Assert.Equal("Sample Product 1",actual);

        }
        [Fact]
        public async Task GetInvoicesAsync_IfInvoiceExists_ShouldReturnThemAll() {
            // arrange
            _saleDbContext.Database.EnsureDeleted();
            _saleDbContext.Database.EnsureCreated();
            await AddMultipleSampleInvoiceAsync();
            var sut=new SampleForTahaBazar.Services.SaleService(_saleDbContext);

            // Act
            var invoice=await sut.GetInvoicesAsync();
            var count=invoice.AsEnumerable().Count();

            // Assert
            Assert.Equal(2,count);

        }

        [Fact]
        public async Task UpdateInvoiceAsync_InvoiceMasterChanged_InvoiceUpdated() {
            // arrange
            _saleDbContext.Database.EnsureDeleted();
            _saleDbContext.Database.EnsureCreated();
            var expected="This is an updated sample for invoice";
            await AddMultipleSampleInvoiceAsync();
            var sut=new SampleForTahaBazar.Services.SaleService(_saleDbContext);
            var updatedInvoice=new InvoiceDto() {
                Id=1,
                Description=expected,
                InvoiceDate=DateTimeOffset.UtcNow,
                SalesLines=new List<SalesLineDto>() {
                    new SalesLineDto() {
                        Product=new ProductDto() {
                            Id=1
                        },
                        UnitOfMeasurement=new UnitOfMeasurementDto {
                            Id=1,
                        },
                        Count=10,
                        UnitPrice=12.5,
                        Taxes=2,
                        Discount=5,
                    }
                },
            };

            // act
            await sut.UpdateInvoiceAsync(updatedInvoice);
            var actual=_saleDbContext.Invoices.Where(i=>i.Id==1).SingleOrDefault()?.Description;

            // Assert
            Assert.Equal(expected,actual);

        }

        [Fact]
        public async Task UpdateInvoiceAsync_AnItemAdded_ShouldUpdateItemsCount() {
            // arrange
            _saleDbContext.Database.EnsureDeleted();
            _saleDbContext.Database.EnsureCreated();
            var expected="This is an updated sample for invoice";
            await AddMultipleSampleInvoiceAsync();
            var sut=new SampleForTahaBazar.Services.SaleService(_saleDbContext);
            var updatedInvoice=new InvoiceDto() {
                Id=1,
                Description=expected,
                InvoiceDate=DateTimeOffset.UtcNow,
                SalesLines=new List<SalesLineDto>() {
                    new SalesLineDto() {
                        Id=1,
                        Product=new ProductDto() {
                            Id=1
                        },
                        UnitOfMeasurement=new UnitOfMeasurementDto {
                            Id=1,
                        },
                        Count=10,
                        UnitPrice=12.5,
                        Taxes=2,
                        Discount=5,
                        InvoiceLineState=InvoiceLineState.Updated,
                    },
                    new SalesLineDto() {
                        Product=new ProductDto() {
                            Id=1
                        },
                        UnitOfMeasurement=new UnitOfMeasurementDto {
                            Id=1,
                        },
                        Count=10,
                        UnitPrice=12.5,
                        Taxes=2,
                        Discount=5,
                        InvoiceLineState=InvoiceLineState.Created,
                    }
                },
            };

            // act
            await sut.UpdateInvoiceAsync(updatedInvoice);
            var actual=_saleDbContext.Invoices.Where(i=>i.Id==1).SingleOrDefault()?.SaleLines.Count();

            // Assert
            Assert.Equal(2,actual);

        }

        [Fact]
        public async Task UpdateInvoiceAsync_AnItemDeleted_ShouldUpdateItemsCount() {
            // arrange
            _saleDbContext.Database.EnsureDeleted();
            _saleDbContext.Database.EnsureCreated();
            var expected="This is an updated sample for invoice";
            await AddMultipleSampleInvoiceAsync();
            var sut=new SampleForTahaBazar.Services.SaleService(_saleDbContext);
            var updatedInvoice=new InvoiceDto() {
                Id=1,
                Description=expected,
                InvoiceDate=DateTimeOffset.UtcNow,
                SalesLines=new List<SalesLineDto>() {
                    new SalesLineDto() {
                        Id=1,
                        Product=new ProductDto() {
                            Id=1
                        },
                        UnitOfMeasurement=new UnitOfMeasurementDto {
                            Id=1,
                        },
                        Count=10,
                        UnitPrice=12.5,
                        Taxes=2,
                        Discount=5,
                        InvoiceLineState=InvoiceLineState.Deleted,
                    }
                },
            };

            // act
            await sut.UpdateInvoiceAsync(updatedInvoice);
            var actual=_saleDbContext.Invoices.Where(i=>i.Id==1).SingleOrDefault()?.SaleLines.Count();

            // Assert
            Assert.Equal(0,actual);

        }
        [Fact]
        public async Task UpdateInvoiceAsync_AnItemEdited_ShouldReflectModification() {
            // arrange
            _saleDbContext.Database.EnsureDeleted();
            _saleDbContext.Database.EnsureCreated();
            await AddMultipleSampleInvoiceAsync();
            var sut=new SampleForTahaBazar.Services.SaleService(_saleDbContext);
            var updatedInvoice=new InvoiceDto() {
                Id=1,
                Description="Item Details changed too",
                InvoiceDate=DateTimeOffset.UtcNow,
                SalesLines=new List<SalesLineDto>() {
                    new SalesLineDto() {
                        Id=1,
                        Product=new ProductDto() {
                            Id=1
                        },
                        UnitOfMeasurement=new UnitOfMeasurementDto {
                            Id=1,
                        },
                        Count=20,
                        UnitPrice=12.5,
                        Taxes=2,
                        Discount=5,
                        InvoiceLineState=InvoiceLineState.Updated,
                    }
                },
            };

            // act
            await sut.UpdateInvoiceAsync(updatedInvoice);
            var actual=_saleDbContext.SaleLines.Where(s=>s.Id==1).SingleOrDefault()?.Count;

            // Assert
            Assert.Equal(20,actual);

        }

        private async Task AddOnesampleProductToDatabaseAsync() {
            _saleDbContext.Products.Add(new Product() {
                Id=1,
                Name="Sample Product 1",
            });
            await _saleDbContext.SaveChangesAsync();
        }
        private async Task AddOnesampleUnitOfMeasurementToDatabaseAsync() {
            _saleDbContext.UnitOfMeasurements.Add(new UnitOfMeasurement() {
                Id=1,
                Name="Sample UoM 1",
            });
            await _saleDbContext.SaveChangesAsync();
        }
        // Add One Invoice with One Sale Line
        private async Task AddOneSampleInvoiceAsync() {
            await AddOnesampleProductToDatabaseAsync();
            await AddOnesampleUnitOfMeasurementToDatabaseAsync();
            var invoice=new Invoice() {
                Id=1,
                InvoiceDate=DateTimeOffset.UtcNow,
                Description="This is a sample invoice",
            };
            _saleDbContext.Invoices.Add(invoice);
            var saleLine=new SaleLine() {
                Id=1,
                ProductId=1,
                UnitOfMeasurementId=1,
                Invoice=invoice,
                Count=10,
                UnitPrice=10.5,
                Taxes=3,
                Discount=2,
            };
            _saleDbContext.SaleLines.Add(saleLine);
            await _saleDbContext.SaveChangesAsync();
        }

        // Add One Invoice with One Sale Line
        private async Task AddMultipleSampleInvoiceAsync() {
            await AddOnesampleProductToDatabaseAsync();
            await AddOnesampleUnitOfMeasurementToDatabaseAsync();
            var invoice1=new Invoice() {
                Id=1,
                InvoiceDate=DateTimeOffset.UtcNow,
                Description="This is a sample invoice",
            };
            var invoice2=new Invoice() {
                Id=2,
                InvoiceDate=DateTimeOffset.UtcNow,
                Description="This is another sample invoice",
            };
            _saleDbContext.Invoices.Add(invoice1);
            _saleDbContext.Invoices.Add(invoice2);
            var saleLine1=new SaleLine() {
                Id=1,
                ProductId=1,
                UnitOfMeasurementId=1,
                Invoice=invoice1,
                Count=10,
                UnitPrice=10.5,
                Taxes=3,
                Discount=2,
            };
            var saleLine2=new SaleLine() {
                Id=2,
                ProductId=1,
                UnitOfMeasurementId=1,
                Invoice=invoice2,
                Count=10,
                UnitPrice=10.5,
                Taxes=3,
                Discount=2,
            };
            _saleDbContext.SaleLines.Add(saleLine1);
            _saleDbContext.SaleLines.Add(saleLine2);
            await _saleDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _saleDbContext.Dispose();
        }
    }
}