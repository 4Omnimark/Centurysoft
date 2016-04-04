var parmShowStockFor;
var parmOnlyNoProducts;
var parmOnlyLowInventory;
var parmOnlyUSA;
var parmFilterBy;
var parmProductID;
var parmVendorID;
var parmSKU;
var parmASIN;
var parmSalesStart;
var parmSalesEnd;
var dataTotalInventoryValue;
var dataAnyCostMissing;
var dataMessage;
var dataProductName;
var dataVendorName;
var queryString;

(window.onpopstate = function () {
    var match,
        pl = /\+/g,  // Regex for replacing addition symbol with a space
        search = /([^&=]+)=?([^&]*)/g,
        decode = function (s) { return decodeURIComponent(s.replace(pl, " ")); },
        query = window.location.search.substring(1);

    queryString = {};
    while (match = search.exec(query))
        queryString[decode(match[1])] = decode(match[2]);
})();

function parmsToQueryString() {
    var rtn = "";

    if (parmShowStockFor != null && parmShowStockFor != "")
        rtn += (rtn == "" ? "?" : "&") + "ShowStockFor=" + encodeURIComponent(parmShowStockFor);

    if (parmASIN != null && parmASIN != "")
        rtn += (rtn == "" ? "?" : "&") + "ASIN=" + encodeURIComponent(parmASIN);

    if (parmProductID != null && parmProductID != "")
        rtn += (rtn == "" ? "?" : "&") + "ProductID=" + encodeURIComponent(parmProductID);

    if (parmSKU != null && parmSKU != "")
        rtn += (rtn == "" ? "?" : "&") + "SKU=" + encodeURIComponent(parmSKU);

    if (parmVendorID != null && parmVendorID != "")
        rtn += (rtn == "" ? "?" : "&") + "VendorID=" + encodeURIComponent(parmVendorID);

    if (parmFilterBy != null && parmFilterBy != "")
        rtn += (rtn == "" ? "?" : "&") + "FilterBy=" + encodeURIComponent(parmFilterBy);

    if (parmOnlyNoProducts != null && parmOnlyNoProducts != "")
        rtn += (rtn == "" ? "?" : "&") + "OnlyNoProducts=" + encodeURIComponent(parmOnlyNoProducts);

    if (parmOnlyLowInventory != null && parmOnlyLowInventory != "")
        rtn += (rtn == "" ? "?" : "&") + "OnlyLowInventory=" + encodeURIComponent(parmOnlyLowInventory);

    if (parmOnlyUSA != null && parmOnlyUSA != "")
        rtn += (rtn == "" ? "?" : "&") + "OnlyUSA=" + encodeURIComponent(parmOnlyUSA);

    if (parmSalesStart != null && parmSalesStart != "")
        rtn += (rtn == "" ? "?" : "&") + "SalesStart=" + encodeURIComponent($.datepicker.formatDate("m/d/yy", parmSalesStart));

    if (parmSalesEnd != null && parmSalesEnd != "")
        rtn += (rtn == "" ? "?" : "&") + "SalesEnd=" + encodeURIComponent($.datepicker.formatDate("m/d/yy", parmSalesEnd));

    return rtn;
}

function settingsToQueryString() {
    var rtn = "";

    if ($("#ShowStockFor").val() != "")
        rtn += (rtn == "" ? "?" : "&") + "ShowStockFor=" + encodeURIComponent($("#ShowStockFor").val());

    if ($("#ASIN").val() != "")
        rtn += (rtn == "" ? "?" : "&") + "ASIN=" + encodeURIComponent($("#ASIN").val());

    if ($("#ProductID").val() != "")
        rtn += (rtn == "" ? "?" : "&") + "ProductID=" + encodeURIComponent($("#ProductID").val());

    if ($("#SKU").val() != "")
        rtn += (rtn == "" ? "?" : "&") + "SKU=" + encodeURIComponent($("#SKU").val());

    if ($("#VendorID").val() != "")
        rtn += (rtn == "" ? "?" : "&") + "VendorID=" + encodeURIComponent($("#VendorID").val());

    if ($("#FilterBy").val() != "")
        rtn += (rtn == "" ? "?" : "&") + "FilterBy=" + encodeURIComponent($("#FilterBy").val());

    if ($("#OnlyNoProducts")[0].checked)
        rtn += (rtn == "" ? "?" : "&") + "OnlyNoProducts=true";

    if ($("#OnlyLowInventory")[0].checked)
        rtn += (rtn == "" ? "?" : "&") + "OnlyLowInventory=true";

    if ($("#OnlyUSA")[0].checked)
        rtn += (rtn == "" ? "?" : "&") + "OnlyUSA=true";

    if ($("#SalesStart").val() != "")
        rtn += (rtn == "" ? "?" : "&") + "SalesStart=" + encodeURIComponent($("#SalesStart").val());

    if ($("#SalesEnd").val() != "")
        rtn += (rtn == "" ? "?" : "&") + "SalesEnd=" + encodeURIComponent($("#SalesEnd").val());

    return rtn;
}

function queryStringToParms() {

    if (queryString["ShowStockFor"] != null)
        parmShowStockFor = queryString["ShowStockFor"];

    if (queryString["ASIN"] != null)
        parmASIN = queryString["ASIN"];

    if (queryString["ProductID"] != null)
        parmProductID = queryString["ProductID"];

    if (queryString["SKU"] != null)
        parmSKU = queryString["SKU"];

    if (queryString["VendorID"] != null)
        parmASIN = queryString["VendorID"];

    if (queryString["OnlyNoProducts"] != null)
        parmOnlyNoProducts = queryString["OnlyNoProducts"];

    if (queryString["OnlyLowInventory"] != null)
        parmOnlyLowInventory = queryString["OnlyLowInventory"];

    if (queryString["OnlyUSA"] != null)
        parmOnlyUSA = queryString["OnlyUSA"];

    if (queryString["SalesStart"] != null)
        parmSalesStart = new Date(queryString["SalesStart"]);

    if (queryString["SalesEnd"] != null)
        parmSalesEnd = new Date(queryString["SalesEnd"]);

}

function loadParmsFromData(data)
{
    parmShowStockFor = data.ShowStockFor;
    parmASIN = data.ASIN;
    parmProductID = data.ProductID;
    parmSKU = data.SKU;
    parmVendorID = data.VendorID;
    parmOnlyNoProducts = data.OnlyNoProducts;
    parmOnlyLowInventory = data.OnlyLowInventory;
    parmOnlyUSA = data.OnlyUSA;
    parmSalesStart = new Date(data.SalesStart.match(/\d+/)[0] * 1);
    parmSalesEnd = new Date(data.SalesEnd.match(/\d+/)[0] * 1);
    dataProductName = data.ProductName;
    dataVendorName = data.VendorName;
}

function loadSettingsFromParms()
{
    if (parmShowStockFor != null)
        $("#ShowStockFor").val(parmShowStockFor);

    if (parmASIN != null)
        $("#ASIN").val(parmASIN);

    if (parmProductID != null)
    {
        $("#ProductID").val(parmProductID);
        $("#ProductSearch").val(dataProductName);
    }

    if (parmSKU != null)
        $("#SKU").val(parmSKU);

    if (parmVendorID != null)
    {
        $("#VendorID").val(parmVendorID);
        $("#VendorSearch").val(dataVendorName);
    }

    if (parmOnlyNoProducts != null)
        $("#OnlyNoProducts")[0].checked = parmOnlyNoProducts;

    if (parmOnlyLowInventory != null)
        $("#OnlyLowInventory")[0].checked = parmOnlyLowInventory;

    if (parmOnlyUSA != null)
        $("#OnlyUSA")[0].checked = parmOnlyUSA;

    if (parmSalesStart != null)
        $("#SalesStart").val($.datepicker.formatDate("m/d/yy", parmSalesStart));

    if (parmSalesEnd != null)
        $("#SalesEnd").val($.datepicker.formatDate("m/d/yy", parmSalesEnd));

}

function initTable()
{

    var tblData = $("#tblData");

    //tblData.stickyTableHeaders();
    setSizes();

    if (dataMessage != null && dataMessage != "")
        alert(dataMessage);

    $("#totalInventoryValue").html("$" + $.formatNumber(dataTotalInventoryValue, {format:"###,###,###.00", locale:"us"}));
    $("#totalInventoryValue")[0].style.color = dataAnyCostMissing ? "red" : "";

    $(".onorder").tooltip({
        relative: true,
        bodyHandler: function () {
            var onorder = $(this).attr("onorder");

            var lines = onorder.split("*LINEBREAK*");
            var tiphtml = "<h3>Products On Order</h3>";

            for (var i = 0; i < lines.length; i++) {
                var cols = lines[i].split("*COLUMNBREAK*");
                tiphtml += cols[1] + ": " + parseInt(cols[2]).toFixed(0).toString() + "<br />";
            }

            var tip = $("<div />").html(tiphtml);

            return tip;
        },
        showURL: false
    });

    $(".stockqty").tooltip({
        relative: true,
        bodyHandler: function () {
            var tip = $("<div style='width:500px; min-height:50px;' />").html("loading...");

            var stocktimestamp = $(this).attr("stocktimestamp");
            var ASIN = $(this).attr("ASIN");

            $.getJSON(
                    "/AmazonInventory/StockDetails/" + $(this).attr("ASIN") + "?store=" + $(this).attr("store"),
                    null,
                    function (json) {

                        var tiptxt = "<b>" + ASIN + "</b> - <b>" + json.StoreName + "</b> - Updated on: " + stocktimestamp + "<br /><br /><table><tr><th>SKU</th><th>InStockQty</th><th>TotalQty</th></tr>";

                        $.each(json.Stock, function () {
                            tiptxt = tiptxt + "<tr><td nowrap>" + this.SKU + "</td><td align='right' nowrap>" + this.InStockQty + "</td><td align='right' nowrap>" + this.TotalQty + "</td></tr>";
                        });

                        tiptxt += "</table>";

                        tip.html(tiptxt);
                    }
                );

            return tip;
        },
        showURL: false
    });

    $(".qtysold").tooltip({
        bodyHandler: function () {

            var html = "<b>" + $(this).attr("ASIN") + "</b><br /><br />" +
                    "<table>" +
                    "   <tr><th>Store</th><th>Qty Sold</th></tr>" +
                    "   <tr><td>Brandzilla</td><td>" + $(this).attr("Brandzilla") + "</td></tr>" +
                    "   <tr><td>FiveStar</td><td>" + $(this).attr("FiveStar") + "</td></tr>" +
                    "   <tr><td>Vitality</td><td>" + $(this).attr("Vitality") + "</td></tr>";

            if (!$("#OnlyUSA")[0].checked)
                html += "   <tr><td>Nutramart</td><td>" + $(this).attr("Nutramart") + "</td></tr>";

            html += "</table>"

            var tip = $("<div style='width:500px; min-height:50px;' />").html(html);

            return tip;
        },
        showURL: false
    });

}

function FilterByVendor(id, name) {
    $("#VendorSearch").val(name);
    $("#VendorID").val(id);
    updateTable();
}

function UpdateInventory(store, asin) {
    $("#stockdiv_" + asin + "_" + store)[0].style.display = "none";

    $.getJSON(
        "/AmazonInventory/UpdateInventory/" + asin + "?store=" + store,
        null,
        function (json) {

            $("#stockdiv_" + asin + "_" + store)[0].style.display = "";
            $("#refreshbtn_" + asin + "_" + store)[0].style.display = "none";

            if (json.Error != null) {
                $("#stock_" + asin + "_" + store)[0].style.color = "red";
                $("#stock_" + asin + "_" + store)[0].innerText = json.Error;
            }
            else {
                $("#stock_" + asin + "_" + store)[0].style.color = "black";
                $("#stock_" + asin + "_" + store)[0].innerText = json.Stock;
            }
        }
    );

}

function ProductSelected(ui) {
    $("#OnlyNoProducts")[0].checked = false;
    ClearFiltersExcept("Product");
    updateTable();
}

function OnlyLowInventoryChanged() {
    updateTable();
}

function OnlyUSAChanged() {
    updateTable();
}

function setSizes() {

    var top = $("#divTable").offset().top - $("#pageTitle").offset().top;
    $("#divTable").height($(window).height() - top - 40);

    $('html, body').animate({
        scrollTop: $("#pageTitle").offset().top - 20
    }, 1000);

}

$(function () {
    
    queryStringToParms();

    $(window).resize(function () { setSizes(); });
    setSizes();

    showFilterDiv("Product");

    if ($("#VendorID").val() != '')
        showFilterDiv("Vendor");

    if ($("#ASIN").val() != '')
        showFilterDiv("ASIN");

    if ($("#SKU").val() != '')
        showFilterDiv("SKU");

    $("#ShowStockFor").change(function() {
        updateTable();
    });

    $("#FilterBy").change(function () {
        showFilterDiv(this.value);

        if (this.value == "") {
            ClearFiltersExcept("");
            updateTable();
        }

    });

    $("#VendorSearch").autocomplete({
        source: "/AmazonInventory/VendorLookup",
        select: function (event, ui) {
            $("#VendorSearch").val(ui.item.label);
            $("#VendorID").val(ui.item.value);
            ClearFiltersExcept("Vendor");
            updateTable();
            return false;
        },
        change: function (event, ui) {
            if (ui.item == null) {
                $("#VendorSearch").val('');
                $("#VendorID").val('');
                updateTable();
            }
        }
    });

    $("#ASIN").change(function () {
        ClearFiltersExcept("ASIN")
        updateTable();
    });

    $("#SKU").change(function () {
        ClearFiltersExcept("SKU")
        updateTable();
    });

    $("#btnInitialLoad").click(function() {
        loadTable(parmsToQueryString());
    });

    $("#OnlyNoProducts").change(function() {
        ClearFiltersExcept("");
        updateTable();
    });

    $("#OnlyLowInventory").change(function() {
        updateTable();
    });

    $("#OnlyUSA").change(function() {
        updateTable();
    });

    if (document.location.search != "")
        loadTable(parmsToQueryString());

});

function updateTable()
{
    loadTable(settingsToQueryString());
}

function loadTable(qs)
{
    $("#divTable").html("<img src='/Content/Images/loading.gif' />");

    var script = "KnownASINsWithInventoryAndSales";

    if (qs != null)
        script += qs;

    $('#blockSettings').find('input, textarea, button, select').attr('disabled','disabled');

    $.ajax({
        url: script,
        success: function (data) {
            $("#divTable").html(data.html);

            dataTotalInventoryValue = data.TotalInventoryValue;
            dataAnyCostMissing = data.AnyCostMissing;
            dataMessage = data.Message;

            loadParmsFromData(data);
            loadSettingsFromParms();

            $("#SalesDays").val(data.SalesDays);

            window.history.pushState("", "Inventory Items", document.location.origin + document.location.pathname + parmsToQueryString());

            $('#blockSettings').find('input, textarea, button, select').attr('disabled',null);

            initTable();
        }
    });
}

function showFilterDiv(name) {
    $(".VisibleFilter").each(function () {
        this.style.display = "none";
        $(this).removeClass("VisibleFilter");
    });
    if (name != '') {
        var divFilter = $("#div" + name + "Filter");
        divFilter.addClass("VisibleFilter");
        divFilter[0].style.display = "";
    }
    $("#FilterBy").val(name);
}

function ClearFiltersExcept(name) {
    if (name != "SKU")
        $("#SKU").val('');

    if (name != "ASIN")
        $("#ASIN").val('');

    if (name != "Product")
        $("#ProductID").val('');

    if (name != "Vendor")
        $("#VendorID").val('');
}


