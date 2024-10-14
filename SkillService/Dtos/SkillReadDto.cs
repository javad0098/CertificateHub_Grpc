namespace SkillService.Dtos
{
    public class SkillReadDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CertificateId { get; set; }

        public bool IsPrimarySkill { get; set; }

        // Add ProficiencyLevel from the Skill model
        public string ProficiencyLevel { get; set; }

        // Add Tags from the Skill model
        public List<string> Tags { get; set; }
    }
}
