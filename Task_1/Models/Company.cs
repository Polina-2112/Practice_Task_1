using System.ComponentModel.DataAnnotations;

namespace RestfullWeb.Model;

public partial class Company
{
    public int id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string name { get; set; } = null!;
}