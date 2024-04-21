var dataTable;
$(document).ready(function () {
    loadDataTable();
});

var connectOrder = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/order", signalR.HttpTransportType.WebSockets)
    .configureLogging(signalR.LogLevel.Trace)
    .build();

connectOrder.on("NewOrderPlaced", () => {
    dataTable.ajax.reload();
    toastr.success("New Order recevied");
})
function success() {
    console.log("Connection to user hub sucessful");
}

function fail() {
    console.log("User hub connection rejected");
}

connectOrder.start().then(success, fail);
function loadDataTable() {

    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Home/GetAllOrder"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "name", "width": "15%" },
            { "data": "itemName", "width": "15%" },
            { "data": "count", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href=""
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> </a>
                      
					</div>
                        `
                },
                "width": "5%"
            }
        ]
    });
}
