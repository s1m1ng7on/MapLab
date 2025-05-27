$(document).ready(function () {
    const $form = $("form[method='get']");
    const $pageInput = $form.find("input[name='page']");

    $form.on("submit", function () {
        if ($pageInput.length) {
            $pageInput.val(1);
        }
    });
});
