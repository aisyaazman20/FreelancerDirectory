namespace Core.Entities;

public class Hobby
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int FreelancerId { get; set; } //foreign key

    public Freelancer? Freelancer { get; set; } //nav property
}
