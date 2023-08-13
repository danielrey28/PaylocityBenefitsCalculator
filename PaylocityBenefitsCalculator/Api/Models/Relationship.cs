using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public enum Relationship
{
    None,
    Spouse,
    [Display(Name = "Domestic Partner")]
    DomesticPartner,
    Child
}

