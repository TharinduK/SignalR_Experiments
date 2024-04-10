// Create connection with hub
var raceVoteCount = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/RaceVotes", signalR.HttpTransportType.WebSockets) 
    .configureLogging(signalR.LogLevel.Trace)
.build();

//connect to methods that hub invokes AKA receve notificaiton from hub
raceVoteCount.on("updateRaceVote", (cloak, stone, wand) => {
    var cloakSpan = document.getElementById("cloakCounter");
    var stoneSpan = document.getElementById("stoneCounter");
    var wandSpan = document.getElementById("wandCounter");

    cloakSpan.innerText = cloak.toString();
    stoneSpan.innerText = stone.toString();
    wandSpan.innerText = wand.toString();
});

//start connection 
function fulfilled() {
    //do somethign on start 
    console.log("Connection to user hub sucessful");
}

function rejected() {
    //hub rejected log
    console.log("User hub connection rejected");
}
raceVoteCount.start().then(fulfilled, rejected)
