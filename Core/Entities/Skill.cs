namespace Core.Entities;

public class Skill
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int FreelancerId { get; set; }

    public Freelancer? Freelancer { get; set; }
}
