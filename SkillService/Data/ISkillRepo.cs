using System.Collections.Generic;
using SkillService.Models;

namespace SkillService.Data
{
    public interface ISkillRepo
    {
        bool SaveChanges();

        // Certificates
        IEnumerable<Certificate> GetAllCertificates();
        void CreateCertificate(Certificate cer);
        bool CertificateExits(int certificateId);
        bool ExternalCertificateExists(int externalCertificateId);
        // skills
        IEnumerable<Skill> GetSkillForCertificate(int certificateId);
        Skill GetSkill(int certificateId, int skillId);
        void CreateSkill(int certificateId, Skill skill);
    }
}