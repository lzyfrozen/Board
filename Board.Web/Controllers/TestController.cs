using Board.Application;
using Board.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace Board.Web.Controllers
{
    public class TestController : BaseController
    {
        private readonly ILogger<TestController> _logger;
        private readonly ITestService _testService;
        public TestController(ILogger<TestController> logger, ITestService testService)
        {
            _logger = logger;
            _testService = testService;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("GetHello")]
        public ApiResult<dynamic> GetHello()
        {
            var data = _testService.Hello();
            var result = new ApiResult<dynamic>()
            {
                Result = true,
                Data = data
            };
            return result;
        }

        [HttpGet]
        [Route("GetTestData")]
        public ApiResult<dynamic> GetTestData()
        {
            var data = _testService.GetLocaltionList();
            var result = new ApiResult<dynamic>()
            {
                Result = true,
                Data = data
            };
            return result;
        }
    }
}
