var monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"];

function showMonthAndSelect(i)
{
    $('.im-monthview-month-selector option')[i].selected = true;
}

$().ready(function () {
    var selectionDiv = $('.im-monthview-month-selection-div');
    var selectedYear = $(this).find("option:selected").val();
    selectionDiv.empty();
    var htmlContent = '';
    for (var i = 0; i < monthNames.length; i++) {
        htmlContent += "<option value=\"" + i + "\">" + monthNames[i] + "</option>";
    }
    htmlContent = "<span class=\"input-group-addon\" style=\"width: 100px;\" id=\"searchMonth\">Month: </span><select class=\"form-control im-addexpense-minwidth im-monthview-month-selector\" aria-describedby=\"searchYear\" onchange=\"monthChange()\">" + htmlContent + "</select>";
    selectionDiv.html(htmlContent);
    /*var d = new Date();
    var n = d.getMonth();*/
    var n = $('#Month').val();
    showMonthAndSelect(n);
});

function monthChange(){
    var selectedMonth = $('.im-monthview-month-selector').find("option:selected").val();
    $('#Month').val(selectedMonth);
}

/*$('.im-monthview-month-selector').change(function () {
    var selectedMonth = $(this).find("option:selected").val();
    $('#Month').val(selectedMonth);
    alert($('#Month').val())
});*/