using Microsoft.AspNetCore.Mvc;

namespace SkillService.Controllers
{
    [ApiController]
    [Route("api/s/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var forecast = new
            {
                Date = DateTime.Now,
                TemperatureC = 25,
                Summary = "Sunny and in skill service"
            };

            return Ok(forecast);
        }
    }
}
