$(document).ready(function () {
    $('#filterForm select').on('change', function () {
        $('#filterForm').submit();
    });
});