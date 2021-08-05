using AnimalFarmsMarket.Commons;
using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.Models;
using AnimalFarmsMarket.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Core.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IConfiguration _config;

        public CheckoutController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Flutterwave()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Initiate()
        {
            var pay = new FlutterAgentPayDto();
            var cardDetails = new CardDetails();

            if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("flutterpay")))
            {
                pay = JsonConvert.DeserializeObject<FlutterAgentPayDto>(HttpContext.Session.GetString("flutterpay"));
                cardDetails.amount = pay.Amount.ToString();
                cardDetails.email = pay.Email;
                cardDetails.tx_ref = "LVS-3425";
            }
            else
            {
                var tempOrder = JsonConvert.DeserializeObject<Order>(HttpContext.Session.GetString("tempOrder"));
                cardDetails.amount = tempOrder.PaymentAmount.ToString();
                cardDetails.email = tempOrder.User.Email;
                cardDetails.tx_ref = "LVS-3425";
            }
            return View(cardDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Initiate(CardDetails cardDetails)
        {

            if (string.IsNullOrWhiteSpace(_config["Flutterwave:BaseUrl"]) ||
                string.IsNullOrWhiteSpace(_config["Flutterwave:SecretKey"]) ||
                string.IsNullOrWhiteSpace(_config["Flutterwave:EncryptionKey"]))
                return View(cardDetails);//go to failure view


            var response = await FlutterwavePayment.AuthorizePayment(cardDetails, _config["Flutterwave:BaseUrl"],
                                                                    "charges?type=card", _config["Flutterwave:SecretKey"],
                                                                    _config["Flutterwave:EncryptionKey"]);

            if (response == null)
            {
                ViewData["errorMsg"] = "Invalid Card details";
                return View(cardDetails); // return error view 
            }

            if (response.status != "success")
                return View(cardDetails);//go to failure view

            TempData["ref"] = response.data.flw_ref;

            return RedirectToAction("Validate", "Checkout");
        }


        [HttpGet]
        public IActionResult Validate()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Validate(string otp)
        {
            var flw_ref = TempData["ref"] as string;
            var validationDto = new FlutterwaveValidationDto
            {
                flw_ref = flw_ref,
                otp = otp
            };

            var validationResponse = await FlutterwavePayment.ValidatePayment(validationDto, _config["Flutterwave:BaseUrl"],
                                                                    "validate-charge", _config["Flutterwave:SecretKey"]);

            if (validationResponse == null)
                return View(); // return error view 

            var serializedResponse = JsonConvert.SerializeObject(validationResponse);

            HttpContext.Session.SetString("verifyDetails", serializedResponse);

            if (validationResponse.status != "success")
                return View();//go to failure view

            if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("flutterpay")))
                return RedirectToAction("ActivateAgent", "Dashboard");

            return RedirectToAction("ShippingDetailsSave", "Market");
        }

        [HttpGet]
        public IActionResult Paystack()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Paystack(string email, string amount)
        {

            if (string.IsNullOrWhiteSpace(_config["Paystack:Token"]))
                return View();//go to failure view

            var request = new PaystackRequestDto
            {
                amount = amount,
                email = email,
                callback_url = UrlHelper.BaseAddress(HttpContext) + "/market/confirmationpage"
            };
            var result = await PaystackPayment.InitiatePayment(request, _config["Paystack:BaseUrl"], "initialize", _config["Paystack:Token"]);


            if (result == null)
                return View(); // return error view 

            if (result.Status == "true")
            {
                return Redirect(result.Data.AuthorizationUrl);
            }
            return View();
        }

    }
}
