$(document).on("scroll", function () {
    const $button = $(".floating-button");
    const $container = $("main");
    const containerBottom = $container[0].getBoundingClientRect().bottom;
    const viewportHeight = window.innerHeight;

    if (containerBottom <= viewportHeight) {
        // Container is visible → button stays inside it
        $button.removeClass("fixed").addClass("floating");
    } else {
        // Container bottom is below the screen → fix button to screen
        $button.removeClass("floating").addClass("fixed");
    }
});