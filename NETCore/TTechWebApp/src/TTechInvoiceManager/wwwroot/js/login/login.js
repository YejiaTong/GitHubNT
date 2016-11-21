$(function () {

    var $formLogin = $('#login-form');
    var $divForms = $('#div-forms');
    var $modalAnimateTime = 300;
    var $msgAnimateTime = 150;
    var $msgShowTime = 3000;

    /*$('#login_btn').click(function () {
        var $lg_username = $('#login_username').val();
        var $lg_password = $('#login_password').val();
        if ($lg_username == "ERROR") {
            msgChange($('#div-login-msg'), $('#icon-login-msg'), $('#text-login-msg'), "error", "glyphicon-remove", "Login error");
        } else {
            msgChange($('#div-login-msg'), $('#icon-login-msg'), $('#text-login-msg'), "success", "glyphicon-ok", "Login OK");
        }
        return false;
    });*/

    $("form").submit(function () {
        switch (this.id) {
            case "login-form":
                var $lg_username = $('#login_username').val();
                var $lg_password = $('#login_password').val();
                var $lg_rememberme = $('#RememberMe').is(':checked');

                if ($lg_username.trim() == "") {
                    msgChange($('#div-login-msg'), $('#icon-login-msg'), $('#text-login-msg'), "error", "glyphicon-remove", "Please enter your username");
                }
                else if ($lg_password.trim() == "") {
                    msgChange($('#div-login-msg'), $('#icon-login-msg'), $('#text-login-msg'), "error", "glyphicon-remove", "Please enter your password");
                }
                else {

                    var sendInfo = {
                        LoginId: $lg_username,
                        Password: $lg_password,
                        RememberMe: $lg_rememberme
                    };

                    $.ajax({
                        type: "POST",
                        url: "/Account/ValidateLogin",
                        dataType: "json",
                        success: function (msg) {
                            msgArray = msg.split("---");
                            if (msgArray[0] == "Pass") {
                                window.location.href = "../../" + msgArray[1];
                            } else {
                                msgChange($('#div-login-msg'), $('#icon-login-msg'), $('#text-login-msg'), "error", "glyphicon-remove", msg);
                            }
                        },

                        data: sendInfo
                    });
                }
                return false;
                break;
            default:
                return false;
        }
        return false;
    });

    function modalAnimate($oldForm, $newForm) {
        var $oldH = $oldForm.height();
        var $newH = $newForm.height();
        $divForms.css("height", $oldH);
        $oldForm.fadeToggle($modalAnimateTime, function () {
            $divForms.animate({ height: $newH }, $modalAnimateTime, function () {
                $newForm.fadeToggle($modalAnimateTime);
            });
        });
    }

    function msgFade($msgId, $msgText) {
        $msgId.fadeOut($msgAnimateTime, function () {
            $(this).text($msgText).fadeIn($msgAnimateTime);
        });
    }

    function msgChange($divTag, $iconTag, $textTag, $divClass, $iconClass, $msgText) {
        var $msgOld = $divTag.text();
        msgFade($textTag, $msgText);
        $divTag.addClass($divClass);
        $iconTag.removeClass("glyphicon-chevron-right");
        $iconTag.addClass($iconClass + " " + $divClass);
        setTimeout(function () {
            msgFade($textTag, $msgOld);
            $divTag.removeClass($divClass);
            $iconTag.addClass("glyphicon-chevron-right");
            $iconTag.removeClass($iconClass + " " + $divClass);
        }, $msgShowTime);
    }
});