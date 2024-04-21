using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MVCMessenger.Interfaces;
using MVCMessenger.Models;
using MVCMessenger.ViewModels;

namespace MVCMessenger.Controllers
{
    public class MainController : Controller
    {
        private readonly IMessengerRepository _messengerRepository;
        private MainViewModel _mainViewModel;
        private string? _userSearch;

        public MainController(IMessengerRepository messengerRepository)
        {
            _messengerRepository = messengerRepository;
            _userSearch = _mainViewModel?.UserSearch;
        }
        
        public IActionResult Index(MainViewModel mainViewModel)
        {
            if (Request.Cookies["Id"].IsNullOrEmpty())
                return RedirectToAction("Index", "Home");

            _mainViewModel = mainViewModel;
            _mainViewModel.UserId = Request.Cookies["Id"];

            if (_userSearch != mainViewModel.UserSearch)
            {
                _mainViewModel.Status = Status.Searching;
                _userSearch = mainViewModel.UserSearch;
            }

            if(!_mainViewModel.InterlocutorId.IsNullOrEmpty())
                _mainViewModel.MessagesWithUser = (List<Message>)_messengerRepository.GetMessagesAsync(_messengerRepository.GetChat(_mainViewModel.UserId, _mainViewModel.InterlocutorId).Result.Id).Result;
            if (_mainViewModel.Status == Status.Searching)
                _mainViewModel.Users = (List<User>)_messengerRepository.GetUsernamesLike(_userSearch).Result;
            else
                _mainViewModel.Users = (List<User>)_messengerRepository.GetChattingUsersAsync(_mainViewModel.UserId).Result;

            _mainViewModel.AllUsers = (List<User>)_messengerRepository.GetUsersAsync().Result;
            return View(_mainViewModel);
        }

        [HttpPost]
        public IActionResult SendMessage(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _messengerRepository.SendMessageAsync(mainViewModel.MessageText, mainViewModel.UserId,
                mainViewModel.InterlocutorId);
            _mainViewModel.MessageText = String.Empty;
            return RedirectToAction("Index", "Main", _mainViewModel);
        }
        [HttpPost]
        public IActionResult OpenOrCreateChatWith(int interlocutorId, MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _mainViewModel.InterlocutorId = interlocutorId.ToString();
            _mainViewModel.UserId = Request.Cookies["Id"];
            Chat? chat = _messengerRepository.GetChat(_mainViewModel.UserId, _mainViewModel.InterlocutorId).Result;
            if (chat == null)
            {
                _messengerRepository.CreateChat(_mainViewModel.UserId, _mainViewModel.InterlocutorId);
            }
            _mainViewModel.Status = Status.OnChat;
            return RedirectToAction("Index", "Main", _mainViewModel);
        }
    }
}
