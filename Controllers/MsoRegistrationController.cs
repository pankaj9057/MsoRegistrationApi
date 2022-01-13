using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MsoRegistrationApi.Models; 
using MsoRegistrationApi.Services;
using System.Threading.Tasks; 
namespace MsoRegistrationApi.Controllers
{
	[ApiVersion( "1.0" )]
	[Route("api/v{version:apiVersion}/")]
	public class MsoRegistrationController : ControllerBase
	{ 
		private readonly ILogger<MsoRegistrationController> _logger;
		private readonly RegistrationService<RegistrationDetails> _service;
		public MsoRegistrationController(ILogger<MsoRegistrationController> logger,RegistrationService<RegistrationDetails> service)
		{
			_logger = logger;
			_service = service;
		}

		[HttpPost]
		[Route("register")]
		public async Task<IActionResult> Register([FromBody]RegistrationDetails registrationDetails)
		{
			await _service.CreateAsync(registrationDetails);
			return CreatedAtAction(nameof(Get), new { id = registrationDetails.Id }, registrationDetails);
		} 

		[HttpGet("{id:length(24)}")]
		public async Task<ActionResult<RegistrationDetails>> Get(string id)
		{
			var book = await _service.GetAsync(id);

			if (book is null)
			{
				return NotFound();
			}

			return book;
		}
	}
}
