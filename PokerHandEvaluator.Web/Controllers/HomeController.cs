using Microsoft.AspNetCore.Mvc;
using PokerHandEvaluator.Core;
using PokerHandEvaluator.Web.Models;

namespace PokerHandEvaluator.Web.Controllers;

public class HomeController : Controller
{
    private readonly HandEvaluator _evaluator;
    public HomeController(HandEvaluator evaluator) => _evaluator = evaluator;

    [HttpGet]
    public IActionResult Index() => View(new PokerViewModel());

    [HttpPost]
    public IActionResult Index(PokerViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            var eval = _evaluator.Evaluate(model.Hand!);
            model.Result = eval.Description;
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("Hand", ex.Message);
        }
        return View(model);
    }
}
