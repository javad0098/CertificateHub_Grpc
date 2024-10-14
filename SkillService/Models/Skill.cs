using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SkillService.Models
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(300)]
        public string Description { get; set; }

        // Level of Expertise, for example: Beginner, Intermediate, Expert
        [Required]
        public int CertificateId { get; set; }


        [StringLength(50)]
        public Proficiency ProficiencyLevel { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        public bool IsPrimarySkill { get; set; }
        public Certificate Certificate { get; set; }

    }
    public enum Proficiency
    {
        Beginner,
        Intermediate,
        Expert
    }
}
