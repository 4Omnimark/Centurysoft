var tableOffset;
var $header;
var $fixedHeader;

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

function position(table) {
    var oldHeight = table.rows[0].clientHeight;
    var widths = Array();
    for (var i = 0; i < table.rows[1].cells.length; i++) {
        widths[i] = table.rows[0].cells[0].clientWidth;
    }

    var rowWidth = table.rows[1].clientWidth;

    table.rows[0].style.position = "absolute";
    table.rows[0].style.top = ($("#divReport").offset().top).toString() + "px";
    
    //if ($(table).width() > $(table.rows[0]).width())
    $(table.rows[0]).width(rowWidth);
    
    $(table).css('margin-top', oldHeight.toString() + "px");
    //$(table.rows[0].children).each(function () { $(this).css('overflow', 'hidden'); });
    for (var row = 0; row < 1; row++) {
        for (var col = 0; col < widths.length; col++) {
            table.rows[row].cells[col].style.width = widths[col] + "px";
        }
    }
}

function max(num1, num2) { return (num1 > num2) ? num1 : num2; }

function setSizes() {

    var top = $("#divReport").offset().top - $("#pageTitle").offset().top;
    $("#divReport").height($(window).height() - top - 40);

    $('html, body').animate({
        scrollTop: $("#pageTitle").offset().top - 20
    }, 100);

}

function enableSettings(yes) {
    $("#blockSettings").find("input, textarea, button, select").attr("disabled", yes ? null : "disabled");
}

function loadReport(script) {
    $("#divReport").html("<img src='/Content/Images/loading.gif' />");

    enableSettings(false);

    $.ajax({
        url: script,
        success: function (data) {
            $("#divReport").html("<div id='header-fixed' style='display:none;'></div>" + data.html);
            //window.history.pushState("", "Inventory Items", document.location.origin + document.location.pathname + parmsToQueryString());

            enableSettings(true);

            $table = $("#divReport table").clone();

            $table.attr("id", "header-fixed-table");
            $fixedHeader = $("#header-fixed").append($table);

            $("#header-fixed-table")[0].style["tableLayout"] = "fixed";

            var cols = $("#divReport table tr:last").children();

            $("#header-fixed tr:first").children().width(function (i, val) {
                return cols.eq(i)[0].offsetWidth-11; // paddingleft + paddingright + 1
            });

            $("#header-fixed-table tr:gt(0)").remove();

            $("#header-fixed-table")[0].style.position = "absolute";
            $("#header-fixed-table")[0].style.top = ($("#divReport").offset().top).toString() + "px";
            $("#header-fixed")[0].style.display = "";



            //position($('#divReport')[0].children[0]);
            $("#divReport table").click(function () { setSizes(); });

            reportLoaded(data);
        }
    });
}

function jsonDateToDate(jsonDate) {
    return new Date(jsonDate.match(/\d+/)[0] * 1);
}

function jsonDateToString(jsonDate) {
    return $.datepicker.formatDate("m/d/yy", jsonDateToDate(jsonDate));
}

function isValidDate(d) {
    if (Object.prototype.toString.call(d) !== "[object Date]")
        return false;
    return !isNaN(d.getTime());
}

function StartEndDateChanged(dateText, inst) {

    var SalesStart = new Date($("#StartDate")[0].value);
    var SalesEnd = new Date($("#EndDate")[0].value);

    if (isValidDate(SalesStart) && isValidDate(SalesEnd)) {
        if (SalesEnd < SalesStart)
            if (inst.id == "StartDate") {
                $("#EndDate")[0].value = '';
                SalesEnd = null;
            }
            else {
                $("#StartDate")[0].value = '';
                SalesStart = null;
            }
    }

    if (isValidDate(SalesStart) && isValidDate(SalesEnd))
        StartEndDateChangedSuccess();
}

$(function () {
    $(window).resize(function () { setSizes(); });
    setSizes();

});