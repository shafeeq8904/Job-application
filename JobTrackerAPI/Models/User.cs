

namespace JobTrackerAPI.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
}
