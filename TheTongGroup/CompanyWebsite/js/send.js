$(document).ready(function () {
    $("#send-form").submit(function (event) {
        alert("Please be advised site is under maintenance.\nPlease contact at 1 (519) 562-6035 or noah089736@gmail.com");
        event.preventDefault();
    });

    $(".team-link").on("click", function () {
        var url = $(this).data("link-url");
        if (url.match("^http")) {
            window.open(url, "_blank")
        } else {
            $(location).attr("href", url);
        }
    });
});