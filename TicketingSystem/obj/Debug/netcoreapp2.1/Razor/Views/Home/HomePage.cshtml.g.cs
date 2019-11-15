#pragma checksum "G:\GitKraken\TicketingSystem\TicketingSystem\Views\Home\HomePage.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1f261e37588170f041b578909c2696871868cd54"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_HomePage), @"mvc.1.0.view", @"/Views/Home/HomePage.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/HomePage.cshtml", typeof(AspNetCore.Views_Home_HomePage))]
namespace AspNetCore
{
    #line hidden
    using System;
#line 1 "G:\GitKraken\TicketingSystem\TicketingSystem\Views\Home\HomePage.cshtml"
using System.Collections.Generic;

#line default
#line hidden
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "G:\GitKraken\TicketingSystem\TicketingSystem\Views\_ViewImports.cshtml"
using TicketingSystem;

#line default
#line hidden
#line 2 "G:\GitKraken\TicketingSystem\TicketingSystem\Views\Home\HomePage.cshtml"
using TicketingSystem.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1f261e37588170f041b578909c2696871868cd54", @"/Views/Home/HomePage.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f3c4937889215bde3acb30a9b6e7e577e67827b2", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_HomePage : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<TicketingSystem.Models.TicketData>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 4 "G:\GitKraken\TicketingSystem\TicketingSystem\Views\Home\HomePage.cshtml"
  
    Layout = "_Layout";

#line default
#line hidden
            BeginContext(148, 525, true);
            WriteLiteral(@"<!DOCTYPE html>

<div class=""text-center"">
    <h2 class=""display-4"">Open Tickets</h2>
    <br />
    <br />
</div>
<div>
    <table id=""dummyTable"" class=""table table-hover table-bordered"">
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
#line 27 "G:\GitKraken\TicketingSystem\TicketingSystem\Views\Home\HomePage.cshtml"
             foreach (var item in Model)
            {

#line default
#line hidden
            BeginContext(730, 19, true);
            WriteLiteral("                <tr");
            EndContext();
            BeginWriteAttribute("onclick", " onclick=\"", 749, "\"", 784, 3);
            WriteAttributeValue("", 759, "rowClicked(", 759, 11, true);
#line 29 "G:\GitKraken\TicketingSystem\TicketingSystem\Views\Home\HomePage.cshtml"
WriteAttributeValue("", 770, item.EntryId, 770, 13, false);

#line default
#line hidden
            WriteAttributeValue("", 783, ")", 783, 1, true);
            EndWriteAttribute();
            BeginContext(785, 27, true);
            WriteLiteral(">\r\n                    <td>");
            EndContext();
            BeginContext(813, 20, false);
#line 30 "G:\GitKraken\TicketingSystem\TicketingSystem\Views\Home\HomePage.cshtml"
                   Write(item.JobType.JobName);

#line default
#line hidden
            EndContext();
            BeginContext(833, 31, true);
            WriteLiteral("</td>\r\n                    <td>");
            EndContext();
            BeginContext(865, 12, false);
#line 31 "G:\GitKraken\TicketingSystem\TicketingSystem\Views\Home\HomePage.cshtml"
                   Write(item.TripNum);

#line default
#line hidden
            EndContext();
            BeginContext(877, 31, true);
            WriteLiteral("</td>\r\n                    <td>");
            EndContext();
            BeginContext(909, 13, false);
#line 32 "G:\GitKraken\TicketingSystem\TicketingSystem\Views\Home\HomePage.cshtml"
                   Write(item.StageNum);

#line default
#line hidden
            EndContext();
            BeginContext(922, 31, true);
            WriteLiteral("</td>\r\n                    <td>");
            EndContext();
            BeginContext(954, 26, false);
#line 33 "G:\GitKraken\TicketingSystem\TicketingSystem\Views\Home\HomePage.cshtml"
                   Write(item.TicketWorker.FullName);

#line default
#line hidden
            EndContext();
            BeginContext(980, 31, true);
            WriteLiteral("</td>\r\n                    <td>");
            EndContext();
            BeginContext(1012, 14, false);
#line 34 "G:\GitKraken\TicketingSystem\TicketingSystem\Views\Home\HomePage.cshtml"
                   Write(item.StartTime);

#line default
#line hidden
            EndContext();
            BeginContext(1026, 31, true);
            WriteLiteral("</td>\r\n                    <td>");
            EndContext();
            BeginContext(1059, 37, false);
#line 35 "G:\GitKraken\TicketingSystem\TicketingSystem\Views\Home\HomePage.cshtml"
                    Write(item.TicketClosed ? "Closed" : "Open");

#line default
#line hidden
            EndContext();
            BeginContext(1097, 30, true);
            WriteLiteral("</td>\r\n                </tr>\r\n");
            EndContext();
#line 37 "G:\GitKraken\TicketingSystem\TicketingSystem\Views\Home\HomePage.cshtml"

            }

#line default
#line hidden
            BeginContext(1144, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(1516, 34, true);
            WriteLiteral("        </tbody>\r\n    </table>\r\n\r\n");
            EndContext();
            BeginContext(1654, 691, true);
            WriteLiteral(@"
</div>

<script src=""https://code.jquery.com/jquery-3.4.1.min.js""
        integrity=""sha256-CSXorXvZcTkaix6Yvo6HppcZGetbYMGWSFlBw8HfCJo=""
        crossorigin=""anonymous""></script>

<link rel=""stylesheet"" type=""text/css"" href=""https://cdn.datatables.net/v/bs/dt-1.10.20/datatables.min.css"" />
<script type=""text/javascript"" src=""https://cdn.datatables.net/v/bs/dt-1.10.20/datatables.min.js""></script>

<script>
    $(document).ready(function () {
        var table = $('#dummyTable').DataTable()
    });</script>

<script>
    function rowClicked(entryId) {
        var id = entryId
        console.log(id)

        $.ajax({
            type: ""POST"",
            url: '");
            EndContext();
            BeginContext(2346, 31, false);
#line 77 "G:\GitKraken\TicketingSystem\TicketingSystem\Views\Home\HomePage.cshtml"
             Write(Url.Action("OpenEntry", "Home"));

#line default
#line hidden
            EndContext();
            BeginContext(2377, 357, true);
            WriteLiteral(@"',
            data: { entryID: id },
            success: function () {
                alert('Success')
            },
            error: function (e) {
                alert('Error')
                console.log(e)
            }
        }).done(function (data) {
            window.location.replace(data.newUrl)
        })

    }
</script>
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<TicketingSystem.Models.TicketData>> Html { get; private set; }
    }
}
#pragma warning restore 1591
