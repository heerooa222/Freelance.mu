$(document).ready(function () {
    const userId = document.getElementById("connected-user");
    if (userId) {
        let connection = new signalR.HubConnectionBuilder().withUrl(`/NotificationHub?userId=${userId.value}`).build();
        connection.on("ReloadMessage", (receiverId, senderId) => {
            if(userId.value === receiverId) {
                loadUserMessage(senderId);
            }
        });
        connection.start().catch(function (err) {
            return console.log(err.toString());
        }).then(function () {
            connection.invoke('GetConnectionId').then(function (connectionId) {
                
            })
        });
    }

    const sendButton = $('#send-button');
    if(sendButton) {
        sendButton.on('click', sendMessage);
    }
    const commentBox = $('#comment-message');
    if(commentBox) {
        commentBox.on('keyup', function (event) {
            if(event.key === 'Enter') {
                sendMessage();
            }
        });
    }

    const commentSection = document.getElementById('message-section');
    if(commentSection) {
        commentSection.scrollTop = commentSection.scrollHeight - commentSection.clientHeight;
    }
});

function sendMessage() {
    const messageInput = document.getElementById('comment-message');
    const receiverId = document.getElementById('receiver-id');
    if(messageInput && messageInput.value !== '' && receiverId) {
        $.post("/message/SendMessage", {id: receiverId.value, message: messageInput.value}, function (response) {
            messageInput.value = '';
            const commentSection = document.getElementById('message-section');
            if(commentSection) {
                commentSection.innerHTML  = response;
                commentSection.scrollTop = commentSection.scrollHeight - commentSection.clientHeight;
            }
        });
    }
}

function loadUserMessage(senderId) {
    $.get(`/message/LoadMessage?id=${senderId}`, function(response) {
        const commentSection = document.getElementById('message-section');
        if(commentSection) {
            commentSection.innerHTML  = response;
            commentSection.scrollTop = commentSection.scrollHeight - commentSection.clientHeight;
        }
    });
}