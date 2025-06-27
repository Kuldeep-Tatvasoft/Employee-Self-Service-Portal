$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/notificationHub")
        .build();

    // Handle incoming notifications
    connection.on("ReceiveNotification", (message) => {
        // const notificationArea = document.getElementById("notificationArea");
        // const notificationMessage = document.getElementById("notificationMessage");
        // notificationMessage.textContent = message;
        // alert(message);
        toastr.success(message);
        console.log(message);
        // notificationArea.style.display = "block";


        // setTimeout(() => { notificationArea.style.display = "none"; }, 5000);
       

    });
    connection.start().catch(err => console.error(err.toString()));
});