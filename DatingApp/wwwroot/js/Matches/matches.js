// matches.js
$(document).ready(function () {
    $('.like-button').on('click', function (event) {
        event.preventDefault();
        var userName = $(this).data('username');
        console.log("User name clicked: " + userName);
        debugger
        $.ajax({
            url: '@Url.Action("AddLike", "Matches")' + '?UserName=' + encodeURIComponent(userName),
            type: 'POST',
            success: function (data) {
                debugger
                if (data.success) {
                    toastr.success('you have liked ' + data.username);
                } else {
                    toastr.error(data.error);
                }
            },
            error: function (xhr, status, error) {
                toastr.error('Error adding like: ' + error);
            }
        });
    });
});
