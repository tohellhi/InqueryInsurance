using Core.Models;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace InqueryInsurance.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
        private readonly RequestService _requestService;
        private readonly InsurancePatternService _insurancePatternService;

        public HomeController(RequestService requestService, InsurancePatternService insurancePatternService)
        {
            _requestService = requestService;
            _insurancePatternService = insurancePatternService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _requestService.MyRequests(UserId));
        }

        public async Task<IActionResult> CreateRequest(long? id)
        {
            var request = id <= 0 ? new Request() : await _requestService.MyRequest(UserId, id.Value);
            if (request == null)
                request = new Request();


            ViewData["patterns"] = (await _insurancePatternService.ListOfInsurancePatterns()).Select(x => new SelectListItem(x.Title, x.Id.ToString())).ToList();


            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest(Request request, List<int> patterns)
        {
            if (patterns.Count == 0)
            {
                ViewData["error"] = "پوششی انتخاب نکرده اید";
                ViewData["patterns"] = (await _insurancePatternService.ListOfInsurancePatterns()).Select(x => new SelectListItem(x.Title, x.Id.ToString(),patterns.Contains(x.Id))).ToList();
                return View(request);
            }
            try
            {
                request.RequesterId = UserId;
                var requestId = await _requestService.NewRequests(request, patterns);
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ViewData["error"] = ex.Message;
                ViewData["patterns"] = (await _insurancePatternService.ListOfInsurancePatterns()).Select(x => new SelectListItem(x.Title, x.Id.ToString(), patterns.Contains(x.Id))).ToList();
                return View(request);
            }
        }

    }
}