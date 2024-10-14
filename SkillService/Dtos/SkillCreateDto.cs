using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SkillService.Dtos
{
    public class SkillCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(300)]
        public string Description { get; set; }

        [Required]
        public Proficiency ProficiencyLevel { get; set; }

        // Tags: Optional list of strings
        public List<string> Tags { get; set; } = new List<string>();

        public bool IsPrimarySkill { get; set; }
    }

    public enum Proficiency
    {
        Beginner,
        Intermediate,
        Expert
    }
}