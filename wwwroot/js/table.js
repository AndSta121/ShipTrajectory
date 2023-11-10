
var table = function (tableId) {

    var columns = [
        { title: "MMSI" },
        { title: "Name" },
        { title: "Type" },
        { title: "Latitude" },
        { title: "Longitude" },
        { title: "Timestamp" }
    ];

    function refreshData(data) {

        $(tableId).DataTable({
            data: data,
            columns: columns
        });
    }

    return {
        refresh: refreshData
    }
};