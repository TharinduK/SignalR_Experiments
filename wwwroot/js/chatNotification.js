let sendButton = document.getElementById("sendButton");

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/chat", signalR.HttpTransportType.WebSockets)
    //.configureLogging(signalR.LogLevel.Trace)
    .build();

sendButton.disabled = true;
sendButton.addEventListener("click", SendMessage);

//notificationInput
function SendMessage() {
    var newMessageTb = document.getElementById("notificationInput");
    var message = newMessageTb.value;
    connection.send("recordChatEntry", message).then(function () {
        var newMessageTb = document.getElementById("notificationInput").value = "";
    });
}

connection.on("populateChat", (messageList, messageCount) => populateChat(messageList, messageCount));
function populateChat(messageList, messageCount) {
    
    var counter = document.getElementById("notificationCounter");
    counter.innerHTML = messageCount;

    let messageListUl = document.getElementById("messageList");
    messageListUl.innerHTML = "";
    for (let i = messageList.length; i > - 0; i--) {
        var li = document.createElement("li");
        li.textContent = "Notification - " + messageList[i-1];
        messageListUl.appendChild(li);
    }
    
}
function fulfilled() {
    console.log("Chat hub notification connection sucessful");

    sendButton.disabled = false;

}

function rejected() {
    // to do
    console.log("Chat hub notification connection failed");
}

connection.start().then(fulfilled, rejected);