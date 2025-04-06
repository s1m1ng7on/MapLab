$(document).ready(function () {
    AOS.init();

    initFloatingAnimation();
    initYouTubeModal();
});

function initFloatingAnimation() {
    const $heroImage = $('.floating-animation');

    if ($heroImage.length) {
        $heroImage.on('mousemove', function (e) {
            const offset = $(this).offset();
            const width = $(this).outerWidth();
            const height = $(this).outerHeight();

            const x = e.pageX - offset.left;
            const y = e.pageY - offset.top;

            const moveX = (x - width / 2) / 20;
            const moveY = (y - height / 2) / 20;

            $(this).css('transform', `translate(${moveX}px, ${moveY}px)`);
        });

        $heroImage.on('mouseleave', function () {
            $(this).css({
                'transform': 'translate(0, 0)',
                'transition': 'transform 0.5s ease'
            });
        });
    }
}

function initYouTubeModal() {
    const $youtubeModal = $('#youtubeModal');
    const $youtubeIframe = $('#youtubeIframe');

    $youtubeModal.on('show.bs.modal', function (e) {
        const videoUrl = $(e.relatedTarget).data('video');
        $youtubeIframe.attr('src', videoUrl + '?autoplay=1');
    });

    $youtubeModal.on('hidden.bs.modal', function () {
        $youtubeIframe.attr('src', '');
    });
}