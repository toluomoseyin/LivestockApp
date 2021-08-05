// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Navbar Mobile
$(document).ready(function () {

    $(".cross").hide();
    $(".menu").hide();
    $(".hamburger").click(function () {
        $(".menu").slideToggle("slow", function () {
            $(".hamburger").hide();
            $(".cross").show();
        });
    });

    $(".cross").click(function () {
        $(".menu").slideToggle("slow", function () {
            $(".cross").hide();
            $(".hamburger").show();
        });
    });

    /***********OWL-CAROUSEL ***********/
    $('#testcreate-agent__btn').on('click', function (e) {
        $('.owl-carousel').trigger('stop.owl.autoplay');
    })

    $('#testcreate-agent__btn__index').on('click', function (e) {
        $('.owl-carousel').trigger('stop.owl.autoplay');
    })

    $('#playCarousel').on('click', function (e) {
        $('.owl-carousel').trigger('play.owl.autoplay');
    })

    $('.owl-carousel').owlCarousel({
        loop: true,
        margin: 10,
        nav: false,
        autoplay: true,
        autoplayTimeout: 8000,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
    })

   
    /******************************************************/

});


/* Sidebar toggle */
const toggle = function () {
    var hamMenu = document.getElementById("toggle-ham-menu");
    if (!hamMenu.classList.contains("ham-menu")) {
        hamMenu.classList.add("ham-menu");
        hamMenu.classList.remove("ham-menu-hide");
    } else {
        hamMenu.classList.remove("ham-menu");
        hamMenu.classList.add("ham-menu-hide");
    }
}

const hamburger = document.getElementById("nav-menu1");
hamburger.addEventListener("click", toggle);
/************************************************/

