#pragma checksum "C:\Users\lks5334\Dev\SeniorDesign\GitKraken\TicketingSystem\TicketingSystem\Views\Home\HomePage.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fc214ae13f1e28bf41985649acbf19b2854aa70e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_HomePage), @"mvc.1.0.view", @"/Views/Home/HomePage.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/HomePage.cshtml", typeof(AspNetCore.Views_Home_HomePage))]
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
#line 1 "C:\Users\lks5334\Dev\SeniorDesign\GitKraken\TicketingSystem\TicketingSystem\Views\_ViewImports.cshtml"
using TicketingSystem;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fc214ae13f1e28bf41985649acbf19b2854aa70e", @"/Views/Home/HomePage.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f3c4937889215bde3acb30a9b6e7e577e67827b2", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_HomePage : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<TicketingSystem.Models.TicketData>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "C:\Users\lks5334\Dev\SeniorDesign\GitKraken\TicketingSystem\TicketingSystem\Views\Home\HomePage.cshtml"
  
    Layout = "_Layout";

#line default
#line hidden
            BeginContext(87, 546, true);
            WriteLiteral(@"<!DOCTYPE html>

<div class=""text-center"">
    <h2 class=""display-4"">Here's the latest data</h2>
    <br />
    <br />
</div>
<div>
    <table id=""dummyTable"" class=""table table-hover table-bordered table-dark"">
        <thead class=""thead-light"">
            <tr>
                <th>Job Type</th>
                <th>Trip Number</th>
                <th>Stage Number</th>
                <th>Employee Name</th>
                <th>Time</th>
                <th>Status</th>
            </tr>
        </thead>
        <tbody>
");
            EndContext();
            BeginContext(1024, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 37 "C:\Users\lks5334\Dev\SeniorDesign\GitKraken\TicketingSystem\TicketingSystem\Views\Home\HomePage.cshtml"
         for (int i = 0; i < 10; i++)
        {

#line default
#line hidden
            BeginContext(1076, 68, true);
            WriteLiteral("            <tr>\r\n                <td>job</td>\r\n                <td>");
            EndContext();
            BeginContext(1145, 1, false);
#line 41 "C:\Users\lks5334\Dev\SeniorDesign\GitKraken\TicketingSystem\TicketingSystem\Views\Home\HomePage.cshtml"
               Write(i);

#line default
#line hidden
            EndContext();
            BeginContext(1146, 151, true);
            WriteLiteral("</td>\r\n                <td>s01</td>\r\n                <td>bob</td>\r\n                <td>12:34</td>\r\n                <td>closed</td>\r\n            </tr>\r\n");
            EndContext();
#line 47 "C:\Users\lks5334\Dev\SeniorDesign\GitKraken\TicketingSystem\TicketingSystem\Views\Home\HomePage.cshtml"
        }

#line default
#line hidden
            BeginContext(1308, 554, true);
            WriteLiteral(@"        </tbody>
    </table>
</div>
<script src=""https://code.jquery.com/jquery-2.2.4.min.js""
        integrity=""sha256-BbhdlvQf/xTY9gja0Dq3HiwQF8LaCRTXxZKRutelT44=""
        crossorigin=""anonymous""></script>
<link rel=""stylesheet"" type=""text/css"" href=""https://cdn.datatables.net/v/bs4/jq-3.3.1/dt-1.10.20/datatables.min.css"" />
<script type=""text/javascript"" src=""https://cdn.datatables.net/v/bs4/jq-3.3.1/dt-1.10.20/datatables.min.js""></script>
<script>$(document).ready(function () {
        $('#dummyTable').DataTable();
    });</script>
");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<TicketingSystem.Models.TicketData>> Html { get; private set; }
    }
}
#pragma warning restore 1591
