$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/notificationHub")
        .build();

    
    connection.on("ReceiveNotification", (message) => {
       
        console.log(message);
        toastr.success(message);
        
        // $(document).ready( function () {             
        //             $.ajax({
        //                 url: '/Event/GetNotifications',
        //                 type: "GET",
        //                 success: function (data) {
        //                     $("#notificationContent").html(data);
        //                 },
        //                 error: function () {
        //                     $("#notificationContent").html('<li>An error has occurred</li>');
        //                 }
        //             });
        // });
    });

    


    connection.start().catch(err => console.error(err.toString()));
});