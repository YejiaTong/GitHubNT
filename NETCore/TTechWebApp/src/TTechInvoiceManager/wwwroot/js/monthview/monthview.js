function findLastDayoftheMonth(m) {
    if (m.getMonth() > 11) {
        return new Date(m.getFullYear(), 0, 0);
    }
    else {
        return new Date(m.getFullYear(), m.getMonth() + 1, 0);
    }
}

var monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"];

function showMonthAndSelect()
{
    var n = $('#Month').val();
    $('.im-monthview-month-selector option')[n].selected = true;
}

function showMonthSelectionLabel() {
    //var selectedYear = $('#Year').find("option:selected").val();
    //var selectedMonth = $('.im-monthview-month-selector').find("option:selected").val();
    var selectedYear = $('#Year').val();
    var selectedMonth = $('#Month').val();
    var monthStart = new Date(selectedYear, selectedMonth, 1);
    var monthEnd = findLastDayoftheMonth(monthStart);
    var htmlContent = '<h4><span class=\"label label-default\">' + (monthStart.getMonth() + 1) + '/' + monthStart.getDate() + '/' + monthStart.getFullYear() + '<span> - </span>' + (monthEnd.getMonth() + 1) + '/' + monthEnd.getDate() + '/' + monthEnd.getFullYear() + '</span></h4>';
    $('.im-monthview-month-selection-label-div').empty();
    $('.im-monthview-month-selection-label-div').html(htmlContent);
}

function monthChange() {
    var selectedMonth = $('.im-monthview-month-selector').find("option:selected").val();
    $('#Month').val(selectedMonth);
    showMonthSelectionLabel();
}

$().ready(function () {
    var selectionDiv = $('.im-monthview-month-selection-div');
    selectionDiv.empty();
    var htmlContent = '';
    for (var i = 0; i < monthNames.length; i++) {
        htmlContent += "<option value=\"" + i + "\">" + monthNames[i] + "</option>";
    }
    htmlContent = "<span class=\"input-group-addon\" style=\"width: 100px;\" id=\"searchMonth\">Month: </span><select class=\"form-control im-addexpense-minwidth im-monthview-month-selector\" aria-describedby=\"searchMonth\" onchange=\"monthChange()\">" + htmlContent + "</select>";
    selectionDiv.html(htmlContent);
    showMonthAndSelect();
    showMonthSelectionLabel();
});

$('#Year').change(function () {
    showMonthSelectionLabel();
});