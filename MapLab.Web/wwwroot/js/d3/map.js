import { getCookie } from '../modules/cookies.js';

const width = window.innerWidth,
    height = window.innerHeight;

let isDragging = false;
let previousTransform = { k: 1, x: 0, y: 0 };

const svg = d3
    .select('body')
    .append('svg')
    .attr('width', width)
    .attr('height', height)
    .call(
        d3
            .zoom()
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

const projection = d3
    .geoMercator()
    .scale(10000)
    .center([25, 42.5])
    .translate([width / 2, height / 2]);

const path = d3.geoPath().projection(projection);

(async function loadAndRenderMap() {
    const mapId = getCookie('mapIdCookie');
    const mapJsonStr = await loadMap(mapId);
    const map = JSON.parse(mapJsonStr);

    g.selectAll('path')
        .data(map.features)
        .enter()
        .append('path')
        .attr('d', path)
        .attr('fill', '#fcba03')
        .attr('stroke', 'black')
        .attr('stroke-width', 0.5)
        .on('mouseenter', function (e, d) {
            d3.select(this).attr('fill', 'orange');
            if (!isDragging) {
                tooltip
                    .style('visibility', 'visible')
                    .text(d.properties.regionName);
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
        .on('mouseout', function () {
            d3.select(this).attr('fill', '#fcba03');
            tooltip.style('visibility', 'hidden');
        });
})();

async function loadMap(mapId) {
    try {
        const response = await fetch(`/api/map/${mapId}`);

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const json = await response.json();
        return json;

    } catch (error) {
        console.error('Error loading data:', error);
    }
}
