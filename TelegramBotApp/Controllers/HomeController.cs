﻿using System.Web.Mvc;

namespace TelegramBotApp.Controllers
{
    public class HomeController : Controller
    {
        public string Index()
        {
            return "Bot was started!";
        }
    }
}