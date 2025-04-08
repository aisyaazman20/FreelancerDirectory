using Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Freelancer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(30)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public bool IsArchived { get; set; } = false;

    public ICollection<Skill> Skills { get; set; } = new List<Skill>();

    public ICollection<Hobby> Hobbies { get; set; } = new List<Hobby>();
}
