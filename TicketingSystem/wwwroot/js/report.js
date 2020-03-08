function getReport() {
    alert('in get report')

    var url = window.location.host + "/report/RunReport"
    //var xhr = new XMLHttpRequest();
    //var params = JSON.stringify({
    //    StartDate: $('#startDate').val(),
    //    EndDate: $('#endDate').val(),
    //    ReportName: $('#reportName').val(),
    //    ReportFormat: $('#reportFormat').val()
    //})
    //xhr.open("POST", url, false)

    //xhr.setRequestHeader("Content-type", "application/json")
    //xhr.setRequestHeader("Content-length", params.length)
    //xhr.setRequestHeader("Connection", "close")

    //xhr.onreadystatechange = function () {
    //    if (xhr.readyState == 4 && xhr.status == 200) {
    //        alert(xhr.responseText);
    //        console.log(xhr.response)
    //    }
    //}

    $.ajax({
        type: "POST",
        url: url,
        //contentType: "application/json",
        data: {
            StartDate: $('#startDate').val(),
            EndDate: $('#endDate').val(),
            ReportName: $('#reportName').val(),
            ReportFormat: $('#reportFormat').val()
        },
        async: false,
        success: function () {
            alert('Success')
        },
        error: function (e) {
            alert('Error')
            console.log(e)
        }
    }).done(function (data) {
        alert('done')
        console.log(data.data)
        //var blob = new Blob([data.data], { type: "application/pdf" })
        //var objectUrl = URL.createObjectURL(blob);
        //window.open(objectUrl)
        //window.location.replace(data.newUrl)
    })
    


}