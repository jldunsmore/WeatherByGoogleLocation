// Initialize and add the map
let map;
let markers = [];

async function initMap() {
    var url = location.href;
    let searchParams = new URLSearchParams(window.location.search);
    var position = { lat: 41.592577723498685, lng: -93.60370324844448 }; // The location of Iowa Capital
    if (searchParams.size > 0)
    {
        position = { lat: searchParams.get('lat'), lng: searchParams.get('lng') };
    }

    const mapDiv = document.getElementById("map");
    const weatherDiv = document.getElementById("weatherData");

    const { Map } = await google.maps.importLibrary("maps");
    const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");

    map = new Map(document.getElementById("map"), {
        zoom: 8,
        center: position,
        mapId: "DefaultLocation",
    });

    if (searchParams.size == 0) {
        addMarker(map, position, "Iowa Capital");
    }
    else {
        var latLng = new google.maps.LatLng(position.lat, position.lng);
        placeMarkerAndPanTo(latLng, map);
    }

    map.addListener("click", (e) => {
        deleteMarkers();
        getWeatherData(e);
    });
}

function addMarker( map, position, title) {
    var marker = new google.maps.marker.AdvancedMarkerElement({
        map: map,
        position: position,
        title: title
    });

    markers.push(marker);
    return marker;
}

function deleteMarkers() {
    for (let i = 0; i < markers.length; i++) {
        markers[i].setMap(null);
    }
}

function placeMarkerAndPanTo(latLng, map) {
    var marker = addMarker(map, latLng, JSON.stringify(latLng))
    map.panTo(latLng);

}

function getWeatherData(e) {
    var json = JSON.stringify(e);
    const jsonObj = JSON.parse(json);
    const position = jsonObj.latLng;
    location.href = `/Home/?lat=${position.lat}&lng=${position.lng}`;
}

initMap();