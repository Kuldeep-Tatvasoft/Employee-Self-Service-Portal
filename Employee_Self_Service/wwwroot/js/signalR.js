$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/notificationHub")
        .build();

    connection.on("ReceiveNotification", (message) => {
        console.log(message);
        toastr.success(message);
        updateNotificationCount();
    });

    connection.start().catch(err => console.error(err.toString()));
});