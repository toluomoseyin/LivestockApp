#pragma checksum "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\Dashboard\DisplayAgentsMarket.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ec8aeb9c9183ae60ddcc1285a8f0e39d7e6a9c66"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Dashboard_DisplayAgentsMarket), @"mvc.1.0.view", @"/Views/Dashboard/DisplayAgentsMarket.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\_ViewImports.cshtml"
using AnimalFarmsMarket.Core;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\_ViewImports.cshtml"
using AnimalFarmsMarket.Data.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\_ViewImports.cshtml"
using AnimalFarmsMarket.Data.DTOs;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\_ViewImports.cshtml"
using AnimalFarmsMarket.Data.ViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\_ViewImports.cshtml"
using System.Globalization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\_ViewImports.cshtml"
using AnimalFarmsMarket.Data.Enum;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ec8aeb9c9183ae60ddcc1285a8f0e39d7e6a9c66", @"/Views/Dashboard/DisplayAgentsMarket.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"53b5788d97abd233649efb838f59001af955b690", @"/Views/_ViewImports.cshtml")]
    public class Views_Dashboard_DisplayAgentsMarket : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<LivestockMarketVm>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Dashboard", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "RegisteredUsers", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-success btn-sm"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("width:50px;"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\Dashboard\DisplayAgentsMarket.cshtml"
   var defaultImg = "https://res.cloudinary.com/oddcubes/image/upload/v1623677056/fluxk0fwarirww2bqpb9.jpg";
    var aa = Model.ToList();
    //var b = $"{aa[0].Agent.FirstName} {aa[0].Agent.LastName}";
    //var x  = Model[0].

#line default
#line hidden
#nullable disable
            WriteLiteral(@"

<style>

    .flip-card {
        background-color: transparent;
        width: 220px;
        height: 250px;
        perspective: 1000px;
    }

        .flip-card .inner {
            position: relative;
            width: 100%;
            height: 100%;
            transition: transform 0.8s;
            transform-style: preserve-3d;
        }

            .flip-card .inner .front,
            .flip-card .inner .back {
                position: absolute;
                width: 100%;
                height: 100%;
                -webkit-backface-visibility: hidden;
                backface-visibility: hidden;
                display: flex;
                align-items: center;
                justify-content: center;
                flex-direction: column;
                border-radius: 4px;
                border: 2px solid #e8e8e8;
            }

            .flip-card .inner .front {
                background-color: #bbb;
            }

            .flip-card .inne");
            WriteLiteral(@"r .back {
                background-color: #2078bf;
                color: white;
                transform: rotateY(180deg);
            }

        .flip-card:hover .inner {
            transform: rotateY(180deg);
        }
</style>
<div class=""container p-5 ml-10 bg-light"">
    <div class=""mb-1"">
        <h2 class=""brand pl-0"" style=""color:#2078BF;"">");
#nullable restore
#line 58 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\Dashboard\DisplayAgentsMarket.cshtml"
                                                 Write(Model.ToList()[0].AgentName);

#line default
#line hidden
#nullable disable
            WriteLiteral(" </h2>\r\n    </div>\r\n    <div class=\"d-flex flex-wrap flex-sm-wrap col-lg-12 col-sm-12 mb-2 pr-0 justify-content-end\">\r\n        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ec8aeb9c9183ae60ddcc1285a8f0e39d7e6a9c668635", async() => {
                WriteLiteral("<i class=\"fa fa-undo\"></i>");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n    </div>\r\n\r\n    <div");
            BeginWriteAttribute("class", " class=\"", 1986, "\"", 1994, 0);
            EndWriteAttribute();
            WriteLiteral(">\r\n        <div class=\"shadow bg-white rounded text-center p-4 mb-4\">\r\n");
#nullable restore
#line 66 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\Dashboard\DisplayAgentsMarket.cshtml"
             foreach (var item in Model)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"mb-4 pl-4 float-left\">\r\n    <h4 class=\"brand pl-0\" style=\"color:#2078BF;\">");
#nullable restore
#line 69 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\Dashboard\DisplayAgentsMarket.cshtml"
                                             Write(item.MarketName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n\r\n</div>\r\n                <div class=\"d-flex flex-wrap flex-sm-wrap col-lg-12 col-sm-12 justify-content-start\">\r\n");
#nullable restore
#line 73 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\Dashboard\DisplayAgentsMarket.cshtml"
                     foreach (var a in item.Livestocks)
                    {

                        {
                            var ima = a.Images.FirstOrDefault().ImageUrl;
                        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"col-3 mb-4\">\r\n        <div class=\"flip-card\">\r\n            <div class=\"inner\">\r\n                <div class=\"front\">\r\n                    <img");
            BeginWriteAttribute("src", " src=\"", 2713, "\"", 2754, 1);
#nullable restore
#line 83 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\Dashboard\DisplayAgentsMarket.cshtml"
WriteAttributeValue("", 2719, a.Images.FirstOrDefault().ImageUrl, 2719, 35, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"rounded\" alt=\"Daily Dev Tips\" style=\"width:220px;height:250px;\" />\r\n                </div>\r\n                <div class=\"back\">\r\n                    <h6>Breed: ");
#nullable restore
#line 86 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\Dashboard\DisplayAgentsMarket.cshtml"
                          Write(a.Breed);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h6>\r\n");
#nullable restore
#line 87 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\Dashboard\DisplayAgentsMarket.cshtml"
                     if (a.Sex == 0)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("<h6>Sex: Male</h6> ");
#nullable restore
#line 89 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\Dashboard\DisplayAgentsMarket.cshtml"
                   }
                    else if (a.Sex == 1)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("<h6>Sex: Female</h6>");
#nullable restore
#line 92 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\Dashboard\DisplayAgentsMarket.cshtml"
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <h6>Selling Price: ");
#nullable restore
#line 93 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\Dashboard\DisplayAgentsMarket.cshtml"
                                  Write(a.SellingPrice);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h6>\r\n                    <h6>Purchase Price: ");
#nullable restore
#line 94 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\Dashboard\DisplayAgentsMarket.cshtml"
                                   Write(a.PurchasePrice);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h6>\r\n                    <h6>Quantity Available: ");
#nullable restore
#line 95 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\Dashboard\DisplayAgentsMarket.cshtml"
                                       Write(a.Quantity);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h6>\r\n                    <h6>Discount: ");
#nullable restore
#line 96 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\Dashboard\DisplayAgentsMarket.cshtml"
                             Write(a.Discount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h6>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>                ");
#nullable restore
#line 100 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\Dashboard\DisplayAgentsMarket.cshtml"
                          }

#line default
#line hidden
#nullable disable
            WriteLiteral("                </div>");
#nullable restore
#line 101 "C:\Users\hp\Desktop\Sprint 4\livestock-platform\AnimalFarmsMarket\AnimalFarmsMarket.Core\Views\Dashboard\DisplayAgentsMarket.cshtml"
                      }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </div>\r\n\r\n\r\n\r\n\r\n\r\n\r\n    </div>\r\n</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public Microsoft.AspNetCore.Http.IHttpContextAccessor HttpContextAccessor { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IAuthorizationService _authorization { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public UserManager<AppUser> _userManager { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<LivestockMarketVm>> Html { get; private set; }
    }
}
#pragma warning restore 1591
