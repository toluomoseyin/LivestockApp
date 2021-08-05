/*----------Details page-----------*/

//Accordion
var acc = document.getElementsByClassName("accordion");
var i;

for (i = 0; i < acc.length; i++) {
    acc[i].addEventListener("click", function () {
        /* Toggle between adding and removing the "active" class,
        to highlight the button that controls the panel */
        this.classList.toggle("active-acc");

        /* Toggle between hiding and showing the active panel */
        var panel = this.nextElementSibling;
        if (panel.style.display === "block") {
            panel.style.display = "none";
        } else {
            panel.style.display = "block";
        }
    });
}


//Increase Count
document.getElementById("pluz-btn").addEventListener("click", increase);

function increase(e) {
    e.preventDefault();
    var input = document.getElementById("count").innerHTML;
    var answer = Number(input) + 1;
    document.getElementById("count").innerHTML = answer;
    document.querySelector("#add-to-cart-button").setAttribute("data-quantity", answer); 
}

//Decrease Count
document.getElementById("minus-btn").addEventListener("click", decrease);

function decrease(e) {
    e.preventDefault();
    var input = document.getElementById("count").innerHTML;
    var answer = Number(input) - 1;
    if (answer <= 1) answer = 1;
    document.getElementById("count").innerHTML = answer;
    document.querySelector("#add-to-cart-button").setAttribute("data-quantity", answer);
}

//Toggle Side-Nav on Mobile
let show_market_btn = document.getElementById("show-market");
let close_market_btn = document.getElementById("close-button");
let side_navigation = document.getElementById("side-navigation");
let show_market_btn_div = document.getElementById("show-market-div");
let side_navigation_state = false;

show_market_btn.addEventListener("click", function () {
    if (side_navigation_state == false) {
        side_navigation.style.display = "block";
        side_navigation.style.zIndex = 2;
        side_navigation_state = true;
        show_market_btn.style.display = "none";
        show_market_btn_div.style.display = "none";
    }
    else {
        side_navigation.style.display = "none";
        side_navigation_state = false;
    }
})

close_market_btn.addEventListener("click", function () {
    if (side_navigation_state == true) {
        side_navigation.style.display = "none";
        side_navigation.style.zIndex = 1;
        side_navigation_state = true;
        show_market_btn.style.display = "block";
        show_market_btn_div.style.display = "block";
    }
    else {
        side_navigation.style.display = "block";
        side_navigation_state = false;
    }
})

//Add Rating and Review
$(document).ready(function (e) {
    var final_review_value = 0;
    var selected_star;
    var star_review_value;
    //on hover over star, change its color and that of those before it
    $(".modal .fa-star").hover(function (e) {
        $(this).nextAll().addClass('text-muted');
        $(this).prevAll().addBack().removeClass('text-muted').css({ "color": "#06864D", "cursor": "pointer" });
        star_review_value = $(this).attr('data-review-value');
        $("#user-review-value").text(star_review_value);
    }, function (e) {
        $(this).prevAll().addBack().addClass('text-muted');
        $("#user-review-value").text(final_review_value);
        selected_star.prevAll().addBack().removeClass('text-muted').css("color", "#06864D");
    });
    //on clicking a star, change its color and that of those before it
    $(".modal .fa-star").click(function (e) {
        selected_star = $(this);
        $(this).nextAll().addClass('text-muted');
        $(this).prevAll().addBack().removeClass('text-muted').css("color", "#06864D");
        star_review_value = final_review_value = $(this).attr('data-review-value');
        $("#user-review-value").text(star_review_value);
        $("#rating").val(final_review_value);
    });
    //enable submit modal submit button when at least one character is inouttted into textarea
    $("#review").keyup(function (e) {
        var character_count = $(this).val().trim().length;
        if (character_count)
            $("#submit-review").prop('disabled', false);
        else
            $("#submit-review").prop('disabled', true);
    });
    $("#submit-review").click(function (e) {
        $("#review-form").submit();
    });
});


//     Image SlideShow    //
var slideIndex = 1;
showSlides(slideIndex);
// Next/previous controls
function plusSlides(n) {
    showSlides(slideIndex += n);
}
// Thumbnail image controls
function currentSlide(n) {
    showSlides(slideIndex = n);
}

function showSlides(n) {
    var i;
    var slides = document.getElementsByClassName("mySlides");
    var dots = document.getElementsByClassName("demo");
    if (n > slides.length) { slideIndex = 1 }
    if (n < 1) { slideIndex = slides.length }
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active", "");
    }
    slides[slideIndex - 1].style.display = "block";
    dots[slideIndex - 1].className += " active";
}

// Review Tab
function openTitle(evt, title) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" activate", "");
    }
    document.getElementById(title).style.display = "block";
    evt.currentTarget.className += " activate";
}

// Get the element with id="defaultOpen" and click on it
document.getElementById("defaultOpen").click();

//Disable Add to Cart
var btn = document.querySelector("#add-to-cart-button");
if (btn.getAttribute("data-available") == "True") {
    btn.disabled = false;
}
else {
    btn.disabled = true;
}

//SetTimout
window.onload = function () {
    setTimeout(function () {
        var msg = document.getElementsByClassName("message");
        msg.style.display = "none";
    }, 5000);
}