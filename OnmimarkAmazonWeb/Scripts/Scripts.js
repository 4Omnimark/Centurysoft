// JavaScript source code
$(document).ready(function () {
    $(".category_box").change(function (e) {
        window.location.replace("/ManualReview/tables?table=" + $(".category_box").val());
    })

    $("#pagenum").on('keypress', function (e) {
        if (e.keyCode == 13){
            page = $("#pagenum").val();
    	
            if (parseInt(page) == NaN){
                return;
            } else if (page <= 0 || page > totalPage){
                return;
            } else if ( page == curPage){
                return;
            }
                   
            var searchUrl = window.location.search.substring(1);
            var searchItems = searchUrl.split('&');
      
            var newSearchUrl = '?';
      
            var i;
            var f = false;
      
            for (i = 0; i < searchItems.length; i++){
                searchElements = searchItems[i].split('=');
          
                if (searchElements[1] == undefined)
                    break;
          
                if (searchElements[0] == 'page'){
                    searchElements[1] = page;
                    f = true;
                }
          
                newSearchUrl += searchElements[0] + '=' + encodeURIComponent(searchElements[1]);
                if (i < searchItems.length - 1){
                    newSearchUrl += '&';
                }
            }
            if (f == false)
                newSearchUrl += '&page=' + page;
            window.location.search = newSearchUrl;
        }
    })
})

function apply(table, asin, value) {
    asin = asin.trim();
    
    $.get('/api/ManualReviewAjax/' + table + '/' + asin + '/' + value,
        function (data) {
            location.reload();
        }
    );
}