// Initialize and add the map
let map;
let markers = [];
let weatherData = "";

async function initMap() {
    // The location of Iowa Capital
    const position = { lat: 41.592577723498685, lng: - 93.60370324844448 };

    const mapDiv = document.getElementById("map");
    const weatherDiv = document.getElementById("weatherData");

    const { Map } = await google.maps.importLibrary("maps");
    const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");

    // The map, centered at Iowa Capital
    map = new Map(document.getElementById("map"), {
        zoom: 8,
        center: position,
        mapId: "DefaultLocation",
    });

    addMarker( map, position, "Iowa Capital");

    map.addListener("click", (e) => {
        deleteMarkers();
        placeMarkerAndPanTo(e.latLng, map);
        console.log(JSON.stringify(e.latLng));
        getWeatherData(e.latLng.lat, e.latLng.lng);
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

function getWeatherData(lat,lng) {
    $.ajax({
        url: '/Home/GetWeatherData',
        type: 'GET',
        data: { lat: lat, lng: lng },
        success: function (data) {
            //console.log(data);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //some errror, some show err msg to user and log the error  
            alert(xhr.responseText);
        }
    });
}

initMap();