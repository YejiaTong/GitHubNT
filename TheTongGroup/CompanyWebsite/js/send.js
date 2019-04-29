$(document).ready(function () {
    var appliactionKey = "thetonggroupnoahytong";

    function isEmail(email) {
        var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        return regex.test(email);
    }

    function isHuman() {
        var response = grecaptcha.getResponse();
        if (response.length == 0) {
            return false;
        } else {
            return true;
        }
    }

    function validateForm() {
        if (!isEmail($("#send-form-email").val())) {
            alert("Invalid email address");
            return false;
        }

        if (!isHuman()) {
            alert("Non-human?! Please verify");
            return false;
        }

        return true;
    }

    function processForm(event) {
        $.ajax({
            url: "https://nodejs.ttechcode.com/sendgrid/mail",
            dataType: "json",
            type: "post",
            data: {
                key: appliactionKey,
                name: $("#send-form-name").val(),
                email: $("#send-form-email").val(),
                phone: $("#send-form-phone").val(),
                subject: $("#send-form-subject").val(),
                message: $("#send-form-message").val()
            },
            success: function (data, textStatus, jQxhr) {
                if (data.success === undefined) {
                    alert("Unhandled exception");
                } else {
                    if (data.success == 'false' && data.msg !== undefined) {
                        alert(data.msg);
                    } else if (data.success == 'true') {
                        alert("Email sent. Thank you");
                        $('#send-form').trigger("reset");
                    } else {
                        alert("Unhandled exception");
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    }

    $("#send-form").submit(function (event) {
        event.preventDefault();
        if (validateForm()) {
            processForm(event);
        }
        else {
            return;
        }
        //alert("Please be advised site is under maintenance.\nPlease contact at 1 (519) 562-6035 or noah089736@gmail.com");
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