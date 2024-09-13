using HomeWork.Redis.Business.Abstractions.CreateSession;
using HomeWork.Redis.Business.Abstractions.DeleteSession;
using HomeWork.Redis.Business.Abstractions.GetSession;
using HomeWork.Redis.Domain;
using HomeWork.Redis.Domain.Dto;
using HomeWork.Redis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HomeWork.Redis.WebApp.Controllers
{
    public class SessionController : Controller
    {
        private readonly ILogger<SessionController> _logger;
        private readonly ICreateGameSessionRequest<CreateGameSessionRequestContext, RequestResult> _createGameSessionRequest;
        private readonly IGetSessionRequest<GetGameSessionRequestContext, GameSession?> _getGameSessionRequest;
        private readonly IDeleteGameSessionRequest<DeleteGameSessionRequestContext, RequestResult> _deleteGameSessionRequest;

        public SessionController(ILogger<SessionController> logger,
            ICreateGameSessionRequest<CreateGameSessionRequestContext, RequestResult> createGameSessionRequest, 
            IGetSessionRequest<GetGameSessionRequestContext, GameSession?> getGameSessionRequest,
             IDeleteGameSessionRequest<DeleteGameSessionRequestContext, RequestResult> deleteGameSessionRequest)
        {
            _logger = logger;
            _createGameSessionRequest = createGameSessionRequest;
            _getGameSessionRequest = getGameSessionRequest;
            _deleteGameSessionRequest = deleteGameSessionRequest;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGameSessionRequestContext context)
        {
            var result = await _createGameSessionRequest.HandleAsync(context);

            if (result.Code == Codes.Success)
            {
                ViewBag.Message = "Game session created successfully!";
            }
            else
            {
                ViewBag.Message = "Failed to create game session: " + result.Message;
            }

            return View("Result", result);
        }

        public IActionResult Result(RequestResult result)
        {
            return View(result);
        }

        public IActionResult Get()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Get(GetGameSessionRequestContext context)
        {
            var result = await _getGameSessionRequest.HandleAsync(context);

            return View("GetGameSessionResult", result);
        }

        public IActionResult GetGameSessionResult(GameSession? result)
        {
            return View(result);
        }

        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteGameSessionRequestContext context)
        {
            var result = await _deleteGameSessionRequest.HandleAsync(context);

            return View("DeleteGameSessionResult", result);
        }

        public IActionResult DeleteGameSessionResult(GameSession? result)
        {
            return View(result);
        }
    }
}