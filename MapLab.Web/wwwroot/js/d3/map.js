(function () {
    // Private scope variables and functions
    let isDragging = false;
    let previousTransform = { k: 1, x: 0, y: 0 };
    let currentTool = 'View'; // Default tool
    let selectedFillColor = '#fafafa'; // Default color
    let selectedIcon;
    let canEdit = false; // Will be set by loadMap

    const loadMap = async (mapJson, editPermission, selectedTool, fillColor) => {
        canEdit = editPermission; // Set permission
        currentTool = selectedTool;
        selectedFillColor = fillColor;

        const width = window.innerWidth;
        const height = window.innerHeight;

        const svg = d3
            .select('#map-container')
            .append('svg')
            .attr('width', width)
            .attr('height', height)
            .call(
                d3.zoom()
                    .scaleExtent([1, 15])
                    .on('start', () => {
                        isDragging = true;
                    })
                    .on('zoom', (e) => {
                        const currentTransform = e.transform;
                        if (currentTransform.k !== previousTransform.k) {
                            isDragging = false;
                        }
                        if (isDragging) {
                            tooltip.style('visibility', 'hidden');
                        }
                        g.attr('transform', currentTransform);
                        previousTransform = { ...currentTransform };
                    })
                    .on('end', () => {
                        isDragging = false;
                        tooltip.style('visibility', 'visible');
                    })
            );

        const g = svg.append('g');
        const tooltip = d3
            .select('body')
            .append('div')
            .attr('class', 'tooltip')
            .style('visibility', 'hidden');

        const map = JSON.parse(mapJson);

        const bounds = d3.geoBounds(map);

        const centerLongitude = (bounds[0][0] + bounds[1][0]) / 2;
        const centerLatitude = (bounds[0][1] + bounds[1][1]) / 2;

        const scaleX = width / (bounds[1][0] - bounds[0][0]);
        const scaleY = height / (bounds[1][1] - bounds[0][1]);
        const scale = Math.min(scaleX, scaleY);

        const projection = d3.geoMercator()
            .scale(scale * 40)
            .center([centerLongitude, centerLatitude])
            .translate([width / 2, height / 2]);

        const path = d3.geoPath()
            .projection(projection);

        console.log(map.features);

        const paths = g.selectAll('path')
            .data(map.features)
            .enter()
            .append('path')
            .attr('d', path)
            .attr('fill', d => d.properties.fill || '#fcba03')
            .attr('stroke', 'black')
            .attr('stroke-width', 0.5)
            .on('mouseenter', function (e, d) {
                d3.select(this)
                    .style('filter', 'brightness(85%)')
                    .style('transition', 'filter 0.1s ease-in-out');

                if (!isDragging) {
                    tooltip.style('visibility', 'visible').text(d.properties.name);
                }
            })
            .on('mousemove', (e) => {
                const tooltipWidth = tooltip.node().offsetWidth;
                const tooltipHeight = tooltip.node().offsetHeight;
                const padding = 10;
                let top = e.pageY + padding;
                let left = e.pageX + padding;
                if (top + tooltipHeight > window.innerHeight) {
                    top = e.pageY - tooltipHeight - padding;
                }
                if (left + tooltipWidth > window.innerWidth) {
                    left = e.pageX - tooltipWidth - padding;
                }
                tooltip.style('top', `${top}px`).style('left', `${left}px`);
            })
            .on('mouseout', function (e, d) {
                d3.select(this)
                    .style('filter', 'none')
                    .style('transition', 'filter 0.1s ease-in-out');
                tooltip.style('visibility', 'hidden');
            })
            .on('click', function (e, d) {
                if (canEdit) {
                    if (currentTool === "Pinpoint") {
                        const [x, y] = d3.pointer(e, this);

                        g.append("foreignObject")
                            .attr("x", x - 10) // Adjust for centering
                            .attr("y", y - 20)
                            .attr("width", 30)
                            .attr("height", 30)
                            .html(() => `<i class="bi bi-${selectedIcon || "pin-map"}"
                            style="font-size: 20px; color: ${selectedFillColor};"></i>`);
                    }
                    if (currentTool === 'Fill') {
                        d.properties.fill = selectedFillColor;
                        d3.select(this).attr('fill', d.properties.fill);
                    }
                }
            })
            .on('contextmenu', function (e, d) {
                e.preventDefault();
                if (canEdit && currentTool === 'Fill') {
                    d.properties.fill = '#fcba03'; // Reset to default fill color
                    d3.select(this).attr('fill', d.properties.fill);
                }
            });
    };

    const updateSelectedTool = (newTool) => {
        if (canEdit) {
            currentTool = newTool;
        }
    };

    const updateFillColor = (newColor) => {
        if (canEdit) {
            selectedFillColor = newColor;
        }
    };

    const updateSelectedIcon = (newIcon) => {
        if (canEdit) {
            selectedIcon = newIcon;
        }
    }

    // Expose only these functions to Blazor via a custom object
    window.mapInterop = {
        loadMap,
        updateSelectedTool,
        updateFillColor,
        updateSelectedIcon
    };
})();