// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.



const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

connection.on("ReceiveNotification", function (message) {
    // Display notification message
    alert(message);
});

connection.start().catch(function (err) {
    console.error(err.toString());
});
