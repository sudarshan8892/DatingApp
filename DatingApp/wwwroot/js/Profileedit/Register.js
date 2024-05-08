


var submitRegisterForm = function () {
    if (!$("#frmRegister").valid()) {
        return;
    }

    //var _frmRegister = $("#frmRegister").serialize();
    debugger
    var jsonData = {
        Gender: $("input[name='Gender']:checked").val(),
        Username: $("#Username").val(),
        KnownAs: $("#KnownAs").val(),
        DateOfBirth: $("#DateOfBirth").val(),
        City: $("#City").val(),
        Country: $("#Country").val(),
        Password: $("#Password").val(),
        ConfirmPassword: $("#ConfirmPassword").val()
    };
    console.log(jsonData);
    debugger
    $.ajax({
        url: '/Home/Register',
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(jsonData),
    
        success: function (response) {
            debugger
            if (response.success) {
                toastr.success("Registration successful");
                location.href = "/Matches/Index";
            } else {

                toastr.error("Failed to register");
            }
        },
        error: function (xhr, status, error) {
            toastr.error("Error:", error);
        }
    });

}

