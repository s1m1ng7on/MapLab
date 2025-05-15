$(document).ready(function () {
    const $fileUpload = $('#fileUpload');
    const $filePreview = $('#filePreview');
    const $fileName = $('#fileName');
    const $fileSize = $('#fileSize');
    const $removeFile = $('#removeFile');

    $fileUpload.on('change', function () {
        const file = this.files && this.files[0];
        if (file) {
            $filePreview.removeClass('d-none');
            $fileName.text(file.name);

            const size = file.size;
            let formattedSize;
            if (size < 1024) {
                formattedSize = size + ' bytes';
            } else if (size < 1024 * 1024) {
                formattedSize = (size / 1024).toFixed(2) + ' KB';
            } else {
                formattedSize = (size / (1024 * 1024)).toFixed(2) + ' MB';
            }
            $fileSize.text(formattedSize);
        }
    });

    $removeFile.on('click', function () {
        $fileUpload.val('');
        $filePreview.addClass('d-none');
        $fileName.text('No file selected');
        $fileSize.text('');
    });
});
