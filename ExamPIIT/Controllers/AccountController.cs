using ExamPIIT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace ExamPIIT.Controllers
{
    public class AccountController : Controller
    {
        private MyContext _db = new MyContext();
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            //save User
            user.Status = 0;
            var code = GenerateCode();
            user.ConfirmCode = code;
            _db.Users.Add(user);
            _db.SaveChanges();
            //code

            // Find your Account Sid and Token at twilio.com/console
            // and set the environment variables. See http://twil.io/secure
            string accountSid = "ACf2a87cf9b9b7f1a67bdc13c0bf2281ce";
            string authToken = "9e711533c743b941c8377c1809516209";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: "Verify code: " + code,
                from: new Twilio.Types.PhoneNumber("+13478365115"),
                to: new Twilio.Types.PhoneNumber(user.Phone)
            );
            return RedirectToAction("ConfirmUser", new { id = user.Id });
        }
        public ActionResult ConfirmUser(int id)
        {
            var user = _db.Users.Find(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult ConfirmUser(int id, string code)
        {
            var user = _db.Users.Find(id);
            if(!user.ConfirmCode.Equals(code))
            {
                TempData["status"] = "Wrong code";
                return View(user);
            }
            user.Status = 2;
            _db.SaveChanges();
            TempData["status"] = "verified success";
            return View(user);
        }
        //helper
        private string GenerateCode()
        {
            Random generator = new Random();
            string r = generator.Next(0, 1000000).ToString("D6");
            return r;
        }
    }
}