"use strict";

document.getElementById("sendButton").addEventListener("click", function (event) {
    const user = document.getElementById("userInput").value;
    const message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

$('#btn-broadcast').click(function () {
    const message = $('#broadcast').val();
    if (message.includes(';')) {
        const messages = message.split(';');
        const subject = new signalR.Subject();
        connection.send("BroadcastStream", subject).catch(err => console.error(err.toString()));
        for (let i = 0; i < messages.length; i++) {
            subject.next(messages[i]);
        }

        subject.complete();

    } else {
        connection.invoke("BroadcastMessage", message).catch(err => console.error(err.toString()));
    }
});