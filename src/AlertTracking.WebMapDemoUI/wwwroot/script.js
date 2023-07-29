import { getRegionsWithAlerts } from './js/alertsTracking.js';
import { displayAlerts } from './js/regionsStyling.js';

let map;

async function initMap() {
    const { Map } = await google.maps.importLibrary("maps");

    map = new Map(document.getElementById("map"), {
        center: {
            lat: 48.38,
            lng: 31.17
        },
        zoom: 6.4,
        mapId: "bee0e30a1a5e6771",
        disableDefaultUI: true
    });

    await trackRegionAlerts();
}

async function trackRegionAlerts() {
    const regionsWithAlerts = await getRegionsWithAlerts(); 

    displayAlerts(map, regionsWithAlerts);
}

initMap();

setInterval(trackRegionAlerts, 500);