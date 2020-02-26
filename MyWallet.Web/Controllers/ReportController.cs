﻿using MyWallet.Data.DTO.Report;
using MyWallet.Data.Repository;
using MyWallet.Web.ViewModels.Report;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MyWallet.Web.Controllers
{
    public class ReportController : BaseController
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult Index()
        {
            var contextId = GetCurrentContextId();

            var viewModel = new ReportFilterViewModel();

            viewModel.SelectListCategory = new List<SelectListItem>();
            var categories = _unitOfWork.CategoryRepository.GetByContextId(contextId);
            foreach (var item in categories)
            {
                viewModel.SelectListCategory.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }

            viewModel.SelectListBankAccount = new List<SelectListItem>();
            var bankAccounts = _unitOfWork.BankAccountRepository.GetByContextId(contextId);
            foreach (var item in bankAccounts)
            {
                viewModel.SelectListBankAccount.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }

            viewModel.SelectListType = new List<SelectListItem>();
            viewModel.SelectListType.Add(new SelectListItem { Text = "Expense", Value = "1"});
            viewModel.SelectListType.Add(new SelectListItem { Text = "Income", Value = "2"});

            viewModel.SelectListSituation = new List<SelectListItem>();
            viewModel.SelectListSituation.Add(new SelectListItem { Text = "All", Value = null});
            viewModel.SelectListSituation.Add(new SelectListItem { Text = "Paid", Value = "1"});
            viewModel.SelectListSituation.Add(new SelectListItem { Text = "Unpaid", Value = "0"});

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(ReportFilterDTO filterDTO)
        {
            filterDTO.ContextId = GetCurrentContextId();

            IEnumerable<EntryDTO> entries = _unitOfWork.ReportRepository.GetByFilter(filterDTO);
            return RedirectToAction("Index");
        }
    }
}