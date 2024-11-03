using Microsoft.AspNetCore.Mvc;

public class CurrencyRatesController : Controller
{
    private readonly CurrencyRateService _currencyRateService;

    public CurrencyRatesController(CurrencyRateService currencyRateService)
    {
        _currencyRateService = currencyRateService;
    }

    public async Task<IActionResult> Index()
    {
        var currencyRates = await _currencyRateService.GetCurrencyRates();
        return View(currencyRates);
    }
}