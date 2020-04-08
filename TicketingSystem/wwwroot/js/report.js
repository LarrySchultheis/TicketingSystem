
function validateDate(date) {
    var strArr = date.split("-")
    var year = strArr[0]
    var month = strArr[1]
    var day = strArr[2]

    var now = new Date()
    var daysInMonth = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]

    if (year > now.getFullYear()) {
        return false
    }
    if (month < 1 || month > 12) {
        return false
    }
    if (day < 1 || day > daysInMonth[month - 1]) {
        return false
    }
    return true
}

function getReport() {

    var startDate = $('#startDate').val()
    var endDate = $('#endDate').val()

    if (validateDate(startDate) && validateDate(endDate)) {
        requestReport(startDate, endDate)
    }
    else {
        alert('Please enter valid dates')
    }

}

function requestReport(startDate, endDate) {
    var url = window.location.protocol + "//" + window.location.host + "/Report/RunReport";
    var reportName = $('#reportName').val()
    var reportFormat = $('#reportFormat').val()
    $.ajax({
        type: "GET",
        url: url,
        data: {
            StartDate: startDate,
            EndDate: endDate,
            ReportName: reportName,
            ReportFormat: reportFormat
        },
        async: true,
        beforeSend: function () {
            document.getElementById('dialog-message').hidden = false
            $("#dialog-message").dialog({
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
        },
        success: function () {
        },
        error: function (e) {
            alert('Error')
            console.log(e)
        }
    }).done(function (data) {
        var headers = data.content.headers

        var contentType
        var contentDisposition
        var fileName
        var today = new Date()
        for (var i = 0; i < headers.length; i++) {
            if (headers[i].key == 'Content-Type')
                contentType = headers[i].value[0]
            if (headers[i].key == 'Content-Disposition') {
                contentDisposition = headers[i].value[0]

            }
        }

        var pos = contentDisposition.search("=")
        fileName = contentDisposition.substring(pos + 2, (contentDisposition.length - 5))

        fileName += "_" + today.getMonth() + "-" + today.getDate() + "-" + today.getFullYear()

        if (contentType == 'application/pdf')
            fileName += ".pdf"
        else if (contentType == 'text/csv; charset=utf-8')
            fileName += ".csv"
        else
            fileType = null

        var byteArray = base64ToArrayBuffer(data.data)
        saveByteArray(fileName, contentType, byteArray)
    })
}

function base64ToArrayBuffer(base64) {
    var binString = window.atob(base64)
    var binLen = binString.length
    var bytes = new Uint8Array(binLen)
    for (var i = 0; i < binLen; i++) {
        var ascii = binString.charCodeAt(i)
        bytes[i] = ascii
    }
    return bytes
}

function saveByteArray(fileName, contentType, bytes) {
    var blob = new Blob([bytes], { type: contentType })
    var link = document.createElement('a')
    link.href = window.URL.createObjectURL(blob)
    link.download = fileName
    link.click()
}