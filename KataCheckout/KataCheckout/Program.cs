using KataCheckout.Controller;
using KataCheckout.Services;
using KataCheckout.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddSingleton<IStockKeepingUnitService, StockKeepingUnitService>();
services.AddSingleton<IPriceService, PriceService>();

services.AddTransient<CheckoutController>();
var provider = services.BuildServiceProvider();

var controller = provider.GetRequiredService<CheckoutController>();
controller.ShowMenu();
