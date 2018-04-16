using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransactionsManager.Domain;
using TransactionsManager.Services;
using TransactionsManager.Services.Interfaces;
using TransactionsManager.UI.Models;
using X.PagedList;

namespace TransactionsManager.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ITransactionsService _transactionsService;

        public HomeController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }

        public async Task<IActionResult> Index(bool? isFraud, string searchNameDest)
        {
            ViewBag.isFraud = isFraud;
            ViewBag.searchNameDest = searchNameDest;
            IPagedList<Transaction> viewModel = null;
            if ((isFraud.HasValue || !string.IsNullOrEmpty(searchNameDest)) )//&& (User.IsAssistant() || User.IsAdministrator()))
            {
                viewModel = await _transactionsService.Find(new TransactionsSearchCriteria()
                {
                    IsFraud = isFraud,
                    SearchNameDest = searchNameDest
                }, 1, 100);
            }
            else
            {
                viewModel = await _transactionsService.Paginate(1, 100);
            }
            
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> MarkAsFraud()
        {
            string id = Request.Form["item.Id"];
            int parsedId;
            if (!int.TryParse(id, out parsedId)) return RedirectToAction("Index");
            await _transactionsService.MarkAsFraud(parsedId);
            return RedirectToAction("Index");
        }

        [Authorize(Policy = "Assistant")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "Assistant")]
        public async Task<IActionResult> Register(Transaction transaction)
        {
            await _transactionsService.Register(transaction);
            return RedirectToAction("Index");
        }
        
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
