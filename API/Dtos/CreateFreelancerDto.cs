using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class CreateFreelancerDto
    {
        [Required]
        [StringLength(30)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public List<SkillDto> Skills { get; set; } = new();
        public List<HobbyDto> Hobbies { get; set; } = new();
    }
}
