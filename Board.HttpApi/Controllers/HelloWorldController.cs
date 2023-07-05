using Board.Application;
using Board.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board.HttpApi.Controllers
{
    /// <summary>
    /// HelloWorldController
    /// </summary>

    public class HelloWorldController : BaseController
    {
        private readonly ILogger<HelloWorldController> _logger;
        private readonly ITestService _testService;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="testService"></param>
        public HelloWorldController(ILogger<HelloWorldController> logger,
                                    ITestService testService)
        {
            _logger = logger;
            _testService = testService;
        }

        /// <summary>
        /// Hello
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Hello")]
        [Obsolete]
        public string Hello()
        {
            return _testService.Hello();
        }

        /// <summary>
        /// TestDB
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("TestDB")]
        public async Task<ApiResult<dynamic>> TestDB()
        {
            var list = await _testService.TestDB();

            return new ApiResult<dynamic>()
            {
                Result = true,
                Data = new
                {
                    List = list,
                    Data = DateTime.Now.ToString()
                }
            };
        }

        /// <summary>
        /// TestDB2
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("TestDB2")]
        public async Task<ApiResult<dynamic>> TestDB2()
        {
            var list = await _testService.TestDB2();

            return new ApiResult<dynamic>()
            {
                Result = true,
                Data = new
                {
                    List = list,
                    Data = DateTime.Now.ToString()
                }
            };
        }

        /// <summary>
        /// TestDatahub
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("TestDatahub")]
        public async Task<dynamic> TestDatahub()
        {
            //{ "result":true,"message":""}
            var str_json = "{\"data\":[{\"workShop\":\"T31仓库\",\"lineNo\":\"T31-01\",\"d40000\":\"36.1\",\"d40001\":\"16.7\",\"d40000_UL\":\"75\",\"d40001_LL\":\"5\",\"d40001_UL\":\"35\",\"createDate\":\"2023-03-17 14:32:18\"},{\"workShop\":\"T31仓库\",\"lineNo\":\"T31-02\",\"d40000\":\"40.8\",\"d40001\":\"17.9\",\"d40000_UL\":\"75\",\"d40001_LL\":\"5\",\"d40001_UL\":\"35\",\"createDate\":\"2023-03-17 15:23:45\"},{\"workShop\":\"95仓库\",\"lineNo\":\"95B-01\",\"d40000\":\"55.5\",\"d40001\":\"12.7\",\"d40000_UL\":\"75\",\"d40001_LL\":\"5\",\"d40001_UL\":\"35\",\"createDate\":\"2023-03-17 14:31:48\"},{\"workShop\":\"95仓库\",\"lineNo\":\"95C-02\",\"d40000\":\"53.3\",\"d40001\":\"12.8\",\"d40000_UL\":\"75\",\"d40001_LL\":\"5\",\"d40001_UL\":\"35\",\"createDate\":\"2023-03-17 14:32:08\"},{\"workShop\":\"95仓库\",\"lineNo\":\"95A-01\",\"d40000\":\"50.4\",\"d40001\":\"12.5\",\"d40000_UL\":\"75\",\"d40001_LL\":\"5\",\"d40001_UL\":\"35\",\"createDate\":\"2023-03-17 14:32:28\"},{\"workShop\":\"95仓库\",\"lineNo\":\"95B-02\",\"d40000\":\"51.1\",\"d40001\":\"13.1\",\"d40000_UL\":\"75\",\"d40001_LL\":\"5\",\"d40001_UL\":\"35\",\"createDate\":\"2023-03-17 14:31:38\"},{\"workShop\":\"95仓库\",\"lineNo\":\"95C-01\",\"d40000\":\"52.5\",\"d40001\":\"13.2\",\"d40000_UL\":\"75\",\"d40001_LL\":\"5\",\"d40001_UL\":\"35\",\"createDate\":\"2023-03-17 14:31:28\"},{\"workShop\":\"95仓库\",\"lineNo\":\"95A-02\",\"d40000\":\"52.2\",\"d40001\":\"12.4\",\"d40000_UL\":\"75\",\"d40001_LL\":\"5\",\"d40001_UL\":\"35\",\"createDate\":\"2023-03-17 14:32:38\"}],\"warehouseId\":\"JM01\",\"code\":0,\"status\":true,\"msg\":\"操作成功！\"}";

            //var json = JsonConvert.DeserializeObject(str_json);

           
            var result = await Task.Run(() =>
            {
                return str_json;
                //return new
                //{
                //    result = false,
                //    message = "",
                //    udf01 = "Y"
                //};
            });
            return result;

            //return new ApiResult<dynamic>()
            //{
            //    Result = true,
            //    Data = new
            //    {
            //        List = list,
            //        Data = DateTime.Now.ToString()
            //    }
            //};
        }
    }
}
