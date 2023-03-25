using Contracts;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace AccountOwnerServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILoggerManager _logger;
        private  IRepositoryWrapper _repository;

        public WeatherForecastController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogInfo("Here is Info message from the controller.");
            _logger.LogDebug("Here is debug message from the controller.");
            _logger.LogWarn("Here is warn message from the controller.");
            _logger.LogError("Here is error message from the controller.");

            var domesticAccounts = _repository.Account.FindByCondition(a => a.AccountType.Equals("Domestic")).ToList();
            var owners = _repository.Owner.FindAll().ToList();

            return new string[] { "value1", "value2" };

            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //})
            //.ToArray();
        }
    }
}