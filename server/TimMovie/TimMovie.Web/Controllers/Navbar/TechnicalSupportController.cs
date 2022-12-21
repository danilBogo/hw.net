using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Const;
using TimMovie.Core.DTO.Messages;
using TimMovie.Core.Services.Messages;

namespace TimMovie.Web.Controllers.Navbar;

public class TechnicalSupportController: Controller
{
    private readonly MessageService _messageService;
    
    public TechnicalSupportController(MessageService messageService)
    {
        _messageService = messageService;
    }
    
    [Authorize(Roles = RoleNames.Support)]
    [HttpGet]
    public IActionResult Support()
    {
        return View("~/Views/Navbar/TechnicalSupport/Support.cshtml");
    }
    
    public IActionResult MessageForSupportPage(MessageDto message)
    {
        return View("~/Views/Navbar/TechnicalSupport/Message.cshtml", message);
    }
    
    public IActionResult MessageForUserChat(MessageDto message)
    {
        return View("~/Views/Support/ModalWindowChat/Message.cshtml", message);
    }

    [HttpGet]
    public IActionResult NotificationForSupportPage(string content)
    {
        return View("~/Views/Navbar/TechnicalSupport/NotificationInChat.cshtml", content);
    }
    
    public IActionResult NotificationForUserChat(string content)
    {
        return View("~/Views/Support/ModalWindowChat/Notification.cshtml", content);
    }
    
    [HttpGet]
    public IActionResult GetAllUserMessages()
    {
        var groupName = User.Identity?.Name;

        var messages = _messageService.GetAllMessagesByGroup(groupName);
        
        return View("~/Views/Support/ModalWindowChat/Messages.cshtml", messages);
    }
    
    [HttpGet]
    public IActionResult GetAllGroupMessagesForSupportPage(string groupName)
    {
        var messages = _messageService.GetAllMessagesByGroup(groupName);
        
        return View("~/Views/Navbar/TechnicalSupport/Messages.cshtml", messages);
    }
}