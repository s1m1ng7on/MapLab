$('.btn-delete').on('click', function () {
    const templateId = $(this).data('id');
    const templateName = $(this).data('name');

    $('#modal-template-id').val(templateId);
    $('#modal-template-name').text(templateName);

    $('#deleteTemplateModal').modal('show');
});