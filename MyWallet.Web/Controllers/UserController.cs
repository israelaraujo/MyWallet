﻿using MyWallet.Data.Domain;
using MyWallet.Data.Repository;
using MyWallet.Web.Util;
using MyWallet.Web.ViewModels.User;
using System;
using System.IO;
using System.Web.Mvc;

namespace MyWallet.Web.Controllers
{
    public class UserController : BaseController
    {
        private UnitOfWork _unitOfWork;

        public UserController()
        {
            _unitOfWork = new UnitOfWork();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                SendModelStateErrors();
                return View(userViewModel);
            }

            User user;
            Context mainContext;
            
            SaveNewUser(userViewModel, out user, out mainContext);

            // Login into plataform - bacause of the Autorization (attribute)
            CookieUtil.SetAuthCookie(user.Id, user.Name, mainContext.Id);

            return RedirectToAction("Index", "Dashboard");
        }

        private void SaveNewUser(UserViewModel userViewModel, out User user, out Context mainContext)
        {
            user = new User();
            user.Name = userViewModel.Name;
            user.LastName = userViewModel.LastName;
            user.Email = userViewModel.Email;
            user.Password = CryptographyUtil.Encrypt(userViewModel.Password);
            user.CreationDate = DateTime.Now;

            _unitOfWork.UserRepository.Save(user);

            mainContext = new Context
            {
                UserId = user.Id,
                IsMainContext = true,
                Name = "My Finances (Default)",
                CountryId = "1", //TODO implement
                CurrencyTypeId = "1"
            };
            _unitOfWork.ContextRepository.Save(mainContext);

            var categories = _unitOfWork.CategoryRepository.GetStandardCategories();
            foreach (var category in categories)
            {
                category.ContextId = mainContext.Id;
                _unitOfWork.CategoryRepository.Save(category);
            }

            var mainBankAccount = new BankAccount
            {
                ContextId = mainContext.Id,
                Name = "My Bank Account (Default)",
                CreationDate = DateTime.Now,
            };
            _unitOfWork.BankAccountRepository.Save(mainBankAccount);

            _unitOfWork.Commit();
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        public ActionResult Edit()
        {
            var user = _unitOfWork.UserRepository.GetById(GetCurrentUserId());

            var viewModel = new UserViewModel()
            {
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email
            };

            if (user.Photo != null)
                viewModel.PhotoBase64 = "data:image/jpeg;base64," + Convert.ToBase64String(user.Photo);
            else
                viewModel.PhotoBase64 = "/Content/Img/img_avatar.png";

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(UserViewModel userViewModel) 
        {
            ModelState.Remove("Password");
            ModelState.Remove("RepeatPassword");

            if (ModelState.IsValid)
            {
                var user = _unitOfWork.UserRepository.GetById(GetCurrentUserId());

                if (userViewModel.NewPhoto != null)
                {
                    byte[] photo = null;
                    using (var memoryStream = new MemoryStream())
                    {
                        userViewModel.NewPhoto.InputStream.CopyTo(memoryStream);
                        photo = memoryStream.ToArray();
                    }
                    user.Photo = photo;
                }

                user.Name = userViewModel.Name;
                user.LastName = userViewModel.LastName;
                user.Email = userViewModel.Email;
                user.Password = CryptographyUtil.Encrypt(userViewModel.Password);

                _unitOfWork.UserRepository.Save(user);
                _unitOfWork.Commit();

                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                SendModelStateErrors();
                return View(userViewModel);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}