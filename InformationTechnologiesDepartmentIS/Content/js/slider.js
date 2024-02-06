
$('.owl_1').owlCarousel({
    loop: false,
    margin: 8,
    responsiveClass: true,
    autoplayHoverPause: true,
    paginationSpeed: 400,
    responsive: {
        0: {
            items: 1,
            nav: true,
            loop: false
        },
        650: {
            items: 2,
            nav: true,
            loop: false
        },
        1000: {
            items: 3,
            nav: true,
            loop: false
        }
    }
})

$(document).ready(function () {
    var li = $(".owl-item li ");
    $(".owl-item li").click(function () {
        li.removeClass('active');
    });
});