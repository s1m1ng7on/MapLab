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