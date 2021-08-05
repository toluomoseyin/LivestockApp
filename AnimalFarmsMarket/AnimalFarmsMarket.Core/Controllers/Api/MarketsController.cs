using AnimalFarmsMarket.Commons;
using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.Models;
using AnimalFarmsMarket.Data.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Core.Controllers.Api
{
   
        [Route("api/v1/[controller]")]
        [ApiController]
        public class MarketsController : ControllerBase
        {
            private readonly IMarketService _shippingDetailsService;
            private readonly IMapper _mapper;
            private readonly UserManager<AppUser> _userManager;


            public MarketsController(IMarketService shippingDetailsInfo, UserManager<AppUser> userManager, IMapper mapper)
            {
                _shippingDetailsService = shippingDetailsInfo;
                _userManager = userManager;
                _mapper = mapper;
            }

            [Authorize]
            [HttpGet("shippingdetails")]
            public async Task<IActionResult> GetShippingDetails()
            {
                var loggedinuser = _userManager.GetUserId(HttpContext.User);
                var user = _userManager.Users.Include(x => x.Address).FirstOrDefault(x => x.Id == loggedinuser);
                if (user == null)
                {
                    ModelState.AddModelError("User", "Invalid User");
                    var errMsg = Utilities.CreateResponse("Invalid User", errs: ModelState, data: "");
                    return BadRequest(errMsg);
                }

                var shippingdetails = await _shippingDetailsService.GetDeliveryModes();
                var paymentinfo = await _shippingDetailsService.GetPaymentMethods();
                var shippingplans = await _shippingDetailsService.GetShippingPlans();

                var userdto = _mapper.Map<ShipDetailsUserDto>(user);


                var datatoreturn = new ShippingDetailsDto()
                {
                    ShippingPlans = shippingplans,
                    PaymentMethods = paymentinfo,
                    DeliveryModes = shippingdetails,
                    User = userdto

                };
                return Ok(Utilities.CreateResponse<ShippingDetailsDto>("", null, datatoreturn));
            }
        }
    
}
