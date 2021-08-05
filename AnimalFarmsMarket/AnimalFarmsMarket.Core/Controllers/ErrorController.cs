using AnimalFarmsMarket.Commons;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AnimalFarmsMarket.Core
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }
        [Route("/Error/{statusCode}")]
        public IActionResult PageNotFoundHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    var statusDetails = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
                    var path = statusDetails.OriginalPath;
                    var qString = statusDetails.OriginalQueryString;
                    // Todo: log error to file
                    _logger.LogError(path, qString);
                    break;

                case 401:
                    var statusDetail = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
                    var paths = statusDetail.OriginalPath;
                    var currentPath = UrlHelper.CreateUrl("Account/Login", HttpContext);
                    var returnUrl = $"{currentPath}?returnUrl={paths}";
                    return Redirect(returnUrl);

                    
                    

            }
            return View("NotFound");
        }


        [Route("/Error")]
        public IActionResult ExceptionHandler(int statusCode)
        {
            var errorDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var path = errorDetails.Path;
            var err = errorDetails.Error;
            // Todo: log to file
            _logger.LogError(err, path);
            return View("Error");
        }

        public IActionResult AccessDeniedHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    var statusDetails = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
                    var path = statusDetails.OriginalPath;
                    var qString = statusDetails.OriginalQueryString;
                    // Todo: log error to file
                    _logger.LogError(path, qString);
                    break;
            }
            return View("AccessDenied");
        }
    }
}
