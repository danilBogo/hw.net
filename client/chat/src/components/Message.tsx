import React, {useEffect, useRef, useState} from 'react';
import {format} from 'date-fns';
import $api from "../http";
import {MessageDto} from "../dto/MessageDto";
import {HubConnection, HubConnectionBuilder} from "@microsoft/signalr";

function Message() {
    const [data, setData] = useState<MessageDto[]>([]);
    const [msg, setMsg] = useState("");
    const [hubConnection, setConnection] = useState<HubConnection>();
    const [isDataLoaded, setIsDataLoaded] = useState(false);
    const lastMessage = useRef<MessageDto>();

    useEffect(() => {
        if (lastMessage.current) {
            if (!data)
                setData([lastMessage.current])
            else
                setData([...data, lastMessage.current])
        }
    }, [lastMessage.current]);

    useEffect(() => {
        const connection = new HubConnectionBuilder()
            .withUrl(`${process.env.REACT_APP_SERVER_URL}/chatSignalR`)
            .build();
        try {
            connection.start().then(() => {
                console.log("я запустился");
                connection.on("Send", message => {
                        const newMsg: MessageDto = {
                            id: "",
                            content: message,
                            time: format(new Date(Date.now()).setHours(new Date(Date.now()).getHours() - 3), 'yyyy/MM/dd kk:mm:ss')
                        };
                        console.log("зашел");
                        // if (!data){
                        //     setData([newMsg]);
                        // }
                        // else
                        //     setData([...data, newMsg]);
                        lastMessage.current = newMsg;
                    }
                );
            })
        } catch (e) {
            console.log("идешь нахуй");
            console.log(e);
        }
        setConnection(connection);
        setInterval(() => {
            setIsDataLoaded(!isDataLoaded);
        }, 100);
        getHistory().then(value => setData(value!));
    }, [])

    function sendMessage() {
        hubConnection!.invoke("Send", msg).then(() => {
            setMsg("");
        });
    }

    function getHistory() {
        return $api.get<MessageDto[]>("/chat").then((res) => {
            if (res.status === 200) {
                console.log("успешно получил историю");
                if (res.data.length > 0)
                    return res.data;
            } else
                console.error('ашипка палучения истори')
        });
    }

    return (
        <div>
            {
                data?.map((value) =>
                    <div id={value.id}>
                        <p>Message: {value.content}</p>
                        <p>Time: {format(new Date(value.time).setHours(new Date(value.time).getHours() + 3), 'yyyy/MM/dd kk:mm:ss')}</p>
                        <br/>
                        <br/>
                    </div>
                )
            }
            <div hidden={true}>{isDataLoaded}</div>
            <textarea value={msg} onChange={(e) => setMsg(e.target.value)}/>
            <br/>
            <button onClick={sendMessage}>Отправить</button>
        </div>
    );
}

export default Message;
