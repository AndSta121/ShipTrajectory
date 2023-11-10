
function showShipTrajectoriesRaw() {

	$.get("shipraw/trajectories", function (response) {

        showResponseRaw(response);
    });

}

function showShipTrajectories() {

	$.get("ship/trajectories", function (response) {

		showResponse(response);
	});
}

function getColor(shipType) {
	switch (shipType) {
		case 'Military':
			return 'green';
		case 'SAR':
			return 'red';
		case 'Fishing':
			return 'yellow';
		case 'Sailing':
			return 'purple';
		case 'Towing':
			return 'orange';
		case 'Other':
			return 'brown';
		default: 
			return 'blue';
    }
}

function showResponseRaw(response) {

    var ships = response.data;

    var tableData = [];
    for (var i = 0; i < ships.length; i++) {

        var ship = ships[i];

        for (var j = 0; j < ship.positions.length; j++) {

            var position = ship.positions[j];

            //L.marker([position.longitude, position.latitude]).addTo(_map.map).bindTooltip(ship.shipName);

			tableData.push([ship.shipID, ship.shipName, ship.shipType, position.latitude, position.longitude, position.currentTime]);
        }
	}

    _table.refresh(tableData);
}

function showResponse(response) {

	var ships = response.data;

	for (var i = 0; i < ships.length; i++) {

		var ship = ships[i];

		var geojsonFeature = {
			"type": "Feature",
			"properties": {
				"name": ship.shipName,
				"shipType": ship.shipType,
				"popupContent": ship.shipName + ' (' + ship.shipType + ')'
			},
			"geometry": JSON.parse(ship.geometry)
		};

		L.geoJSON(geojsonFeature,
            {
				color: getColor(ship.shipType),
				dashArray: '5, 5',
                dashOffset: '0'
			})
			.addTo(_map.map)
			.bindTooltip(geojsonFeature.properties.popupContent);
    }
}

function showView(viewSelector) {
	$('.type-view').hide();
	$(viewSelector).show();
}

var _map = new map('map', [55.68, 12.6], 6);
var _table = new table('#data-table');

$(document).ready(function () {

	$('#StartDate').datetimepicker();
	$('#EndDate').datetimepicker();

	showShipTrajectories();
    showShipTrajectoriesRaw();
});

