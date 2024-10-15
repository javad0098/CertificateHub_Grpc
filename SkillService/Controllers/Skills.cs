using System;
using System.Collections.Generic;
using AutoMapper;
using SkillService.Data;
using SkillService.Dtos;
using SkillService.Models;
using Microsoft.AspNetCore.Mvc;

namespace SkillService.Controllers
{
    [Route("api/s/certificates/{certificateId}/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillRepo _repository;
        private readonly IMapper _mapper;

        public SkillsController(ISkillRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SkillReadDto>> GetSkillsForCertificate(int certificateId)
        {
            Console.WriteLine($"--> Hit GetSkillssForCertificate: {certificateId}");

            if (!_repository.CertificateExits(certificateId))
            {
                return NotFound();
            }

            var skills = _repository.GetSkillForCertificate(certificateId);

            return Ok(_mapper.Map<IEnumerable<SkillReadDto>>(skills));
        }

        [HttpGet("{skillId}", Name = "GetSkillForCertificate")]
        public ActionResult<SkillReadDto> GetSkillForCertificate(int certificateId, int skillId)
        {
            Console.WriteLine($"--> Hit GetSkillForCertificate: {certificateId} / {skillId}");

            if (!_repository.CertificateExits(certificateId))
            {
                return NotFound();
            }

            var skill = _repository.GetSkill(certificateId, skillId);

            if (skill == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<SkillReadDto>(skill));
        }

        [HttpPost]
        public ActionResult<SkillReadDto> CreateSkillForCertificate(int certificateId, SkillCreateDto skillDto)
        {
            Console.WriteLine($"--> Hit CreateSkillForCertificate: {certificateId}");

            if (!_repository.CertificateExits(certificateId))
            {
                return NotFound();
            }

            var skill = _mapper.Map<Skill>(skillDto);

            _repository.CreateSkill(certificateId, skill);

            _repository.SaveChanges();

            var skillReadDto = _mapper.Map<SkillReadDto>(skill);

            return CreatedAtRoute(nameof(GetSkillForCertificate),
                new { certificateId = certificateId, skillId = skillReadDto.Id }, skillReadDto);
        }
    }
}