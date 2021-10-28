using System;
using System.Collections.Generic;
using Freelancer.mu.Classes.Notification.Sender;
using Freelancer.mu.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Freelancer.Models;
using Services.Freelancer.Services.Message;

namespace Freelancer.mu.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly INotificationSender _notificationSender;

        public MessageController(IMessageService messageService, INotificationSender notificationSender)
        {
            _messageService = messageService;
            _notificationSender = notificationSender;
        }

        public IActionResult Index(string id)
        {
            var user = JsonConvert.DeserializeObject<UserModel>(HttpContext.Session.GetString("user"));
            var lastMessage = _messageService.LoadMessages(id, user.IdUser);
            var messageViewModel = new MessageViewModel
            {
                LastMessageModel = lastMessage,
                Messages = _messageService.GetMessages(id, user.IdUser)
            };
            return View(messageViewModel);
        }

        public PartialViewResult LoadMessage(string id)
        {
            var user = JsonConvert.DeserializeObject<UserModel>(HttpContext.Session.GetString("user"));
            var messageViewModel = new MessageViewModel
            {
                Messages = _messageService.GetMessages(id, user.IdUser)
            };
            return PartialView("Partial/_MessagePartialView", messageViewModel);
        }

        [HttpPost]
        public PartialViewResult SendMessage(string id, string message)
        {
            var user = JsonConvert.DeserializeObject<UserModel>(HttpContext.Session.GetString("user"));
            bool saved = _messageService.SendMessage(id, user.IdUser, message);
            var messageViewModel = new MessageViewModel
            {
                Messages = _messageService.GetMessages(id, user.IdUser)
            };
            try
            {
                _notificationSender.ReloadMessage(id, user.IdUser);
            }
            catch
            {
                //ignored
            }
            return PartialView("Partial/_MessagePartialView", messageViewModel);
        }
    }
}