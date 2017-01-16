function findLastDayoftheMonth(m) {
    if (m.getMonth() > 11) {
        return new Date(m.getYear(), 0, 0);
    }
    else {
        return new Date(m.getYear(), m.getMonth() + 1, 0);
    }
}

/*function findFirstDayoftheMonth(m) {
    return new Date(m.getFullYear(), m.getMonth(), 1);
}*/

function findWeekNum(d) {
    var jan1 = new Date(d.getFullYear(), 0, 1);
    var firstDayofWeek = getMonday(jan1);
    var firstDayofTargetWeek = getMonday(d);
    return Math.ceil(((firstDayofTargetWeek.getTime() - firstDayofWeek.getTime()) / 86400000) / 7) + 1;
}

function getDateOfISOWeek(w, y) {
    var jan1 = new Date(y, 0, 1);
    var firstDayofWeek = getMonday(jan1);
    var ret = new Date();
    ret.setTime(firstDayofWeek.getTime() + (24 * 60 * 60 * 1000) * 7 * (w - 1))
    return ret;
}

function getMonday(d) {
    d = new Date(d);
    var day = d.getDay(),
        diff = d.getDate() - day + (day == 0 ? -6 : 1);
    return new Date(d.setDate(diff));
}

var monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"];

function showMonthAndSelect() {
    var n = $('#Month').val();
    $('.im-monthview-month-selector option')[n].selected = true;
}

function showWeek() {
    var selectionDiv = $('.im-monthview-week-selection-div');
    var y = $('#Year').val();
    var n = $('#Month').val();
    var d = new Date(y, n, 1);
    var w = findWeekNum(d);
    var fd = getDateOfISOWeek(w, y);
    var date = fd;
    selectionDiv.empty();
    var htmlContent = '';
    var today = new Date();
    for (var i = 0; i < 6; i++) {
        date = getDateOfISOWeek(w + i, y);
        var ld = new Date();
        ld.setTime(date.getTime() + (24 * 60 * 60 * 1000) * 6);
        if (date.getMonth() == d.getMonth() || ld.getMonth() == d.getMonth()) {

        } else {
            continue;
        }
        htmlContent += "<option value=\"" + (w + i) + "\">" + ((date.getMonth() + 1) + "/" + date.getDate() + "/" + date.getFullYear()) + " - " + ((ld.getMonth() + 1) + "/" + ld.getDate() + "/" + ld.getFullYear()) + "</option>";
    }
    htmlContent = "<span class=\"input-group-addon\" style=\"width: 100px;\" id=\"searchWeek\">Week: </span><select class=\"form-control im-addexpense-minwidth im-monthview-week-selector\" aria-describedby=\"searchWeek\" onchange=\"weekChange()\">" + htmlContent + "</select>";
    selectionDiv.html(htmlContent);
    selectWeek(w);
}

function selectWeek(w) {
    var weekNum = $('#Week').val();
    $('.im-monthview-week-selector option')[weekNum - w].selected = true;
}

function showWeekSelectionLabel() {
    var selectedYear = $('#Year').val();
    var selectedWeek = weekNum = $('#Week').val();
    var fd = getDateOfISOWeek(selectedWeek, selectedYear);
    var ld = new Date();
    ld.setTime(fd.getTime() + (24 * 60 * 60 * 1000) * 6);
    var htmlContent = '<h4><span class=\"label label-default\">' + (fd.getMonth() + 1) + '/' + fd.getDate() + '/' + fd.getFullYear() + '<span> - </span>' + (ld.getMonth() + 1) + '/' + ld.getDate() + '/' + ld.getFullYear() + '</span></h4>';
    $('.im-monthview-week-selection-label-div').empty();
    $('.im-monthview-week-selection-label-div').html(htmlContent);
}

function monthChange() {
    var selectedMonth = $('.im-monthview-month-selector').find("option:selected").val();
    $('#Month').val(selectedMonth);
    var selectedYear = $('#Year').val();
    var d = new Date(selectedYear, selectedMonth, 1);
    var w = findWeekNum(d);
    $('#Week').val(w);
    showWeek();
    showWeekSelectionLabel();
}

function weekChange() {
    var selectedWeek = $('.im-monthview-week-selector').find("option:selected").val();
    $('#Week').val(selectedWeek);
    showWeekSelectionLabel();
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
    showWeek();
    showWeekSelectionLabel();
});

$('#Year').change(function () {
    showMonthAndSelect();
    monthChange();
});