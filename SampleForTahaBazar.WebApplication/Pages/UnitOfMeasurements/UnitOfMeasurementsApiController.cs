using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SampleForTahaBazar.Entities.Models;
using SampleForTahaBazar.Services;

namespace SampleForTahaBazar.WebApplication.Pages.Products
{
    [ApiController]
    [Route("api/uoms")]
    public class UnitOfMeasurementsApiController : ControllerBase
    {
        private readonly IUnitOfMeasurementService _unitOfMeasurementsService;

        public UnitOfMeasurementsApiController(IUnitOfMeasurementService unitOfMeasurementsService)
        {
            _unitOfMeasurementsService = unitOfMeasurementsService;
        }
        [HttpGet()]
        public ActionResult<IEnumerable<UnitOfMeasurement>> GetAll()
        {
            return Ok(_unitOfMeasurementsService.GetUoMs());
        }
        [HttpPost()]
        public async Task<IActionResult> Add([FromBody] UnitOfMeasurement unitOfMeasurement)
        {
            await _unitOfMeasurementsService.AddUoM(unitOfMeasurement);
            return Ok();
        }
    }
}