
function clearForm() {
    document.getElementById('entryForm').reset()
}

function updateFields() {

    $('*[data-attr="nonDefault"]').remove()
    var jType = document.getElementById('jobType')

    if (jType.selectedIndex == 2) { //case picker
        //add cases number and pallet wrap number
        $('<input name="CasesNum" type="number" class="form-control" id="casesNum" data-attr="nonDefault">').appendTo($('#casesNumberInnerContainer'))
        $('<label class="control-label col-sm-2" for="casesNum" id="casesNumLbl" data-attr="nonDefault">Cases Number:</label>').insertBefore($('#casesNumberInnerContainer'))
        
        $('<input name="PalletWrapsNumber" type="number" class="form-control" id="palletWrapsNumber" data-attr="nonDefault">').appendTo($('#palletWrapsInnerContainer'))
        $('<label class="control-label col-sm-2" for="palletwrapsNumber" id="palletWrapsNumberLbl" data-attr="nonDefault">Pallet Wraps Number:</label>').insertBefore($('#palletWrapsInnerContainer'))
    }
    else if (jType.selectedIndex == 5) { //inbound
        //add trailer no. and carrier fields
        $('<input name="TrailerNumber" type="number" class="form-control" id="trailerNumber" data-attr="nonDefault">').appendTo($('#trailerNumberInnerContainer'))
        $('<label class="control-label col-sm-2" for="trailerNumber" id="trailerNumberLbl" data-attr="nonDefault">Trailer Number:</label>').insertBefore($('#trailerNumberInnerContainer'))

        $('<input name="Carrier" type="text" class="form-control" id="carrier" data-attr="nonDefault">').appendTo($('#carrierInnerContainer'))
        $('<label class="control-label col-sm-2" for="carrier" id="carrierLbl" data-attr="nonDefault">Carrier:</label>').insertBefore($('#carrierInnerContainer'))
    }
    else if (jType.selectedIndex == 6) { //outbound
        //add barrier/ALCOR and ASN fields

    }
}

function getValidNames() {

}

function verifyInput() {
    var validNames

    var url = window.location.protocol + "//" + window.location.host + "/Home/ValidNames"
    $.ajax({
        type: "GET",
        url: url,
        async: false,
        success: function () {
            //alert('Success')
        },
        error: function (e) {
            console.log(e)
        }
    }).done(function (data) {
        validNames = data.names
    })

    var jType = document.getElementById('jobType')
    var tripNum = document.getElementById('tripNumber')
    var stageNum = document.getElementById('stageNumber')
    var empName = document.getElementById('employeeName')
    var startTime = document.getElementById('startTime')
    var endTime

    var validName = false
    for (var i = 0; i < validNames.length; i++) {
        if (empName.value == validNames[i]) {
            validName = true
        }
    }

    if (!validName) {
        alert('Error, the worker name you entered was not found in the system')
        return false
    }

    if (jType.selectedIndex < 1) {
        alert('Please select a Job Type')
        return false
    }
    if (tripNum.value == "") {
        alert('Please enter a valid trip number')
        return false
    }
    if (stageNum.value == "") {
        alert('Please enter a valid stage number')
        return false
    }
    if (empName.value == "") {
        alert('Please enter a valid employee name')
        return false
    }
    if (startTime.value == "") {
        alert('Please enter a valid start time')
        return false
    }
    if ((endTime = document.getElementById('endTime')) != null) {
        if (endTime.value == "") {
            alert('Please enter a valid end time')
            return false
        }
    }
    return true
    //alert('success')
}
