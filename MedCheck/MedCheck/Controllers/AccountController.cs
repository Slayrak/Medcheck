using MedCheck.Configuration;
using MedCheck.Models;
using MedCheck.Models.ViewModels;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MedCheck.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<MainUser> _userManager;
        private readonly SignInManager<MainUser> _signInManager;
        private readonly EmailConfig _emailConfig;

        public AccountController(UserManager<MainUser> userManager, SignInManager<MainUser> signInManager, IOptions<EmailConfig> opts)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailConfig = opts.Value;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/Account/Signup")
        {
            return View(new LoginModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                MainUser user = await _userManager.FindByNameAsync(model.Email);

                if (user != null)
                {
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "You did not verify email");
                        return View(model);
                    }

                    await _signInManager.SignOutAsync();

                    var result =
                        await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        // check if url belongs to app
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Wrong password or email");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid name or password");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Signup(SignupModel model)
        {
            if (ModelState.IsValid)
            {
                MainUser user = new MainUser { Name = model.Name, FamilyName = model.FamilyName, Email = model.Email, UserName = model.Email };
                // add user
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token }, Request.Scheme);


                    SendEmailConfirmationEmail(
                            $"Dear {user.Name} {user.FamilyName},\nHere is the link to confirm your email: {confirmationLink}",
                            user.Email);
                    // installing cookies
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }


        private void SendEmailConfirmationEmail(string message, string to)
        {
            using var mailMessage = new MailMessage(_emailConfig.Address, to)
            {
                Subject = "Email Confirmation",
                Body = message,
            };

            SendEmail(mailMessage);
        }

        private void SendEmail(MailMessage mailMessage)
        {
            var smtp = new SmtpClient(_emailConfig.Host, _emailConfig.Port)
            {
                Credentials = new NetworkCredential()
                {
                    UserName = _emailConfig.Address,
                    Password = _emailConfig.Password,
                },

                EnableSsl = true,
            };

            smtp.Send(mailMessage);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId is null || token is null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }

            if ((await _userManager.ConfirmEmailAsync(user, token)).Succeeded)
            {
                return View();
            }

            return View("Error");
        }
    }
}
