using AnimalFarmsMarket.Commons;
using AnimalFarmsMarket.Data.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Core.Controllers.Api
{
    [Route("api/v1/[controller]")]
    public class PaymentMethodController : Controller
    {
        private readonly IMarketService _payment;

        public PaymentMethodController(IMarketService payment)
        {
            _payment = payment;
        }


        [Authorize]
        [HttpGet("{paymentmethodId}")]
        public async Task<IActionResult> GetPaymentMethodById(string paymentMethodId)
        {

            var method =await _payment.GetPaymentMethodById(paymentMethodId);

            if (method == null)
            {
                ModelState.AddModelError("Method", "No Method was found with this Id");
                return NotFound(Utilities.CreateResponse(message: "No Method found", errs: ModelState, ""));
            }

            return Ok(Utilities.CreateResponse(message: "List ", errs: null, data: method));
        }
    }
}
