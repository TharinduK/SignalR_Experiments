
let sendMessageBtn = document.getElementById("sendMessage");
sendMessageBtn.addEventListener("click", function (event) {
    var chatMessage = document.getElementById("chatMessage").value;
    var fromEmail = document.getElementById("senderEmail").value;
    var toEmail = document.getElementById("receiverEmail").value;

    if (toEmail == "") {
        //message send to all suers 
        chatConnection.send("MessageSendToAllUsers", chatMessage, fromEmail).catch(function (err) {
            console.log(err.toString());
        });
    }
    else {
        chatConnection.send("MessageSendToUser", chatMessage, fromEmail, toEmail).catch(function (err) {
            console.log(err.toString());
        });
    }
    event.preventDefault();
    document.getElementById("chatMessage").value = "";
});

sendMessageBtn.disabled = true;

// Create connection with hub
var chatConnection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/chathub", signalR.HttpTransportType.WebSockets)
    .configureLogging(signalR.LogLevel.Trace)
    .build();

//connect to methods that hub invokes AKA receve notificaiton from hub
chatConnection.on("messageReceived", (user, message) => {

    let messageListUl = document.getElementById("messagesList");
    let li = document.createElement("li");
    li.textContent = `${user} : ${message}`;
    messageListUl.appendChild(li);
});

//start connection 
function fulfilled() {
    console.log("Connection to user hub sucessful");
    sendMessageBtn.disabled = false;
}

function rejected() {
    console.log("User hub connection rejected");
}
chatConnection.start().then(fulfilled, rejected)
