namespace Tutorial8.Models.DTOs;

public class ClientDto
{
    public int? Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? Telephone { get; set; }
    public string? Pesel { get; set; }
}