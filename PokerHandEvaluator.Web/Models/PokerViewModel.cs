using System.ComponentModel.DataAnnotations;
using PokerHandEvaluator.Core;

namespace PokerHandEvaluator.Web.Models;

public class PokerViewModel
{
    [Required]
    [Display(Name = "Poker Hand")]
    public string? Hand { get; set; }
    public string? Result { get; set; }
}
