﻿$(function () {

    var model;
    var key = 'AIzaSyA2suLtNXrg0wmUColpRWdhT_w93KFL248';

    function error(msg) {
        alert('Error in geolocation');
        $('#addexpense_Address').val("Current Location");
    }

    function success(position) {
        var lats = position.coords.latitude;
        var lngs = position.coords.longitude;
        lats = Math.round(lats * 1000) / 1000;
        lngs = Math.round(lngs * 1000) / 1000;

        $.getJSON('https://maps.googleapis.com/maps/api/geocode/json?key=' + key, {
                sensor: false,
                latlng: lats + "," + lngs
            },
            function (data, textStatus) {
                $('#addexpense_Address').val(data.results[0].formatted_address);
            }
        );
    };


    $("#addexpense-modal").click(function () {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(success, error);
        } else {
            alert('Location not supported');
            $('#addexpense_Address').val("Current Location");
        }
    });

    $("#addexpense-cancel").click(function () {

        $('#addexpense_Name').val("");
        $('#addexpense_Cost').val("");
        $('#addexpense_Time').val("");
        $('#addexpense_Address').val("");

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

        if ($ae_ExpenseCategId == 0) {
            $('#categoryGroupAlert').text("** Invalid Expense Category");
            $pass = false;
        }
        else {
            $('#categoryGroupAlert').text("");
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

        if ($pass)
        {
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
        else
        {
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