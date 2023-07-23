using System.ComponentModel.DataAnnotations;

namespace RestfullWeb.Model;

public partial class Company
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = null!;
}