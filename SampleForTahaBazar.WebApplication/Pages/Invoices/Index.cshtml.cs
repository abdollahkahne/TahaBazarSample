using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SampleForTahaBazar.Entities.DataTransferObject;
using SampleForTahaBazar.Services;

namespace SampleForTahaBazar.WebApplication.Invoices
{
    public class IndexPageModel : PageModel
    {
        private readonly ILogger<Index> _logger;
        private readonly ISaleService _saleService;
        private readonly IStringLocalizer<IndexPageModel> _localizer;

        public IndexPageModel(ILogger<Index> logger, ISaleService saleService, IStringLocalizer<IndexPageModel> localizer)
        {
            _logger = logger;
            _saleService = saleService;
            _localizer = localizer;
        }
        public InputModel? Input { get; set; }

        [ViewData]
        public string Title => _localizer["Invoices"];// This view data can not changed at actions/handlers

        public async Task<IActionResult> OnGetAsync()
        {

            var invoices = (await _saleService.GetInvoicesAsync()).ToList();
            // await SharpPad.Output.Dump(invoices);
            if (invoices is not null)
                Input = new InputModel()
                {
                    Invoices = invoices,
                };
            else
                Input = new InputModel()
                {
                    Invoices = new List<InvoiceDto>(),
                };
            return Page();
        }
        public class InputModel
        {
            public IEnumerable<InvoiceDto>? Invoices { get; set; }
            public int InvoiceCounts => Invoices?.Count() ?? 0;
        }
    }
}