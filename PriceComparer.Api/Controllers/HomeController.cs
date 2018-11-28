namespace PriceComparer.Api.Controllers
{
    using System.Reflection;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var response = new
            {
                Version = $"{version.Major}.{version.Minor}.{version.Build}",
                Name = "Komplett.ServicePartners.Info.Api"
            };

            return Ok(response);
        }
    }
}
