using Api.Models;

namespace Api.Dtos.Dependent;

//Removed the Dependent Model as it was redundant. 
public class GetDependentDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Relationship Relationship { get; set; }
}
