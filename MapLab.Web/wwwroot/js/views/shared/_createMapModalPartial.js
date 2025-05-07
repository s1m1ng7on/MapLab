import MultiStepModal from '../../modules/multistep-modal.js';

function setupSearchAndFilters() {
    let typingTimer;
    const delay = 300;

    const searchInput = $('.search-bar input');
    const regionFilter = $('#continentFilter');
    const byMapLabToggle = $('#byMapLabSwitch');

    const suggestedTemplates = $('.suggested-map-templates');
    const searchResultsContainer = $('.search-results');

    function toggleVisibility(isSearching) {
        if (isSearching) {
            suggestedTemplates.hide();
            searchResultsContainer.show();
        } else {
            searchResultsContainer.hide();
            suggestedTemplates.show();
        }
    }

    function fetchTemplates() {
        const searchQuery = searchInput.val().trim();
        const isSearching = searchQuery !== '';
        toggleVisibility(isSearching);

        if (!isSearching) {
            searchResultsContainer.empty();
            return;
        }

        const region = regionFilter.val();
        const byMapLab = byMapLabToggle.is(':checked');

        searchResultsContainer.html(`
            <div class="d-flex justify-content-center align-items-center py-5">
                <div class="spinner-border text-primary" role="status" aria-hidden="true"></div>
                <span class="ms-2">Loading...</span>
            </div>
        `);

        $.ajax({
            url: '/templates/search',
            type: 'GET',
            data: {
                searchQuery,
                region,
                byMapLab
            },
            success: function (html) {
                searchResultsContainer.html(html);
                initializeSelectableCards();
            },
            error: function () {
                searchResultsContainer.html('<p class="text-danger">Failed to load results.</p>');
            }
        });
    }

    searchInput.on('input', function () {
        clearTimeout(typingTimer);
        typingTimer = setTimeout(fetchTemplates, delay);
    });

    function triggerSearchIfQueryExists() {
        if (searchInput.val().trim() !== '') {
            fetchTemplates();
        }
    }

    regionFilter.on('change', triggerSearchIfQueryExists);
    byMapLabToggle.on('change', triggerSearchIfQueryExists);
}

function setupInfiniteScroll(containerSelector, type) {
    let page = 1;
    let loading = false;

    const $container = $(containerSelector);
    const $ul = $container.find('ul');

    const $spinner = $(`
        <li class="loading-spinner d-flex align-items-center justify-content-center" style="
            width: 160px;
            height: 100%;
            list-style: none;
            flex: 0 0 auto;
        ">
            <div class="spinner-border text-primary" role="status" aria-hidden="true"></div>
        </li>
    `);

    $container.on('scroll', function () {
        const scrollLeft = $container.scrollLeft();
        const scrollWidth = $container[0].scrollWidth;
        const containerWidth = $container.width();

        if (!loading && scrollLeft + containerWidth >= scrollWidth - 100) {
            loading = true;
            page++;

            $ul.append($spinner);

            $.ajax({
                url: '/templates/load',
                type: 'GET',
                data: {
                    type,
                    page
                },
                success: function (html) {
                    $spinner.remove();
                    $ul.append(html);
                    loading = false;
                    initializeSelectableCards(); // Rebind for new cards
                },
                error: function () {
                    $spinner.remove();
                    console.error('Failed to load more templates');
                    loading = false;
                }
            });
        }
    });
}

function initializeSelectableCards() {
    $('.selectable-card').off('click').on('click', function () {
        $('.selectable-card').removeClass('selected');
        $(this).addClass('selected');

        const templateId = $(this).data('map-template-id');
        $('#SelectedMapTemplateId').val(templateId);

        const $preview = $('#selectedTemplatePreview');
        const $card = $(this).clone().removeClass('selectable-card selected').addClass('card');
        $preview.html($card);
    });
}

$(document).ready(function () {
    new MultiStepModal('#createMapModal');

    $('[data-bs-toggle="tooltip"]').tooltip();

    setupSearchAndFilters();

    setupInfiniteScroll('.recent-map-templates', 'recent');
    setupInfiniteScroll('.by-maplab-map-templates', 'by-maplab');
    setupInfiniteScroll('.featured-map-templates', 'featured');

    initializeSelectableCards();

    const $form = $('#createMapForm');

    if ($form.length) {
        $.validator.unobtrusive.parse($form);
    }

    $('#finishBtn').on('click', function (e) {
        e.preventDefault();

        if (!$form.valid()) {
            const firstError = $form.find(".input-validation-error").first();
            if (firstError.length) {
                $('html, body').animate({
                    scrollTop: firstError.offset().top - 100
                }, 500);
            }
            return;
        }

        $form.submit();
    });
});
