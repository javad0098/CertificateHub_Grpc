using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SkillService.Data;
using SkillService.Dtos;
using System;
namespace SkillService.Controllers
{
    [ApiController]
    [Route("api/s/[controller]")]
    public class CertificatesController : ControllerBase
    {
        private readonly ISkillRepo _repository;
        private readonly IMapper _mapper;

        public CertificatesController(ISkillRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CertificateReadDto>> GetCertificates()
        {
            Console.WriteLine("--> Getting certificate from skillService");

            var certificateItems = _repository.GetAllCertificates();

            return Ok(_mapper.Map<IEnumerable<CertificateReadDto>>(certificateItems));
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult TestInboundConnection()
        {
            Console.WriteLine("==> Inbound post # Skill Services ");
            return Ok("Inbound test of from Certificate controller");
        }
    }
}
