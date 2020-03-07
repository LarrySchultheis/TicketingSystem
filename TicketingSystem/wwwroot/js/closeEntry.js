
$(document).ready(fillFields)

function fillFields() {
    //alert('fill fields')
    console.log(document.URL)
    var ticketData = $.getJSON(document.URL, function (data) {
        //alert('data')
        console.log(data)
    })
}