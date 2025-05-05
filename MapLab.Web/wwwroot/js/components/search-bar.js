$(document).ready(function () {
    const $searchInput = $('.search-bar input');
    const $clearButton = $('.search-bar .clear-button');

    function toggleClearButton() {
        if ($searchInput.val().length > 0) {
            $clearButton.show();
        } else {
            $clearButton.hide();
        }
    }

    $searchInput.on('input', toggleClearButton);

    $clearButton.on('click', function () {
        $searchInput.val('');
        $searchInput.trigger('input');
        $searchInput.focus();

        toggleClearButton();
    });

    $searchInput.on('keydown', function (e) {
        if (e.key === 'Enter' || e.keyCode === 13) {
            e.preventDefault();
        }
    });

    toggleClearButton();
});