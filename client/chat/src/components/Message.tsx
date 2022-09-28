import React, {useEffect, useRef, useState} from 'react';
import $api from "../http";
import {MessageDto} from "../dto/MessageDto";

import {AMQPChannel, AMQPWebSocketClient} from "@cloudamqp/amqp-client";

function Message() {
    const [data, setData] = useState<MessageDto[]>();
    const [msg, setMsg] = useState("");
    const channel = useRef<AMQPChannel>()
        
    useEffect(() => {
        subscribe().then(() => getHistory().then(value => setData(value)))
    }, [])

    async function sendMessage() {
        try {
            await channel.current!.basicPublish("amq.fanout", "", msg, { contentType: "text/plain" })
        } catch (err) {
            console.error("Error", err)
        }
    }
    
    async function subscribe() {
        const tls = window.location.protocol === "https:";
        const url = `${tls ? "wss" : "ws"}://localhost:15670`;
        const vhost = "/";
        const username = "guest";
        const password = "guest";
        const queueName = "MyQueue";
        const exchange = "amq.fanout";
        const routingKey = "";

        const amqp = new AMQPWebSocketClient(url, vhost, username, password);
        const client = await amqp.connect();
        channel.current = await client.channel();
        
        const queue = await channel.current.queue(queueName);
        await queue.bind(exchange, routingKey);
        await queue.subscribe({}, (msg) => {
            const body = msg.bodyToString();
            if (body === null) {
                console.error('Received body is null');
                return;
            }
            try {
                // console.log(body);
                console.log(JSON.parse(body));
            } catch (e) {
                console.error('Could not parse received message', e);
                return;
            }
        });
    }

    async function getHistory() {
        let res = (await $api.get<MessageDto[]>("/chat"))
        if (res.status === 200)
            return res.data;
        else
            console.error('ашипка палучения истори')
    }

    return (
        <div>
            {
                data?.map((value) =>
                    <div id={value.id}>
                        <p>Message id: {value.id}</p>
                        <p>Message: {value.content}</p>
                        <p>Time: {value.time}</p>
                        <br/>
                        <br/>
                    </div>
                )
            }
            <textarea value={msg} onChange={(e) => setMsg(e.target.value)}/>
            <br/>
            <button onClick={sendMessage}>Отправить</button>
        </div>
    );
}

export default Message;
