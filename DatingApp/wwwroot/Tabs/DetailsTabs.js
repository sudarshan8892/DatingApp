/* JS for  tabs */
$(document).ready(function () {
    $(".tab").click(function () {
        var tab_id = $(this).attr("data-tab");

        $(".tab").removeClass("active");
        $(".tabcontent").removeClass("active");

        $(this).addClass("active");
        $("#" + tab_id).addClass("active");
    });
});

