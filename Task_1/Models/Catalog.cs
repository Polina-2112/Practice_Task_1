using System.ComponentModel.DataAnnotations;
namespace RestfullWeb;

public partial class Catalog
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = null!;
    public int MinOrder { get; set; } 
    public int MaxOrder { get; set; }
    public string measurement { get; set; } = null!;
    public int? CompanyId { get; set; }
}