$(document).ready(function () {
    $('.btn-delete').on('click', function () {
        const mapId = $(this).data('id');
        const mapName = $(this).data('name');

        $('#modal-entity-id').val(mapId);
        $('#modal-entity-name').text(mapName);

        $('#deleteMapModal').modal('show');
    });
});