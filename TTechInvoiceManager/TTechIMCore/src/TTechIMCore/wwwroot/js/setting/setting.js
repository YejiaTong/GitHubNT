$(function () {

    $('#settingCheckboxesTable input[type=checkbox]').click(function () {
        if ($(this).is(":checked")) {
            $('#settingCheckboxesTable').find(':checkbox').not(this).removeAttr('checked');
        }
        else {
            // Placeholder
        }
    });

    $('#settingForm').on('submit', function (e) {

        var checkboxes = $('#settingCheckboxesTable').find('input:checkbox:checked');
        if (checkboxes.length == 0) {
            alert("No selection found!");
            return false;
        } else if (checkboxes.length > 1) {
            alert("More than one selections found!");
            return false;
        }
        return true;
    });
});