using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MsoRegistrationApi.Models;
using MsoRegistrationApi.Services;
using System.Threading.Tasks; 
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MsoRegistrationApi.Controllers
{
	[ApiVersion( "1.0" )]
	[Route("api/v{version:apiVersion}/")]
	public class GenerateMasterDataController : ControllerBase
	{ 
		private readonly ILogger<GenerateMasterDataController> _logger;
		private readonly RegistrationService<FcaDetails> _fcaService;
        private readonly RegistrationService<Title> _titleService;
        private readonly RegistrationService<Role> _roleService;
        private readonly RegistrationService<Brand> _brandService;
		public GenerateMasterDataController(ILogger<GenerateMasterDataController> logger,
        RegistrationService<FcaDetails> fcaService,
        RegistrationService<Title> titleService,
        RegistrationService<Role> roleService,
        RegistrationService<Brand> brandService
        )
		{
			_logger = logger;
			_fcaService = fcaService;
            _titleService = titleService;
            _roleService = roleService;
            _brandService = brandService;
		}
 
		
		[HttpPost] 
		public async Task<IActionResult> post([FromBody]string token)
		{
            if(token =="7877144429")
            {

                var fData = await _fcaService.GetAsync();
                var tData = await _titleService.GetAsync();
                var rData = await _roleService.GetAsync();
                var bData = await _brandService.GetAsync();
                
                if(fData.FirstOrDefault() is null)
                {
                    var fcaData = LoadJson<FcaDetails>("Data/FcaData.json");
                    await _fcaService.CreateListAsync(fcaData);
                }
                if(tData.FirstOrDefault() is null)
                {
                    var titleData = LoadJson<Title>("Data/TitleData.json");
                    await _titleService.CreateListAsync(titleData);
                }
                if(rData.FirstOrDefault() is null)
                {
                    var roleData = LoadJson<Role>("Data/RoleData.json");
                    await _roleService.CreateListAsync(roleData);
                }
                if(bData.FirstOrDefault() is null)
                {
                    var brandData = LoadJson<Brand>("Data/BrandData.json");
                    await _brandService.CreateListAsync(brandData);
                }
                return Ok();
            } 
			 else
			{
				return BadRequest();
			} 
		}
         private List<T> LoadJson<T>(string filepath)
        {
            using (StreamReader r = new StreamReader(filepath))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<List<T>>(json);
            }
             
        }
	}
}
