$(document).ready(function () {
    $('#editor').summernote({
        height: 300,
        disableResizeEditor: true,
        toolbar: [
            ['style', ['bold', 'italic', 'underline', 'strikethrough']],
            ['para', ['style', 'ul', 'ol', 'paragraph']],
            ['table', ['table']],
            ['insert', ['link', 'picture']],
            ['misc', ['undo', 'redo']]
        ],
        styleTags: [
            { title: 'Heading 1', tag: 'h1', value: 'h1' },
            { title: 'Heading 2', tag: 'h2', value: 'h2' },
            { title: 'Paragraph', tag: 'p', value: 'p' }
        ],
        fontNames: ['Exo 2'],
        fontNamesIgnoreCheck: ['Exo 2']
    });
});