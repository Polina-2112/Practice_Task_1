using System.ComponentModel.DataAnnotations;

namespace RestfullWeb.Model;

public partial class Address_in_Location
{
    [Required(ErrorMessage = "Fias House Code is required")]
    public string fias_house_code { get; set; } = null!;

    [Required(ErrorMessage = "Location Id is required")]
    public int? location_id { get; set; }

    public virtual Location? location { get; set; }
}