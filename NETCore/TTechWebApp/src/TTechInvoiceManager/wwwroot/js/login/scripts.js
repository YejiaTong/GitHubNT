jQuery(document).ready(function () {

    var $formLogin = $('#login-form');
    var $divForms = $('#div-forms');
    var $modalAnimateTime = 300;
    var $msgAnimateTime = 150;
    var $msgShowTime = 3000;

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

    /*
        Fullscreen background
    */
    $.backstretch([
                    "../../images/login/2.jpg"
	              , "../../images/login/3.jpg"
	              , "../../images/login/1.jpg"
	             ], {duration: 3000, fade: 750});
    
    /*
        Form validation
    */
    $('.form-horizontal input[type="text"], .form-horizontal input[type="password"], .form-horizontal textarea').on('focus', function () {
    	$(this).removeClass('input-error');
    });
    
    $('.form-horizontal').on('submit', function (e) {

        var $lg_username = $('#login_username').val();
        var $lg_password = $('#login_password').val();
        var $lg_rememberme = $('#RememberMe').is(':checked');

        if ($lg_username.trim() == "" || $lg_password.trim() == "") {
            msgChange($('#div-login-msg'), $('#icon-login-msg'), $('#text-login-msg'), "error", "glyphicon-remove", "Please enter valid credentials");
            if ($lg_username.trim() == "") {
                $('#login_username').addClass('input-error');
            }
            if ($lg_password.trim() == "") {
                $('#login_password').addClass('input-error');
            }

            return false;
        }
        else
        {
            $('#login_username').removeClass('input-error');
            $('#login_password').removeClass('input-error');
        }

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
                if (msg == "Pass") {
                    window.location.href = "../../Home/Account";
                } else {
                    msgChange($('#div-login-msg'), $('#icon-login-msg'), $('#text-login-msg'), "error", "glyphicon-remove", msg);
                }
            },

            data: sendInfo
        });

        return false;
    });
});
