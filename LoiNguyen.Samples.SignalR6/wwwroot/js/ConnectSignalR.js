"use strict";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/messagehub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

async function start() {
    try {
        await connection.start();
        console.log('connected');

    } catch (err) {
        console.log(err);
        setTimeout(() => start(), 5000);
    }
};

connection.onclose(async () => {
    await start();
});

start();

connection.on("ReceiveMessageHandler", function (message) {
    $('#signalr-message-panel').prepend($('<div />').text(message));
});