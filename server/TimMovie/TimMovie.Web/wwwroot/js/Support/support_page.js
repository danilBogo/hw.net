(async function () {
    const chatBody = $("#chatBody");
    const messageInput = $("#messageInput");
    const startWorkBtn = $("#startWorkBtn");
    const stopWorkBtn = $("#stopWorkBtn");
    const disconnectCurrentChatBtn = $("#disconnectBtn");
    const userPhoto = $("#userPhotoInChat");
    const userName = $("#userNameInChat");
    const messageOnConnectToNewUser = $("#messageOnConnectToNewUser");
    const sendMessageBtn = $("#sendMessageBtn")
    let chatIsPrepare = false;
    let userInfoIsPrepare = false;
    
    
    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .build();
    
    hubConnection.on("PrepareChat", function (groupName) {
        $.get({
            url: `/TechnicalSupport/GetAllGroupMessagesForSupportPage?groupName=${groupName}`,
            success: function (messagesHtml) {
                if (!chatIsPrepare){
                    chatIsPrepare = true;
                    onConnectToNewUser(messagesHtml);
                }
            },
        });
    });

    hubConnection.on("PrepareUserInfoInChat", function (userInfo) {
        if (!userInfoIsPrepare){
            userInfoIsPrepare = true;
            userName.text(userInfo?.displayName ?? "Незарегистрированный пользователеь");
            if (userInfo?.pathToPhoto == null){
                userPhoto.hide();
            }else {
                userPhoto.attr("src", userInfo?.pathToPhoto);
                userPhoto.show();
            }
        }
    });

    hubConnection.on("ShowNotificationInSupportPage", function (content) {
        $.get({
            url: "/TechnicalSupport/NotificationForSupportPage",
            data: {content},
            success: function (messageHtml) {
                chatBody.append(messageHtml);
                chatBody.scrollTop(chatBody.prop('scrollHeight'));
            },
        });
    });

    hubConnection.on("OnDisconnectSupport",  function () {
        onWaitingNewUser();
        chatIsPrepare = false;
        userInfoIsPrepare = false;
    });

    hubConnection.on("ReceiveNewMessage", function (message) {
        $.post({
            url: "/TechnicalSupport/MessageForSupportPage",
            data: message,
            success: function (messageHtml) {
                chatBody.append(messageHtml);
                chatBody.scrollTop(chatBody.prop('scrollHeight'));
            },
        });
    });
    
    hubConnection.on("OnStartWork", async function (){
        if (chatIsPrepare){
            return;
        }
        
        onWaitingNewUser();
        startWorkBtn.attr("disabled", true);
    });

    hubConnection.on("OnStopWork", function (){
        stopWorkBtn.attr("disabled", true);
        startWorkBtn.attr("disabled", false);
        chatBody.empty();
    });
    
    function hubIsConnect(){
        return hubConnection.state === "Connected";
    }
    
    async function sendMessage() {
        if(!hubIsConnect() || !chatIsPrepare){
            return;
        }
        
        let textMessage = messageInput.val();
        if (textMessage == null || textMessage.length === 0 || !/\S/.test(textMessage)){
            return;
        }
        messageInput.val("");
        await hubConnection.invoke("SendMessageToUser", textMessage);
    }
    
    async function disconnect(){
        await hubConnection.invoke("DisconnectFromChatWithUser");
    }
    
    function onConnectToNewUser(htmlWithMessages){
        updateChat(htmlWithMessages);
        disconnectCurrentChatBtn.attr("disabled", false);
        stopWorkBtn.attr("disabled", true);
        messageOnConnectToNewUser.toast("show");
    }

    function updateChat(htmlWithMessages) {
        chatBody.empty();
        chatBody.append(htmlWithMessages);
        chatBody.scrollTop(chatBody.prop('scrollHeight'));
    }
    
    async function startWork(){
        if (!hubIsConnect()){
            return;
        }
        
        startWorkBtn.attr("disabled", true);
        await hubConnection.invoke("StartWorkForSupport");
    }
    
    async function stopWork(){
        stopWorkBtn.attr("disabled", true);
        await hubConnection.invoke("StopWorkForSupport");
    }
    
    function onWaitingNewUser(){
        chatBody.empty();
        chatBody.append(`
            <div class="h-100 w-100 d-flex justify-content-center align-items-center">
                Ожидайте нового пользователя.
            </div>`);
        disconnectCurrentChatBtn.attr("disabled", true);
        userPhoto.hide();
        userName.text("Имя пользователя");
        stopWorkBtn.attr("disabled", false);
    }
    
    async function onKeypress(e) {
        if ((e.key === "Enter" || e.keyCode === 13) && !e.shiftKey && messageInput.is(":focus")) {
            await sendMessage();
        }
    }
    
    sendMessageBtn.on("click", sendMessage);
    disconnectCurrentChatBtn.on("click", disconnect);
    startWorkBtn.on("click", startWork);
    stopWorkBtn.on("click", stopWork);
    $(document).on("keypress", onKeypress);
    $('textarea').keypress(function(event) {
        if (event.keyCode === 13 && !event.shiftKey) {
            event.preventDefault();
        }
    });

    await hubConnection.start();
})()