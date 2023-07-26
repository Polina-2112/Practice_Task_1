using System.ComponentModel.DataAnnotations;

namespace RestfullWeb.Model;

public partial class Location
{
    public int id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string name { get; set; } = null!;

    public int? company_id { get; set; }

    public virtual Company? company { get; set; }
}