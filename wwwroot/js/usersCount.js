// Create connection with hub
var raceVoteCount = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/userCount", signalR.HttpTransportType.LongPolling)          
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Trace)
.build();

//connect to methods that hub invokes AKA receve notificaiton from hub
raceVoteCount.on("updateTotalViews", (value) => {
    var newCountSpan = document.getElementById("totalViewsCounter");
    newCountSpan.innerText = value.toString();
});

raceVoteCount.on("updateTotalSession", (value) => {
    var newCountSpan = document.getElementById("totalSessionCounter");
    newCountSpan.innerText = value.toString();
});

raceVoteCount.onclose((error) => {
    document.body.style.background = "red";
});

raceVoteCount.onreconnected((connectionID) => {
    document.body.style.background = "green";
});

raceVoteCount.onreconnecting((error) => {
    document.body.style.background = "orange";
});

//invoke hub methods (hub method name) aka send notification to hub (no response back from server)
function newWindowLoadedOnClient() {
    raceVoteCount.send("NewWindowsLoaded");

    raceVoteCount.invoke("GetTotalViewsOnNewWindowLoad").then((value) => console.log(value));
}

//start connection 
function fulfilled() {
    //do somethign on start 
    console.log("Connection to user hub sucessful");
    newWindowLoadedOnClient();
    console.log("NewWindowsLoaded message invoked");
}

function rejected() {
    //hub rejected log
    console.log("User hub connection rejected");
}
raceVoteCount.start().then(fulfilled, rejected)
