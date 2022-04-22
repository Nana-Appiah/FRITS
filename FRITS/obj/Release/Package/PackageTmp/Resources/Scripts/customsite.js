function showSuccessMessage(msg) {
    swal("Success!", msg, "success");
}

function showErrorMessage(msg) {
    swal("Error!", msg, "error");
}

function ConfirmAction(msg) {
    confirm(msg);
}

function alertMessage(msg) {
    alert(msg);
}

function sleep(milliseconds) {
    var start = new Date().getTime();
    for (var i = 0; i < 1e7; i++) {
        if ((new Date().getTime() - start) > milliseconds) {
            break;
        }
    }
}

function showHideElement(eleName, showElement = false) {
    if (showElement) {
        $('#' + eleName).hide();
    }       
    else {
        $('#' + eleName).show();
    }
}

function getConfirmation(msg) {
    swal({
        title: "Confirm Action?",
        text: msg,
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                return true;
            } else {
                return false;
            }
        });
}

function confirmDelete(msg) {
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover this record!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    }).then((willDelete) => {
        if (willDelete) {
            swal("Poof! Record successfully been deleted!", {
                icon: "success",
            });
            return true;
        } else {
            swal("Record not deleted!");
            return false;
        }
    });
}