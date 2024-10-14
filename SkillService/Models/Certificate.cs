using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SkillService.Models;

namespace SkillService.Models
{
    public class Certificate
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ExternalID { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Skill> Skills { get; set; } = new List<Skill>();
    }
}