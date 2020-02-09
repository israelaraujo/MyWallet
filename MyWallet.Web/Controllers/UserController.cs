﻿using MyWallet.Data.Domain;
using MyWallet.Data.Repository;
using MyWallet.Service;
using MyWallet.Web.Util;
using MyWallet.Web.ViewModels.User;
using System;
using System.IO;
using System.Web.Mvc;

namespace MyWallet.Web.Controllers
{
    public class UserController : BaseController
    {
        private UserService _userService;
        private UnitOfWork _unitOfWork;


        public UserController()
        {
            _userService = new UserService();
            _unitOfWork = new UnitOfWork();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = userViewModel.Name,
                    LastName = userViewModel.LastName,
                    Email = userViewModel.Email,
                    Password = userViewModel.Password,
                    CreationDate = DateTime.Now
                };

                var mainContext = new Context
                {
                    UserId = user.Id,
                    IsMainContext = true,
                    Name = "My Finances (Default)",
                    CountryId = 1, //TODO implement
                    CurrencyTypeId = 1
                };


                var categories = _unitOfWork.CategoryRepository.GetStandardCategories();
                foreach (var category in categories)
                {
                    category.ContextId = mainContext.Id;
                }

                var mainBankAccount = new BankAccount
                {
                    ContextId = mainContext.Id,
                    Name = "My Bank Account (Default)",
                    CreationDate = DateTime.Now,
                };

                _unitOfWork.UserRepository.Add(user);
                _unitOfWork.ContextRepository.Add(mainContext);
                _unitOfWork.CategoryRepository.Add(categories);
                _unitOfWork.BankAccountRepository.Add(mainBankAccount);
                _unitOfWork.Commit();

                // Login into plataform - bacause of the Autorization (attribute)
                CookieUtil.SetAuthCookie(user.Id, user.Name, user.GetTheMainContextId());

                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                SendModelStateErrors();
                return View(userViewModel);
            }
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        public ActionResult Edit()
        {
            var user = _userService.GetById(GetCurrentUserId());
            var viewModel = new UserViewModel()
            {
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                byte[] photo = null;
                using (var memoryStream = new MemoryStream())
                {
                    userViewModel.Photo.InputStream.CopyTo(memoryStream);
                    photo = memoryStream.ToArray();
                }

                var oldUser = _userService.GetById(GetCurrentUserId());

                var updateUser = new User()
                {
                    Id = GetCurrentUserId(),
                    Name = userViewModel.Name,
                    LastName = userViewModel.LastName,
                    CreationDate = oldUser.CreationDate,
                    Email = userViewModel.Email,
                    Password = userViewModel.Password,
                    Photo = photo
                };
                _userService.Update(updateUser);

                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                SendModelStateErrors();
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}