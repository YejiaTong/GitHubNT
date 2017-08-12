$(document).ready(function () {
    $("#send-form").submit(function (event) {
        alert("Site is under maintenance.");
        event.preventDefault();
    });

    $(".team-link").on("click", function () {
        var url = $(this).data("link-url");
        $(location).attr("href", url);
    });
});