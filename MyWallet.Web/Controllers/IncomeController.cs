using MyWallet.Data.Domain;
using MyWallet.Data.Repository;
using MyWallet.Web.ViewModels.Income;
using System;
using System.Net;
using System.Web.Mvc;

namespace MyWallet.Web.Controllers
{
    public class IncomeController : BaseController
    {
        private UnitOfWork _unitOfWork;

        public IncomeController()
        {
            _unitOfWork = new UnitOfWork();
        }

        // GET: Income
        public ActionResult Index()
        {
            var contextId = GetCurrentContextId();

            var incomeList = _unitOfWork.IncomeRepository.GetByContextId(contextId);
            var viewModelList = new ListAllIncomeViewModel();
            viewModelList.Currency = _unitOfWork.CurrencyTypeRepository.GetCurrencySymbolByContextId(contextId);

            foreach (var income in incomeList)
            {
                var viewModel = new IncomeViewModel()
                {
                    Id = income.Id,
                    Description = income.Description,
                    CategoryId = income.CategoryId,
                    Received = income.Received,
                    BankAccountId = income.BankAccountId,
                    Value = income.Value,
                    Date = income.Date,
                    Observation = income.Observation,
                    BankAccount = income.BankAccount.Name,
                    Category = income.Category.Name,
                };

                viewModelList.IncomeList.Add(viewModel);
            }

            return View(viewModelList);
        }

        public ActionResult Create(IncomeViewModel viewModel)
        {
            var income = new Income()
            {
                BankAccountId = viewModel.BankAccountId,
                CategoryId = viewModel.CategoryId,
                ContextId = GetCurrentContextId(),
                CreationDate = DateTime.Now,
                Date = viewModel.Date.Value,
                Description = viewModel.Description,
                Observation = viewModel.Observation,
                Received = viewModel.Received,
                Value = viewModel.Value.Value
            };
            _unitOfWork.IncomeRepository.Save(income);
            _unitOfWork.Commit();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public HttpStatusCodeResult Delete(string id)
        {
            _unitOfWork.IncomeRepository.Delete(id);
            _unitOfWork.Commit();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public PartialViewResult GetByContext()
        {
            var incomeList = _unitOfWork.IncomeRepository.GetByContextId(GetCurrentContextId());

            var viewModelList = new ListAllIncomeViewModel();
            viewModelList.Currency = "€";

            foreach (var item in incomeList)
            {
                var income = new IncomeViewModel
                {
                    Id = item.Id,
                    Description = item.Description,
                    Value = item.Value,
                    Date = item.Date,
                    Received = item.Received,
                    BankAccount = item.BankAccount.Name,
                    Category = item.Category.Name
                };

                viewModelList.IncomeList.Add(income);
            }
            return PartialView("PartialView/_IncomeList",viewModelList);
        }

        public PartialViewResult GetIncomeById(string id)
        {
            var income = _unitOfWork.IncomeRepository.GetById(id);
            var incomeViewModel = new IncomeViewModel
            {
                Id = income.Id,
                Description = income.Description,
                Value = income.Value,
                Date = income.Date,
                Received = income.Received,
                BankAccountId = income.BankAccountId,
                CategoryId = income.CategoryId
            };

            var bankAccounts = _unitOfWork.BankAccountRepository.GetByContextId(income.ContextId);
            foreach (var item in bankAccounts)
            {
                incomeViewModel.SelectListBankAccount.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }

            var categories = _unitOfWork.CategoryRepository.GetByContextId(income.ContextId);
            foreach (var item in categories)
            {
                incomeViewModel.SelectListCategory.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });
            }

            return PartialView("PartialView/_IncomeEditFields", incomeViewModel);
        }

        [HttpPost]
        public HttpStatusCodeResult Edit(IncomeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var income = _unitOfWork.IncomeRepository.GetById(viewModel.Id);
                income.Date = viewModel.Date.Value;
                income.Description = viewModel.Description;
                income.CategoryId = viewModel.CategoryId;
                income.BankAccountId = viewModel.BankAccountId;
                income.Value = viewModel.Value.Value;
                income.Received = viewModel.Received;

                _unitOfWork.IncomeRepository.Save(income);
                _unitOfWork.Commit();

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}