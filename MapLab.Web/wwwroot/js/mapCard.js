$(document).ready(function () {
    $(".like-button").on("click", function (e) {
        e.preventDefault();

        let button = $(this);
        let mapId = button.data("id");
        let token = $("input[name='__RequestVerificationToken']").val();
        let heartIcon = button.find("i");

        $.ajax({
            url: `/map/like/${mapId}`,
            type: "POST",
            headers: {
                "RequestVerificationToken": token
            },
            success: function (response) {
                if (response.success) {
                    let likeCountElement = $(`.like-count[data-id='${mapId}']`);
                    likeCountElement.text(response.likesCount);
                    heartIcon.addClass("heart-animate");

                    let icon = button.find("i");
                    if (icon.hasClass("bi-heart")) {
                        createParticles(mapId);
                        icon.removeClass("bi-heart").addClass("bi-heart-fill text-primary");
                    } else {
                        icon.removeClass("bi-heart-fill text-primary").addClass("bi-heart");
                    }

                    setTimeout(function () {
                        heartIcon.removeClass("heart-animate");
                    }, 500);
                }
            },
            error: function (xhr, status, error) {
                if (xhr.status === 401) {
                    window.location.href = "/login?returnUrl=%2Fmaps";
                } else {
                    console.log("AJAX Error:", xhr.status, status, error);
                    alert("Error liking the map. Please try again.");
                }
            }
        });
    });

    $('.btn-edit-map').on('click', function () {
        var mapId = $(this).data('id');

        $.ajax({
            url: '/map/get/' + mapId,
            method: 'GET',
            success: function (data) {
                console.log("AJAX data:", data); // Confirm you're getting data
                console.log(data.id, data.name, data.isPublic);

                $('#editMapModal #editMapId').val(data.id); // Populate the hidden input for map ID
                $('#editMapModal #mapNameInput').val(data.name); // Populate the map name input
                $('#editMapModal #publicSwitch').prop('checked', data.isPublic);

                // Now show the modal
                $('#editMapModal').modal('show');
            },
            error: function () {
                alert('Failed to load map data.');
            }
        });
    });

    $('#editMapForm').on('submit', function (e) {
        e.preventDefault();

        // Scoped selectors inside the modal
        var mapId = $('#editMapModal #editMapId').val();  // Scoped to modal
        var name = $('#editMapModal #mapNameInput').val();  // Scoped to modal
        var isPublic = $('#editMapModal #publicSwitch').is(':checked');  // Scoped to modal
        var token = $('input[name="__RequestVerificationToken"]').val();
        var $saveBtn = $('#finishBtn');

        // Disable the save button and show the loading spinner
        $saveBtn.prop('disabled', true).html('<span class="spinner-border spinner-border-sm me-2" role="status"></span>Saving...');

        $.ajax({
            url: '/map/edit',
            method: 'POST',
            data: {
                __RequestVerificationToken: token,
                Id: mapId,
                Name: name,
                IsPublic: isPublic
            },
            success: function () {
                // Hide the modal upon successful save
                var modal = bootstrap.Modal.getInstance(document.getElementById('editMapModal'));
                modal.hide();
                location.reload(); // You can replace this with a custom success action
            },
            error: function (xhr) {
                // Handle any errors
                alert('Error: ' + xhr.responseText || 'Could not save.');
            },
            complete: function () {
                // Re-enable the save button
                $saveBtn.prop('disabled', false).html('<i class="bi bi-floppy-fill me-2"></i>Save');
            }
        });
    });

    $('.btn-delete-map').on('click', function () {
        const mapId = $(this).data('id');
        const mapName = $(this).data('name');

        $('#modal-entity-id').val(mapId);
        $('#modal-entity-name').text(mapName);

        $('#deleteMapModal').modal('show');
    });

    function createParticles(mapId) {
        const particlesContainer = document.getElementById(`particles-${mapId}`);
        const numberOfParticles = 8;

        // Clear any existing particles
        particlesContainer.innerHTML = '';

        for (let i = 0; i < numberOfParticles; i++) {
            const particle = document.createElement('div');
            particle.className = 'particle';

            // Random direction for each particle
            const angle = (i / numberOfParticles) * Math.PI * 2;
            const distance = 20 + Math.random() * 10;
            const tx = Math.cos(angle) * distance;
            const ty = Math.sin(angle) * distance;

            // Set the custom properties for the animation
            particle.style.setProperty('--tx', `${tx}px`);
            particle.style.setProperty('--ty', `${ty}px`);

            // Position particle at center initially
            particle.style.top = '50%';
            particle.style.left = '50%';
            particle.style.transform = 'translate(-50%, -50%)';

            // Apply animation
            particle.style.animation = 'particleFade 0.7s forwards';

            particlesContainer.appendChild(particle);
        }
    }
});