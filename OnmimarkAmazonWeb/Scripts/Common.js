$.ajaxSetup({ cache: false });

var urlParams;
(window.onpopstate = function () {
    var match,
        pl = /\+/g,  // Regex for replacing addition symbol with a space
        search = /([^&=]+)=?([^&]*)/g,
        decode = function (s) { return decodeURIComponent(s.replace(pl, " ")); },
        query = window.location.search.substring(1);

    urlParams = {};
    while (match = search.exec(query))
        urlParams[decode(match[1])] = decode(match[2]);
})();

function UpdateQueryString(key, value, url) {
    if (!url) url = window.location.href;
    var re = new RegExp("([?|&])" + key + "=.*?(&|#|$)", "gi");

    if (url.match(re)) {
        if (value)
            return url.replace(re, '$1' + key + "=" + value + '$2');
        else {
            var rtn = url.replace(re, '$2');
            return rtn.indexOf('?') == -1 ? rtn.replace('&', '?') : rtn;
        }
    }
    else {
        if (value) {
            var separator = url.indexOf('?') !== -1 ? '&' : '?',
                hash = url.split('#');
            url = hash[0] + separator + key + '=' + value;
            if (hash[1]) url += '#' + hash[1];
            return url;
        }
        else
            return url;
    }
}

function htmlEncode(str) {
    return String(str)
            .replace(/&/g, '&amp;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;');
}

function htmlDecode(value) {
    return String(value)
        .replace(/&quot;/g, '"')
        .replace(/&#39;/g, "'")
        .replace(/&lt;/g, '<')
        .replace(/&gt;/g, '>')
        .replace(/&amp;/g, '&');
}

String.prototype.format = function () {
    var formatted = this;
    for (var prop in arguments[0]) {
        var regexp = new RegExp('\\{' + prop + '\\}', 'gi');
        formatted = formatted.replace(regexp, arguments[0][prop]);
    }
    return formatted;
};

$(function () {

    $body = $("body");

//    $(document).on({
//        ajaxStart: function () { $body.addClass("loading"); },
//        ajaxStop: function () { $body.removeClass("loading"); }
//    });

});

String.prototype.formatFromProperties = function () {
    var formatted = this;
    for (var prop in arguments[0]) {
        if (arguments[0][prop] != null) {
            var regexp = new RegExp('\\{' + prop + '\\}', 'gi');
            formatted = formatted.replace(regexp, arguments[0][prop]);
        }
        else {
            var regexp = new RegExp('\\{' + prop + '\\}', 'gi');
            formatted = formatted.replace(regexp, '');
        }
    }
    return formatted;
};

function htmlFromTemplate(template_name, obj) {
    return $("#" + template_name).html().replace(/_src/g, "src").replace(/_remove_/g, "").formatFromProperties(obj);
}

function jqObjectFromTemplate(template_name, obj) {
    return $(htmlFromTemplate(template_name, obj));
}