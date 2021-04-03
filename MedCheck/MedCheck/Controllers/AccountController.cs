using MedCheck.Configuration;
using MedCheck.Models;
using MedCheck.Models.ViewModels;

using Microsoft.AspNetCore.Authorization;
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
                    return Content("Check Email plz");
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);

                    SendPasswordResetEmail(
                        $"Dear {user.UserName},\nHere is the link to reset your password: {passwordResetLink}",
                        user.Email);

                    return View("ForgotPasswordConfirmation");
                }

                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        private void SendPasswordResetEmail(string message, string to)
        {
            using var mailMessage = new MailMessage(_emailConfig.Address, to)
            {
                Subject = "Password reset",
                Body = message,
            };

            SendEmail(mailMessage);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token is null || email is null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }

                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }

                    return View("ResetPasswordConfirmation");
                }
            }

            return View(model);
        }
    }
}
