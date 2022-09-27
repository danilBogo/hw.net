import React, {useEffect, useState} from 'react';
import './App.css';
import $api from "../http";
import {MessageDto} from "../dto/MessageDto";

function Message() {
    const [data, setData] = useState<MessageDto[]>();
    useEffect(() => {
        getHistory().then(value => setData(value))
    }, [])

    async function getHistory() {
        let res = (await $api.get<MessageDto[]>("/chat"))
        if (res.status === 200)
            return res.data;
        else
            console.error('ашипка')
    }

    return (
        <>
            {
                data?.map((value) => <p>{value.content}</p>)
            }
        </>
    );
}

export default Message;
