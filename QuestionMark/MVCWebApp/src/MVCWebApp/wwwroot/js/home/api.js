$(function () {

    var strWindowFeatures = "location=yes,height=240,width=360,scrollbars=yes,status=yes";

    $("#linkOne").click(function () {
        var url = "../api/ping";
        var win = window.open(url, "_blank", strWindowFeatures);
        //window.location = "../api/ping";
    });

    $("#linkTwo").click(function () {
        var $lv_One = $('#getDiffInDaysInputOne').val();
        var $lv_Two = $('#getDiffInDaysInputTwo').val();
        if ($lv_One.trim() == "" || $lv_Two.trim() == "") {
            alert("Please enter non-empty values");
        }
        else {
            var url = "../api/getDiffInDays/" + $lv_One + "/" + $lv_Two;
            var win = window.open(url, "_blank", strWindowFeatures);
            //window.location = "../api/getDiffInDays/" + $lv_One + "/" + $lv_Two;
        }
    });

});