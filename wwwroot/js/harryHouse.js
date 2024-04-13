let lbl_houseJoined = document.getElementById("lbl_houseJoined");

let btn_un_gryffindor = document.getElementById("btn_un_gryffindor");
let btn_un_slytherin = document.getElementById("btn_un_slytherin");
let btn_un_hufflepuff = document.getElementById("btn_un_hufflepuff");
let btn_un_ravenclaw = document.getElementById("btn_un_ravenclaw");
let btn_gryffindor = document.getElementById("btn_gryffindor");
let btn_slytherin = document.getElementById("btn_slytherin");
let btn_hufflepuff = document.getElementById("btn_hufflepuff");
let btn_ravenclaw = document.getElementById("btn_ravenclaw");

let trigger_gryffindor = document.getElementById("trigger_gryffindor");
let trigger_slytherin = document.getElementById("trigger_slytherin");
let trigger_hufflepuff = document.getElementById("trigger_hufflepuff");
let trigger_ravenclaw = document.getElementById("trigger_ravenclaw");

trigger_gryffindor.addEventListener("click", function (event) {
    harryHouseConnection.send("TriggerNotification", "gryffindor");
    event.preventDefault();
})

trigger_slytherin.addEventListener("click", function (event) {
    harryHouseConnection.send("TriggerNotification", "slytherin");
    event.preventDefault();
})

trigger_hufflepuff.addEventListener("click", function (event) {
    harryHouseConnection.send("TriggerNotification", "hufflepuff");
    event.preventDefault();
})

trigger_ravenclaw.addEventListener("click", function (event) {
    harryHouseConnection.send("TriggerNotification", "ravenclaw");
    event.preventDefault();
})

// Create connection with hub
var harryHouseConnection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/harryHouses", signalR.HttpTransportType.LongPolling)          //
    .configureLogging(signalR.LogLevel.Trace)
    .build();

//connect to methods that hub invokes AKA receve notificaiton from hub
harryHouseConnection.on("subscriptionStatus", (strGroupsJoined, houseName, joinOperation) => {
    var joinedHouseListSpan = document.getElementById("lbl_houseJoined");
    joinedHouseListSpan.innerText = strGroupsJoined.toString();

    if (joinOperation) {
        // Display an info toast with no title
        toastr.info(`Subscribed to ${houseName}`)
    }
    else {
        toastr.info(`Unsubscribed from ${houseName}`)
    }
});

harryHouseConnection.on("otherSubscriptionNotification", (houseName, joinOperation) => {
    if (joinOperation) {
        // Display an info toast with no title
        toastr.success(`Someone subscribed to ${houseName}`)
    }
    else {
        toastr.warning(`Someone unsubscribed from ${houseName}`)
    }
});

harryHouseConnection.on("triggerNotification", (houseName) => {
    toastr.success(`New notification sent from ${houseName}`)
});


//invoke hub methods (hub method name) aka send notification to hub (no response back from server)
function joinHouseOnClient(houseName) {
    harryHouseConnection.send("JoinHouse", houseName);

    var unsubBtn = document.getElementById("btn_un_" + houseName);
    unsubBtn.style.display = "";

    var subBtn = document.getElementById("btn_" + houseName);
    subBtn.style.display = "none";
}

function leaveHouseOnClient(houseName) {
    harryHouseConnection.send("LeaveHouse", houseName);

    var unsubBtn = document.getElementById("btn_un_" + houseName);
    unsubBtn.style.display = "none";

    var subBtn = document.getElementById("btn_" + houseName);
    subBtn.style.display = "";
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
harryHouseConnection.start().then(fulfilled, rejected)
