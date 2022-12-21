// (async function () {
//     const chatBody = $("#chatBody");
//     const messageInput = $("#inputMessage");
//     const supportChatButton = $("#supportChatButton");
//
//    
//     const hubConnection = new signalR.HubConnectionBuilder()
//         .withUrl("/chat")
//         .build();
//    
//     hubConnection.on("ReceiveNewMessage", function (message){
//         $.post({
//             url: "/TechnicalSupport/MessageForUserChat",
//             data: message,
//             success: function (messageHtml) {
//                 chatBody.append(messageHtml);
//                 chatBody.scrollTop(chatBody.prop('scrollHeight'));
//             },
//         });
//     });
//
//     hubConnection.on("GetUserInfo", async function () {
//         await hubConnection.invoke("SendUserInfoToSupport");
//     });
//
//     hubConnection.on("TryAddUserToDisconnectedGroups", async function () {
//         await hubConnection.invoke("AddUserGroupToDisconnected");
//     });
//
//     hubConnection.on("ShowNotificationForUserChat", function (content){
//         $.post({
//             url: "/TechnicalSupport/NotificationForUserChat",
//             data: {content},
//             success: function (messageHtml) {
//                 chatBody.append(messageHtml);
//                 chatBody.scrollTop(chatBody.prop('scrollHeight'));
//             },
//         });
//     });
//
//     hubConnection.on("GetInWaitingQueue", async function (){
//         await hubConnection.invoke("PutInQueueCurrentUser");
//     });
//
//     function hubIsConnect(){
//         return hubConnection.state === "Connected";
//     }
//
//     async function sendMessage() {
//         if(!hubIsConnect()){
//             return;
//         }
//        
//         let textMessage = messageInput.val();
//         if (textMessage == null || textMessage.length === 0 || !/\S/.test(textMessage)){
//             return;
//         }
//
//         messageInput.val("");
//         await hubConnection.invoke("SendMessageToSupport", textMessage);
//     }
//    
//     function showAllUserMessage(){
//         $.get({
//             url: "/TechnicalSupport/GetAllUserMessages",
//             success: function (messagesHtml) {
//                 chatBody.append(messagesHtml);
//                 chatBody.scrollTop(chatBody.prop('scrollHeight'));
//             },
//         });
//     }
//    
//     async function onKeypress(e) {
//         if ((e.key === "Enter" || e.keyCode === 13) && !e.shiftKey && messageInput.is(":focus")) {
//             await sendMessage();
//         }
//     }
//    
//     async function openConnection(){
//         await hubConnection.start();
//     }
//
//     $(document).on("keypress", onKeypress);
//     $("#sendMessageButton").on("click", sendMessage);
//     supportChatButton.one("click", showAllUserMessage);
//     supportChatButton.one("click", openConnection);
//     $('textarea').keypress(function(event) {
//         if (event.keyCode === 13 && !event.shiftKey) {
//             event.preventDefault();
//         }
//     });
// })()