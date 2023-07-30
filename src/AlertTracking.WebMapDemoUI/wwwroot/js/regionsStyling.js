const alertColor = "#e85743";
const nonAlertColor = "#aee34b";
const fillOpacity = 0.65;
const strokeWeight = 0.5;

let regionsLayer;
let crimeaLayer;
let kyivLayer;

export async function displayAlerts(map, regionsWithAlertStatuses) {
    const kyiv = regionsWithAlertStatuses.find(
        (region) => region.regionName === "м. Київ"
    );

    const crimea = regionsWithAlertStatuses.find(
        (region) => region.regionName === "Автономна Республіка Крим"
    );

    const otherRegions = regionsWithAlertStatuses.filter(
        (region) => region.regionName !== "м. Київ" && region.regionName !== "Автономна Республіка Крим"
    );

    styleKyiv(map, kyiv.isAlert);
    styleCrimea(map, crimea.isAlert);
    await styleOtherRegions(map, otherRegions);
}

async function styleOtherRegions(map, regionsWithAlerts) {
    const regions = await convertRegions(regionsWithAlerts);

    if (!regionsLayer) {
        initRegionsLayer(map);
    }

    regionsLayer.style = (featureStyleFunctionOptions) => {
        const placeFeature = featureStyleFunctionOptions.feature;
        const isAlert = regions[placeFeature.placeId];

        var isUkraine = regions.hasOwnProperty(placeFeature.placeId);

        if (isUkraine) {
            return {
                fillColor: isAlert ? alertColor : nonAlertColor,
                fillOpacity,
                strokeWeight,
                strokeColor: "black"
            };
        } else {
            return {}
        }
    };
}

function styleCrimea(map, isAlert) {
    if (!crimeaLayer) {
        initCrimeaLayer(map)
    }

    crimeaLayer.setStyle({
        fillColor: isAlert ? alertColor : nonAlertColor,
        fillOpacity,
        strokeWeight
    })
}

function styleKyiv(map, isAlert) {
    if (!kyivLayer) {
        initKyivLayer(map)
    }

    kyivLayer.setStyle({
        fillColor: isAlert ? alertColor : nonAlertColor,
        fillOpacity,
        strokeWeight
    })
}

function initRegionsLayer(map) {
    regionsLayer = map.getFeatureLayer(
        google.maps.FeatureType.ADMINISTRATIVE_AREA_LEVEL_1,
    );
}

function initCrimeaLayer(map) {
    crimeaLayer = new google.maps.Data();
    crimeaLayer.setMap(map);

    fetch('../config/crimea.json')
        .then((response) => response.json())
        .then((geoJson) => {
            crimeaLayer.addGeoJson(geoJson);
        });
}

function initKyivLayer(map) {
    kyivLayer = new google.maps.Data();
    kyivLayer.setMap(map);

    fetch('../config/kyiv.json')
        .then((response) => response.json())
        .then((geoJson) => {
            kyivLayer.addGeoJson(geoJson);
        });
}

async function convertRegions(regionsWithAlerts) {
    const regionsPlaceIdsData = await fetch("../config/regionPlaceIds.json?v=1");
    const regionsPlaceIds = await regionsPlaceIdsData.json();

    const googleMapsRegions = {};

    regionsWithAlerts.forEach((region) => {
        const placeId = regionsPlaceIds[region.regionName];
        if (placeId) {
            googleMapsRegions[placeId] = region.isAlert;
        }
    });

    return googleMapsRegions;
}