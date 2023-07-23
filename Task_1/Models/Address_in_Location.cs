using System.ComponentModel.DataAnnotations;

namespace RestfullWeb.Model;

public partial class Address_in_Location
{
    [Required(ErrorMessage = "Fias House Code is required")]
    public string FiasHouseCode { get; set; } = null!;

    [Required(ErrorMessage = "Location Id is required")]
    public int? LocationId { get; set; }

    public virtual Location? Location { get; set; }
}