using GetsBets.Models;
using GetsBets.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GetsBets
{
    [Route("[controller]")]
    [ApiController]
    public class ExtractionController : ControllerBase
    {
        private Serilog.ILogger _logger = Log.ForContext<ExtractionController>();
        [HttpPut]
        [Route("/extractions/trigger-extraction")]
        public async Task<IActionResult> TriggerExtractionAsync()
        {
            var rez=await _extractionService.TriggerExtractionAsync()
                .Match(ok =>
                {
                    _logger.Information("Manual extraction from source successful");
                    return StatusCode(200, "manual extraction succesful");
                }, err =>
                {
                    _logger.Error($"Failed manual extraction from source with error:{err.Message}");
                    return StatusCode(500, $"Manual extraction failed with reason: {err.Message}");
                });
            return rez;
        }
        

        public IExtractionService _extractionService { get; }

        // GET: api/<ValuesController1>
        [HttpGet]
        [Route("/extractions/get-extractions-for-date")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetExtractionsForDateAsync(GetExtractionsForDateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var modelErrrors = GetModelStateErrors(ModelState);
                return BadRequest(modelErrrors);
            }
            var  result = await Adapter
                .Adapt(dto)
                .ToAsync()
            
            .Bind(_extractionService.GetExtractionsForDateAsync)
            .Match(ok =>
            {
                return StatusCode(200, ok);
            }, err =>
            {
                return StatusCode(500, err.Message);
            });
            return result;
            
        }

       

        [HttpGet]
        [Route("/extractions/get-top-extracted-numbers-for-date")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetTopExtractedNumbersForDateAsync(GetTopExtractedNumbersDto dto)
        {
            if (!ModelState.IsValid)
            {
                var modelErrrors = GetModelStateErrors(ModelState);
                return BadRequest(modelErrrors);
            }
            var result = await Adapter
                .Adapt(dto)
                .ToAsync()

            .Bind(_extractionService.GetTopExtractedNumbersForDateAsync2)
            .Match(ok =>
            {
                return StatusCode(200, ok);
            }, err =>
            {
                return StatusCode(500, err.Message);
            });
            return result;

        }
        private static string GetModelStateErrors(ModelStateDictionary modelState)
        {
            var errorList = modelState
                .Keys
                .SelectMany(key => modelState[key].Errors)
                .Select(error => error.Exception != null ? error.Exception.Message : error.ErrorMessage)
                .ToList();
            return string.Join(Environment.NewLine, errorList);
        }
        

        public ExtractionController(IExtractionService extractionService)
        {
            _extractionService = extractionService??throw new ArgumentNullException(nameof(extractionService));
        }
    }
}
