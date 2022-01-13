using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MsoRegistrationApi.Models;
using MsoRegistrationApi.Services;
using System.Threading.Tasks; 
using System.Linq;
namespace MsoRegistrationApi.Controllers
{
	[ApiVersion( "1.0" )]
	[Route("api/v{version:apiVersion}/")]
	public class FcaValidationController : ControllerBase
	{ 
		private readonly ILogger<FcaValidationController> _logger;
		private readonly RegistrationService<FcaDetails> _service;
		public FcaValidationController(ILogger<FcaValidationController> logger,RegistrationService<FcaDetails> service)
		{
			_logger = logger;
			_service = service;
		}
 
		
		[HttpPost]
		[Route("validatefca")]
		public async Task<IActionResult> ValidateFca([FromBody]string fcanumber)
		{
			var allFca = await _service.GetAsync();
			var filteredFca = allFca?.FirstOrDefault(x=>x.FcaNumber == fcanumber && x.IsValid);
			 if (filteredFca is null)
			{
				return NotFound();
			}
			else
			{
			return Ok();
			}
		}
	}
}
