$(document).ready(function() {

$('div[menuselect]').click(function(){
$('div[menuselect]').each(function(){
$('div[menuselect]').removeClass("active");
$('div[menuselect]').attr('menuselect' ,'no');
})
$(this).addClass("active");
$(this).attr('menuselect' ,'yes')
})

// Main Nav over & our
$('div[menuselect]').mouseover(function(){
		if($(this).attr('class')!=='active')
		{$(this).addClass('active');}
		}).mouseout(function(){
		if($(this).attr('menuselect')=='no')
	{$(this).removeClass('active');}
	});


// lefthand Side Arrow Function hide
$('#controlbtn').click(function() {							
$('#v4-42').show();



$('#sidePanel').css("height","0px");
$(this).css('display','none');
   $('#inside_cotent').animate({ width: 'toggle' },300);
    // $('#product-list').removeAttr("margin-left");
	//$('#product-list').css("margin-left","20px");
 	$('#product-list').removeClass('product-listOpen');
	$('#product-list').addClass('product-list1Close').show();
});

// lefthand Side Arrow Function Show
$('#v4-42').click(function() {
   
	$('#inside_cotent').animate({ width: 'toggle' },400);
	$('#controlbtn').css('display','block');
	$('#v4-42').hide();
	$('#product-list').removeClass('product-list1Close');
    $('#product-list').addClass('product-listOpen');
  // $('#product-list').css("margin-left","300px");
});



// Seting the Height in LftsidePanle
		var hightLftsidePanle= $('#product-list').height();
		$('#sidePanel').css({'height':hightLftsidePanle+'px'});


//View Doc overlay
 $('#viewDoc').mouseover(function() {
 
 $(this).css('color','#333');
 $('#ViewDocBox').addClass('ViewDocBoxdisplay');
  $('#ViewDocBox').removeClass('ViewDocBoxhide');
 $('#viewDoc').addClass('greyLinkTxt1');
  $('#viewDoc').removeClass('greyLinkTxt');
 })  .mouseout(function()
                   {
                   $('#ViewDocBox').addClass('ViewDocBoxhide');
  $('#ViewDocBox').removeClass('ViewDocBoxdisplay');
  $('#viewDoc').addClass('greyLinkTxt');
                $('#viewDoc').css('color','#111');
  $('#viewDoc').removeClass('greyLinkTxt1');
  


});
});