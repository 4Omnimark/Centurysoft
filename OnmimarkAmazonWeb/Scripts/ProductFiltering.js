$(function () {
    $("#FilterProductSearch").autocomplete({
        source: "/AmazonInventory/ProductLookup",
        select: function (event, ui) {
            $("#FilterProductSearch").val(ui.item.label);
            window.location = UpdateQueryString("ProductID", ui.item.value, UpdateQueryString("ProductFilter", encodeURIComponent(ui.item.label)));
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#FilterProductSearch").val('');
                window.location = UpdateQueryString("ProductID", null, UpdateQueryString("ProductFilter", null));
            }
        }
    });
});

