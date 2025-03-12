(function () {
    let isDragging = false;
    let previousTransform = { k: 1, x: 0, y: 0 };
    let currentTool = 'View'; // Default tool
    let selectedFillColor = '#fafafa'; // Default color
    let selectedIcon;
    let canEdit = false;
    let mapJsonObj;
    const defaultFill = '#fcba03';
    let dotNetReference;
    let svg;

    const loadMap = async (_dotNetReference, mapTemplateJson, mapJson, editPermission, selectedTool, fillColor) => {
        dotNetReference = _dotNetReference;
        canEdit = editPermission;
        currentTool = selectedTool;
        selectedFillColor = fillColor;

        const width = window.innerWidth;
        const height = window.innerHeight - document.querySelector('nav.navbar').offsetHeight;

        svg = d3
            .select('#map-container')
            .append('svg')
            .attr('width', width)
            .attr('height', height)
            .call(
                d3.zoom()
                    .scaleExtent([1, 15])
                    .on('start', () => { isDragging = true; })
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
            .attr('class', 'map-tooltip')
            .style('visibility', 'hidden');

        const map = JSON.parse(mapTemplateJson);
        mapJsonObj = JSON.parse(mapJson);

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

        const path = d3.geoPath().projection(projection);

        g.selectAll('path')
            .data(map.features)
            .enter()
            .append('path')
            .attr('d', path)
            .attr('fill', d => {
                const feature = mapJsonObj.features.find(f => f.type === 'Feature' && f.properties.name === d.properties.name);
                const legendItem = mapJsonObj.legend.find(l => l.type === 'Region' && l.id === feature?.properties.id);
                return legendItem?.fill || defaultFill;
            })
            .attr('stroke', 'black')
            .attr('stroke-width', 0.5);

        const createPinpointElement = (x, y, icon, fill) =>
            g.append("foreignObject")
                .attr("x", x - 10)
                .attr("y", y - 20)
                .attr("width", 30)
                .attr("height", 30)
                .html(() => `<i class="bi bi-${icon || "pin-map"}"
                    style="font-size: 20px; color: ${fill}; pointer-events: none;"></i>`);

        // ** Event Delegation on `<g>` **
        g.on('mousemove', (e) => {
            if (!e.target.matches('path')) return;
            const tooltipWidth = tooltip.node().offsetWidth;
            const tooltipHeight = tooltip.node().offsetHeight;
            const padding = 10;
            let top = e.pageY + padding;
            let left = e.pageX + padding;
            if (top + tooltipHeight > window.innerHeight) top = e.pageY - tooltipHeight - padding;
            if (left + tooltipWidth > window.innerWidth) left = e.pageX - tooltipWidth - padding;
            tooltip.style('top', `${top}px`).style('left', `${left}px`);
        });

        g.on('mouseenter', (e) => {
            if (!e.target.matches('path')) return;
            d3.select(e.target)
                .style('filter', 'brightness(85%)')
                .style('transition', 'filter 0.1s ease-in-out');

            if (!isDragging) {
                const regionName = e.target.__data__.properties.name;
                const feature = mapJsonObj.features.find(f => f.properties.name === regionName);

                // Ensure the feature and legend are valid before accessing properties
                const regionLegendName = feature
                    ? mapJsonObj.legend.find(l => l.type === 'Region' && l.id === feature.properties.id)?.name
                    : null;

                // Display tooltip with conditional <h2>
                tooltip.style('visibility', 'visible').html(`
                    <h1>${regionName}</h1>
                    ${regionLegendName ? `<h2>${regionLegendName}</h2>` : ''}
                `);
            }

        }, true);

        g.on('mouseout', (e) => {
            if (!e.target.matches('path')) return;
            d3.select(e.target)
                .style('filter', 'none')
                .style('transition', 'filter 0.1s ease-in-out');
            tooltip.style('visibility', 'hidden');
        }, true);

        g.on('click', (e) => {
            if (!canEdit || !e.target.matches('path')) return;
            const d = e.target.__data__;

            if (currentTool === "Pinpoint") {
                const [x, y] = d3.pointer(e, g.node());
                const [longitude, latitude] = projection.invert([x, y]);
                createPinpointElement(x, y, selectedIcon, selectedFillColor);
                dotNetReference.invokeMethodAsync("Pinpoint", "", selectedIcon, selectedFillColor, longitude, latitude);
            }

            if (currentTool === 'Fill') {
                d.properties.fill = selectedFillColor;
                d3.select(e.target).attr('fill', d.properties.fill);
                dotNetReference.invokeMethodAsync("Fill", d.properties.name, d.properties.fill);
            }
        });

        g.on('contextmenu', (e) => {
            e.preventDefault();
            const d = e.target.__data__;

            if (currentTool === 'Fill' && e.target.matches('path')) {
                d.properties.fill = defaultFill;
                d3.select(e.target).attr('fill', d.properties.fill);
                dotNetReference.invokeMethodAsync("Fill", d.properties.name, null);
            }
            if (currentTool === 'Pinpoint' && e.target.matches('foreignObject')) {
                const point = e.target;

                const [x, y] = [
                    parseFloat(point.getAttribute('x')) + 10,
                    parseFloat(point.getAttribute('y')) + 20
                ];

                const [longitude, latitude] = projection.invert([x, y]);

                d3.select(point).remove();
                dotNetReference.invokeMethodAsync("Pinpoint", "", null, null, longitude, latitude);
            }
        });

        mapJsonObj.features
            .filter(f => f.geometry?.type === "Point")
            .forEach(pointFeature => {
                const [longitude, latitude] = pointFeature.geometry.coordinates;
                const [x, y] = projection([longitude, latitude]);

                const legendItem = mapJsonObj.legend.find(l => l.id === pointFeature?.properties.id);
                const [icon, fill] = [legendItem.icon, legendItem.fill];

                createPinpointElement(x, y, icon, fill);
            });
    };

    const shareMap = (title, text, url) => {
        if (navigator.share) {
            navigator.share({
                title: title,
                text: text,
                url: url
            }).catch(err => console.log("Sharing failed", err));
        } else {
            console.log("Web Share API not supported.");
        }
    }

    const updateSelectedTool = (newTool) => { if (canEdit) currentTool = newTool; };
    const updateFillColor = (newColor) => { if (canEdit) selectedFillColor = newColor; };
    const updateSelectedIcon = (newIcon) => { if (canEdit) selectedIcon = newIcon; };

    const downloadMap = async () => {
        const createStyleElementFromCSS = () => {
            // Get the first stylesheet loaded in the document
            const sheet = document.styleSheets[0];
            const styleRules = [];

            // Collect all CSS rules
            try {
                for (let i = 0; i < sheet.cssRules.length; i++) {
                    styleRules.push(sheet.cssRules.item(i).cssText);
                }
            } catch (e) {
                console.error("Error accessing CSS rules (possible cross-origin issue):", e);
                return document.createElement('style'); // Return empty style element as fallback
            }

            const style = document.createElement('style');
            style.type = 'text/css';
            style.appendChild(document.createTextNode(styleRules.join(' ')));
            return style;
        };

        // Get the original SVG element
        const svgElement = document.querySelector('#map-container svg');

        // Clone the SVG to avoid modifying the original
        const svgClone = svgElement.cloneNode(true);

        // Embed styles into the cloned SVG
        const style = createStyleElementFromCSS();
        svgClone.insertBefore(style, svgClone.firstChild);

        // Ensure SVG has explicit width and height attributes to capture the full map
        const bbox = svgElement.getBBox(); // Get the bounding box of the full SVG content
        svgClone.setAttribute('width', bbox.width);
        svgClone.setAttribute('height', bbox.height);
        svgClone.setAttribute('viewBox', `${bbox.x} ${bbox.y} ${bbox.width} ${bbox.height}`);

        // Serialize the cloned SVG
        const data = new XMLSerializer().serializeToString(svgClone);
        const svgBlob = new Blob([data], { type: 'image/svg+xml;charset=utf-8' });
        const url = URL.createObjectURL(svgBlob);

        // Create an image to load the SVG
        const img = new Image();
        img.width = bbox.width;
        img.height = bbox.height;

        // Wait for the image to load
        await new Promise((resolve, reject) => {
            img.onload = resolve;
            img.onerror = reject;
            img.src = url;
        });

        // Create a canvas with the full dimensions of the map
        const canvas = document.createElement('canvas');
        canvas.width = bbox.width;
        canvas.height = bbox.height;
        const ctx = canvas.getContext('2d');

        // Draw the image onto the canvas
        ctx.drawImage(img, 0, 0, bbox.width, bbox.height);

        // Clean up the object URL
        URL.revokeObjectURL(url);

        // Trigger the PNG download
        const a = document.createElement('a');
        a.download = 'map.png';
        document.body.appendChild(a);
        a.href = canvas.toDataURL('image/png');
        a.click();
        a.remove();
    };

    window.addEventListener('resize', () => {
        const width = window.innerWidth;
        const height = window.innerHeight - document.querySelector('nav.navbar').offsetHeight;

        svg.attr('width', width).attr('height', height);
    });

    window.addEventListener("keydown", function (e) {
        if (canEdit && e.ctrlKey && e.key === 's') {
            e.preventDefault();
            dotNetReference.invokeMethodAsync('Save');
        }
    });

    window.mapInterop = {
        loadMap,
        shareMap,
        updateSelectedTool,
        updateFillColor,
        updateSelectedIcon,
        downloadMap
    };
})();
