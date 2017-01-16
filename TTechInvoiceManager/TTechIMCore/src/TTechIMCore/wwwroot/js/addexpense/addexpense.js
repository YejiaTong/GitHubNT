$(function () {

    var model;
    var geoCodeKey = 'AIzaSyA2suLtNXrg0wmUColpRWdhT_w93KFL248';
    var staticMapKey = 'AIzaSyDVF4IZYdJKa6Gq7_1_j1GQWbSA7JRWIE0';

    var existedLats = 0;
    var existedLngs = 0;
    var existedLocation;
    var existedImg;

    // Noah - Test if google API is ready
    /*if (google && google.maps) {
        alert('Google maps loaded');
    }
    if (!google || !google.maps) {
        alert('Not loaded yet');
    }*/

    function error(msg) {
        var errorMsg = '';
        switch (msg.code) {
            case msg.PERMISSION_DENIED:
                errorMsg = "User denied the request for Geolocation. Please use https login for the sake of security."
                break;
            case msg.POSITION_UNAVAILABLE:
                errorMsg = "Location information is unavailable."
                break;
            case msg.TIMEOUT:
                errorMsg = "The request to get user location timed out."
                break;
            case msg.UNKNOWN_ERROR:
                errorMsg = "An unknown error occurred."
                break;
        }
        alert('Error in geolocation' + '\n- ' + errorMsg);
        $('#addexpense_Address').val("Current Location");
        $('#addexpense-img').html("");
    }

    function success(position) {
        var lats = position.coords.latitude;
        var lngs = position.coords.longitude;
        var imgSrc = 'https://maps.googleapis.com/maps/api/staticmap?zoom=12&size=340x100&markers=icon:https://maps.gstatic.com/mapfiles/ms2/micons/red-dot.png|' + lats + ',' + lngs + '&key=' + staticMapKey;
        lats = Math.round(lats * 10000) / 10000;
        lngs = Math.round(lngs * 10000) / 10000;

        if (existedLats == 0 || existedLngs == 0) {
            existedLats = lats;
            existedLngs = lngs;

            $.getJSON('https://maps.googleapis.com/maps/api/geocode/json?', {
                sensor: false,
                latlng: lats + "," + lngs,
                key: geoCodeKey
            },
                function (data, textStatus) {
                    $('#addexpense_Address').val(data.results[0].formatted_address);
                    existedLocation = data.results[0].formatted_address;
                }
            );

            $('#addexpense-img').html("");
            existedImg = new Image();
            existedImg.src = imgSrc;
            $('#addexpense-img').append(existedImg);
            $('#addexpense-img').append('<a href="#" data-toggle="addexpense-tooltip" title="Please enter address if necessary!"><mark>Incorrect Address?</mark></a>');
            $('[data-toggle="addexpense-tooltip"]').tooltip();
        } else {
            if (existedLats != lats || existedLngs != existedLngs) {
                existedLats = lats;
                existedLngs = lngs;

                $.getJSON('https://maps.googleapis.com/maps/api/geocode/json?', {
                    sensor: false,
                    latlng: lats + "," + lngs,
                    key: geoCodeKey
                },
                    function (data, textStatus) {
                        $('#addexpense_Address').val(data.results[0].formatted_address);
                        existedLocation = data.results[0].formatted_address;
                    }
                );

                $('#addexpense-img').html("");
                existedImg = new Image();
                existedImg.src = imgSrc;
                $('#addexpense-img').append(existedImg);
                $('#addexpense-img').append('<a href="#" data-toggle="addexpense-tooltip" title="Please enter address if necessary!"><mark>Incorrect Address?</mark></a>');
                $('[data-toggle="addexpense-tooltip"]').tooltip();
            } else {
                $('#addexpense_Address').val(existedLocation);
                $('#addexpense-img').html("");
                $('#addexpense-img').append(existedImg);
                $('#addexpense-img').append('<a href="#" data-toggle="addexpense-tooltip" title="Please enter address if necessary!"><mark>Incorrect Address?</mark></a>');
                $('[data-toggle="addexpense-tooltip"]').tooltip();
            }
        }

        /*$.getJSON('https://maps.googleapis.com/maps/api/place/nearbysearch/json?', {
                location: lats + "," + lngs,
                key: key
            },
            function (data, textStatus) {
                alert('nice!');
            }
        );*/
    };

    $("#addexpense-modal").click(function () {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(success, error); //{ timeout: 5000 }
        } else {
            alert('Location not supported');
            $('#addexpense_Address').val("Current Location");
            $('#addexpense-img').html("");
        }
    });

    $("#addexpense-cancel").click(function () {

        $('#addexpense_Name').val("");
        $('#addexpense_Cost').val("");
        $('#addexpense_Time').val("");
        $('#addexpense_Address').val("");
        $('#addexpense-img').html("");

        $('#categoryGroupAlert').text("");
        $('#expenseNameAlert').text("");
        $('#costGroupAlert').text("");
        $('#timeGroupAlert').text("");

        $('#modalAlert').text("");
    });

    $("#addexpense-submit").click(function () {
        var $ae_ExpenseCategId = $('#addexpense_ExpenseCategId').val();
        var $ae_ExpenseCategName = $('#addexpense_ExpenseCategId option:selected').text();
        var $ae_Name = $('#addexpense_Name').val();
        var $ae_Cost = $('#addexpense_Cost').val();
        var $ae_Time = $('#addexpense_Time').val();
        var $ae_Location = $('#addexpense_Address').val();
        var $pass = true;

        if (!$ae_ExpenseCategId) {
            $('#categoryGroupAlert').text("** Please set up your Expense Category");
            $pass = false;
        } else {
            if ($ae_ExpenseCategId == 0) {
                $('#categoryGroupAlert').text("** Invalid Expense Category");
                $pass = false;
            }
            else {
                $('#categoryGroupAlert').text("");
            }
        }
        if ($ae_Name.trim() == "") {
            $('#expenseNameAlert').text("** Empty Expense Name");
            $pass = false;
        }
        else {
            $('#expenseNameAlert').text("");
        }
        if (isNaN($ae_Cost) || $ae_Cost.trim() == "") {
            $('#costGroupAlert').text("** Invalid Cost");
            $pass = false;
        }
        else {
            $('#costGroupAlert').text("");
        }
        if ($ae_Time.trim() == "") {
            $('#timeGroupAlert').text("** Invalid Time");
            $pass = false;
        }
        else {
            $('#timeGroupAlert').text("");
        }

        if ($pass) {
            var sendInfo = {
                AddExpenseExpenseCategId: $ae_ExpenseCategId,
                AddExpenseName: $ae_Name,
                AddExpenseCost: $ae_Cost,
                AddExpenseTime: $ae_Time,
                AddExpenseAddress: $ae_Location
            };

            $.ajax({
                type: "POST",
                url: "/InvoiceManager/ValidateNewExpense",
                dataType: "json",
                success: function (msg) {
                    if (msg == "Pass") {
                        if (model == null) {
                            model = {
                                Expenses: [
                                    {
                                        ExpenseCategId: $ae_ExpenseCategId,
                                        ExpenseCategName: $ae_ExpenseCategName,
                                        Name: $ae_Name,
                                        Cost: $ae_Cost,
                                        Time: $ae_Time,
                                        Address: $ae_Location
                                    }
                                ]
                            };
                        }
                        else {
                            var newItem = {
                                ExpenseCategId: $ae_ExpenseCategId,
                                ExpenseCategName: $ae_ExpenseCategName,
                                Name: $ae_Name,
                                Cost: $ae_Cost,
                                Time: $ae_Time,
                                Address: $ae_Location
                            };
                            model.Expenses.push(newItem);
                        }
                    } else {
                        $('#modalAlert').text("** " + msg);
                    }

                    showExpenseItems();

                    $("#addexpense-cancel").click();
                },

                data: sendInfo
            });

            $('#categoryGroupAlert').text("");
            $('#expenseNameAlert').text("");
            $('#costGroupAlert').text("");
            $('#timeGroupAlert').text("");

            $('#modalAlert').text("");
        }
        else {
            // Placeholder
        }
    });

    function showExpenseItems() {
        if (model != null && model.Expenses != null && model.Expenses.length != 0) {
            $('#addexpense-table tbody').empty();
            var i = 0;
            while (i < model.Expenses.length) {
                var tr = '<tr><td>' + '<button type="button" class="btn btn-sm" id="btnDelete' + i + '"><span class="glyphicon glyphicon-remove"></span></button>'
                    + '</td><td>' + model.Expenses[i].ExpenseCategName
                    + '</td><td>' + model.Expenses[i].Name
                    + '</td><td>' + '$' + model.Expenses[i].Cost
                    + '</td><td>' + model.Expenses[i].Time
                    + '</td><td>' + model.Expenses[i].Address
                    + '</td></tr>';
                $('#addexpense-table tbody').append(tr);
                $('#btnDelete' + i).bind("click", removeItem);

                i++;
            }
        } else {
            $('#addexpense-table tbody').empty();
            var tr = '<tr><td colspan="6"><div class="alert alert-info" role="alert"><p>...Add New Item...</p></div></td></tr>';
            $('#addexpense-table tbody').append(tr);
        }

        $('#addexpense-success').html('');
        $('#addexpense-warning').html('');
    }

    function removeItem() {
        var index = parseInt(this.id.replace('btnDelete', ''));

        if (model != null && model.Expenses != null) {
            model.Expenses.splice(index, 1);
            showExpenseItems();
        }
        else {
            alert("Unexpected error when removing the item")
        }
    }

    $("#addexpense-submit-form").submit(function () {
        if (model != null && model.Expenses != null && model.Expenses.length != 0) {
            var sendInfo = model;

            $.ajax({
                type: "POST",
                url: "/InvoiceManager/AddExpense",
                dataType: "json",
                success: function (msg) {
                    if (msg == "Pass") {
                        model = null;
                        showExpenseItems();
                        $('#addexpense-success').html('<p class="bg-success">New items were added successfully...</p>');
                    } else {
                        $('#addexpense-warning').html('<p class="bg-warning">' + msg + '</p>');
                    }
                },

                data: sendInfo
            });
        }
        else {
            $('#addexpense-warning').html('<p class="bg-warning">No item found</p>');
        }

        return false;
    });
});