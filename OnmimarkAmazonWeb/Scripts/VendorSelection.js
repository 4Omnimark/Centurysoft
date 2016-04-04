$(function () {
    $("#VendorSearch").autocomplete({
        source: "/Purchasing/VendorLookup",
        select: function (event, ui) {
            if (ui.item.label == "-- New Vendor --") {
                $('#newVendorName').val($("#VendorSearch")[0].value);
                $('#newVendorDialog').dialog('open');
            }
            else {

                $("#VendorSearch").val(ui.item.label);
                $("#VendorID").val(ui.item.value);
                VendorSelected(ui);
            }

            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#VendorSearch").val('');
                $("#VendorID").val('');
            }
        }
    });

    $('#newVendorDialog').dialog({
        modal: true,
        autoOpen: false,
        buttons: {
            'Cancel': function () {
                $(this).dialog('close');
            },
            'Accept': function () {
                $.getJSON(
                    "/Purchasing/NewVendor?Name=" + $('#newVendorName')[0].value,
                    null,
                    function (json) {
                        $("#VendorSearch").val($('#newVendorName')[0].value);
                        $("#VendorID").val(json.NewID);
                        VendorSelected(null);
                        $('#newVendorDialog').dialog('close');
                    }
                );
            }
        }
    });

});

