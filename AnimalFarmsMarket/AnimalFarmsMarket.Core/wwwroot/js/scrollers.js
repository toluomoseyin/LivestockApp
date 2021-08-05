$(document).ready(function(){
    $(".scrollerNextBtn").on('click', function () {
        var scrollerContainer = $(this).closest('.scroller-container');
        var scroller = scrollerContainer.find(".scroller");
        var allSliderItems = scroller.find('.slider-item');
        var firstSliderItemPosition = allSliderItems.eq(0).position().left;
        var secondSliderItemPosition = allSliderItems.eq(1).position().left;
        var scrollWidth = secondSliderItemPosition - firstSliderItemPosition;
        scrollerContainer.find(".scroller").animate({'scrollLeft' : '+='+scrollWidth}, 500);
    });

    $(".scrollerPrevBtn").on('click', function(){
        var scrollerContainer = $(this).closest('.scroller-container');
        var scroller = scrollerContainer.find(".scroller");
        var allSliderItems = scroller.find('.slider-item');
        var firstSliderItemPosition = allSliderItems.eq(0).position().left;
        var secondSliderItemPosition = allSliderItems.eq(1).position().left;
        var scrollWidth = secondSliderItemPosition - firstSliderItemPosition;
        scrollerContainer.find(".scroller").animate({'scrollLeft' : '-='+scrollWidth}, 500);
    });
});