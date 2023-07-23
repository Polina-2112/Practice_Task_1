using System.ComponentModel.DataAnnotations;

namespace RestfullWeb.Model;

public partial class Location
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = null!;

    public int? CompanyId { get; set; }

    public virtual Company? Company { get; set; }
}