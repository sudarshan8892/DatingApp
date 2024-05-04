

var formChanged = false;
var checkFormChanges = function () {
    $('form textarea, form input').on('input', function () {
        $('#Alertcheck').show();
        $('#submitButton').prop('disabled', false);
        formChanged = true;

    });
}

$(window).on('beforeunload', async function () {
    if (formChanged) {
        var result = await showConfirmationDialog();
        if (!result) {
            return 'Your changes may not be saved.';
        }
    }
});

function submitEditForm() {
    
    $.ajax({
        url: $("#Editmember").data("ajax-url"),
        method: $("#Editmember").data("ajx-mehtod"),
        data: $("#Editmember").serialize(),
        success: function (response) {
            toastr.success("updated successfully!");
            $('#submitButton').prop('disabled', true);
            $('#Alertcheck').hide();
        },
        error: function (xhr, status, error) {
            toastr.error("Error:", error);
        }
    });

}
$(document).ready(function () {
    checkFormChanges();
});

