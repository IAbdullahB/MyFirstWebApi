using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MyFirstWebAPI.Controllers
{
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        // IOptions VS IOptionsSnapshot:
        // IOptionsSnapshot is a scoped service, which means it is created once per request.
        // It allows you to retrieve configuration values that can change during the lifetime of the application,
        // such as when you modify the configuration file while the application is running.
        // This is useful for scenarios where you want to read updated configuration values without restarting the application.
        private readonly IOptionsSnapshot<AttachmentOptions> _attachmentOptions;


        public ConfigController(IConfiguration configuration, IOptionsSnapshot<AttachmentOptions> attachmentOptions)
        {
            _configuration = configuration;
            _attachmentOptions = attachmentOptions;
        }

        [HttpGet]
        [Route("[controller]")]

        public ActionResult GetConfigValue()
        {
            var config = new
            {
                AllowedHosts = _configuration["AllowedHosts"],
                DefaultConnection = _configuration["ConnectionStrings:DefaultConnection"], // nested configuration value
                DefaultLogLevel = _configuration["Logging:LogLevel:Default"],
                TestKey = _configuration["TestKey"], // from the custom configuration provider
                AttachmentsOptions = _attachmentOptions.Value 

            };

            return Ok(config);
        }
    }
}
