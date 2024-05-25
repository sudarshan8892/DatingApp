var filterclick = function () {
    var jsonData = {
        MaxAge: $("#MaxAge").val(),
        MinAge: $("#MinAge").val(),
        Gender: $("#Gender").val(),
        OrderBy: $("#OrderBy").val()
    };
    debugger
    $.ajax({
        type: 'POST',
        url: '/Matches/PartialView',
        data: JSON.stringify(jsonData),
        contentType: "application/json",
        success: function (response) {
            $('#results').html(response);
        },
        error: function (xhr) {
            toastr.info(JSON.parse(xhr.responseText).error);
        }
    });
};


