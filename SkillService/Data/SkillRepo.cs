using System.Runtime.ConstrainedExecution;
using SkillService.Models;

namespace SkillService.Data
{
    public class SkillRepo : ISkillRepo
    {
        private readonly AppDbContext _context;
        public SkillRepo(AppDbContext context)
        {
            _context = context;

        }

        public bool CertificateExits(int certificateId)
        {
            return _context.Certificates.Any(p => p.Id == certificateId);

        }

        public void CreateCertificate(Certificate cer)
        {
            if (cer == null)
            {
                throw new ArgumentNullException(nameof(cer));
            }
            _context.Certificates.Add(cer);
        }

        public void CreateSkill(int certificateId, Skill skill)
        {
            if (skill == null)
            {
                throw new ArgumentNullException(nameof(skill));
            }

            skill.CertificateId = certificateId;
            _context.Skills.Add(skill);
        }

        public bool ExternalCertificateExists(int externalCertificateId)
        {
            return _context.Certificates.Any(p => p.ExternalID == externalCertificateId);
        }


        public IEnumerable<Certificate> GetAllCertificates()
        {
            return _context.Certificates.ToList();
        }

        public Skill GetSkill(int certificateId, int skillId)
        {
            return _context.Skills
                .Where(s => s.CertificateId == certificateId && s.Id == skillId).FirstOrDefault();
        }

        public IEnumerable<Skill> GetSkillForCertificate(int certificateId)
        {
            return _context.Skills
                            .Where(s => s.CertificateId == certificateId)
                            .OrderBy(s => s.Name);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}