using KataCheckout.Controller;
using KataCheckout.Services;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddSingleton<StockKeepingUnitService>();
services.AddTransient<CheckoutController>();
var provider = services.BuildServiceProvider();

var controller = provider.GetRequiredService<CheckoutController>();
controller.ShowMenu();
