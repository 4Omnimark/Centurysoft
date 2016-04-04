$(function () {
    $("#ProductSearch").autocomplete({
        source: "/AmazonInventory/ProductLookup",
        select: function (event, ui) {
            $("#ProductSearch").val(ui.item.label);
            $("#ProductID").val(ui.item.value);
            ProductSelected(ui);
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#ProductSearch").val('');
                $("#ProductID").val('');
                $("form")[0].submit();
            }
        }
    });

    $("#SalesStart").datepicker({
        onSelect: DateChanged
    });
    $("#SalesEnd").datepicker({
        onSelect: DateChanged
    });
});

function OnlyNoProductsChanged() {
    if ($("#OnlyNoProducts")[0].checked == true) {
        $("#ProductSearch").val('');
        $("#ProductID").val('');
    }

    $("form")[0].submit();
}

function isValidDate(d) {
    if (Object.prototype.toString.call(d) !== "[object Date]")
        return false;
    return !isNaN(d.getTime());
}

function isInt(value) {
    return !isNaN(parseInt(value)) && (parseFloat(value) == parseInt(value));
}

function dateString(date) {
    var dd = date.getDate();
    var mm = date.getMonth() + 1; //January is 0!
    var yyyy = date.getFullYear();

    return mm + "/" + dd + "/" + yyyy;
}

function DateChanged(dateText, inst) {
    if (inst.id == "SalesDays" && isInt($("#SalesDays")[0].value)) {
        var today = new Date();

        $("#SalesEnd")[0].value = dateString(today);
        $("#SalesStart")[0].value = dateString(new Date(today - (1000 * 60 * 60 * 24 * parseInt($("#SalesDays")[0].value))));

    }

    var SalesStart = new Date($("#SalesStart")[0].value);
    var SalesEnd = new Date($("#SalesEnd")[0].value);

    if (isValidDate(SalesStart) && isValidDate(SalesEnd)) {
        if (SalesEnd < SalesStart)
            if (inst.id == "SalesStart") {
                $("#SalesEnd")[0].value = '';
                SalesEnd = null;
            }
            else {
                $("#SalesStart")[0].value = '';
                SalesStart = null;
            }
    }

    if (isValidDate(SalesStart) && isValidDate(SalesEnd))
        $("#form")[0].submit();
}
