
var map = function (mapId, latlng, zoom) {

    var settings = {
        currentFocus: null,
        currentZoom: zoom,
        currentFocusRadius: 100 * 1000
    };

    var map = L.map(mapId).setView(latlng, settings.currentZoom);

    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=pk.eyJ1IjoibWFwYm94IiwiYSI6ImNpejY4NXVycTA2emYycXBndHRqcmZ3N3gifQ.rJcFIG214AriISLbB6B5aw', {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
        maxZoom: 18,
        id: 'mapbox/streets-v11'
    }).addTo(map);

    map.on('click', function (e) {
        searchRadius(e.latlng);
    });

    function searchRadius(latlng) {

        changeFocus(latlng);

        centerMap(latlng.lat, latlng.lng);

        $.get("ship/searchradius",
                {
                    lat: latlng.lat,
                    lng: latlng.lng
                })
            .done(function (response) {
                showResponse(response);
            });
    }

    function changeFocus(latlng) {

        if (settings.currentFocus) {
            map.removeLayer(settings.currentFocus);
        }

        settings.currentFocus = L.circle([latlng.lat, latlng.lng], {
            radius: settings.currentFocusRadius
        }).addTo(map);
    }

    function centerMap(lat, lng) {

        map.setView(new L.LatLng(lat, lng), settings.currentZoom);
    }

    return {
        map: map,
        centerMap: centerMap
    }
};