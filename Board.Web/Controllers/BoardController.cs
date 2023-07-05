using Board.Application;
using Board.Application.DataBoard.Dto;
using Board.Domain.DataBoard;
using Board.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;

namespace Board.Web.Controllers
{
    public class BoardController : BaseController
    {
        private readonly ILogger<BoardController> _logger;
        public BoardController(ILogger<BoardController> logger)
        {
            _logger = logger;
        }

        [Route("Index")]
        public IActionResult Index()
        {
            _logger.LogDebug("DataBoard-Index");
            return View();
        }

    }
}
