import React, {useEffect, useRef, useState} from 'react';
import {format} from 'date-fns';
import $api from "../http";
import {MessageMetadataDto} from "../dto/MessageMetadataDto";
import {HubConnection, HubConnectionBuilder} from "@microsoft/signalr";
import {MessageDto} from "../dto/MessageDto";
import {OneMessageComponent} from "./OneMessageComponent";
import {MetadataDto} from "../dto/MetadataDto";
import {Guid} from "guid-typescript";
import {SaveMetadataDto} from "../dto/SaveMetadataDto";

function Message() {
    const [data, setData] = useState<MessageMetadataDto[]>([]);
    const [msg, setMsg] = useState("");
    const metadata = useRef<MetadataDto>();
    const [hubConnection, setConnection] = useState<HubConnection>();
    const [fileUploadedSignalRConnection, setFileUploadedSignalRConnection] = useState<HubConnection>();
    const [metadataValue, setMetadataValue] = useState('');

    useEffect(() => {
        const connection = new HubConnectionBuilder()
            .withUrl(`${process.env.REACT_APP_SERVER_URL}/chatSignalR`)
            .build();
        connection.on("Send", (message: string, metadata: MetadataDto) => {
                const newMsg: MessageMetadataDto = {
                    content: message,
                    time: format(new Date(Date.now()).setHours(new Date(Date.now()).getHours() - 3), 'yyyy/MM/dd kk:mm:ss'),
                    metadata: metadata
                };
                setData((prev) => !prev ? [newMsg] : [...prev, newMsg]);
            }
        );
        
        const fileUploadedSignalRConnection = new HubConnectionBuilder()
            .withUrl(`${process.env.REACT_APP_CONSUMER_URL}/fileUploadedSignalR`)
            .build();
        fileUploadedSignalRConnection.on("ReceiveMessage", (message: string, fileId: string) => {
            alert(message);
            metadata.current.fileId = fileId;
        });
        
        connection.start().then();
        fileUploadedSignalRConnection.start().then();
        
        setConnection(connection);
        setFileUploadedSignalRConnection(fileUploadedSignalRConnection);
        getMessagesHistory().then(value => {
            if (value && value.length > 0)
                value!.forEach(item => {
                    let messageMetadata: MessageMetadataDto = {
                        content: item.content,
                        time: item.time,
                        metadata: null
                    };
                    console.log(item);
                    if (item?.fileId?.length > 0) {
                        getMetadataByFileId(item.fileId).then(res => {
                            messageMetadata.metadata = res!
                            setData((prev) => !prev ? [messageMetadata] : [...prev, messageMetadata])
                        });
                    } else
                        setData((prev) => !prev ? [messageMetadata] : [...prev, messageMetadata])
                });
        });
    }, [])

    const sendMessage = async () => {
        console.log(metadata.current);
        if(metadata.current != undefined && metadata.current?.name != '' && (metadata.current?.fileId === null || metadata?.current.fileId.length === 0))
        //if (metadata.current != undefined || metadata.current?.name != "" && (metadata.current?.fileId === null || metadata?.current.fileId.length === 0))
        {
            alert("Нельзя отправить сообщение пока файл не загрузился");
            return;
        }
        console.log(metadata);
        const currentMetadata: MetadataDto = {
            id: "",
            name: "",
            contentType: "",
            value: "",
            fileId: ""
        };
        hubConnection!.invoke("Send", msg, metadata.current == undefined ? currentMetadata : metadata.current).then(() => {
            setMsg("");
            setMetadataValue("");
            metadata.current = currentMetadata;
        });
    }

    function getMessagesHistory() {
        return $api.get<MessageDto[]>("/chat").then((res) => {
            if (res.status === 200) {
                if (res.data.length > 0)
                    return res.data;
            } else
                console.error('can not get messages history')
        });
    }

    function getMetadataByFileId(fileId: string) {
        return $api.get<MetadataDto>(`/metadata`, {params: {fileId: fileId}})
            .then((res) => {
                if (res.status === 200) {
                    return res.data;
                } else
                    console.error('can not get metadata by fileId')
            });
    }

    const handleChange = (event: any) => {
        const currentFile = event.target.files[0];
        let answer = window.confirm(`Вы уверены, что хотите загрузить файл ${currentFile?.name}?`);
        if (!answer)
            return;
        const requestId = Guid.create().toString().toUpperCase();

        console.log(metadata);

        metadata.current.name = event.target.files[0].name;
        metadata.current.contentType = event.target.files[0].type;
        
        
        const saveMetadata: SaveMetadataDto = {
            requestId: requestId,
            metadata: metadata.current,
            userId: fileUploadedSignalRConnection.connectionId
        };
        $api.post("/metadata", saveMetadata).then(() => console.log("метаданные загрузились"));
        
        const fileFormData = new FormData();
        fileFormData.set('File', currentFile!);
        fileFormData.append('RequestId', requestId);
        fileFormData.append('UserId', fileUploadedSignalRConnection.connectionId);
        $api.post("/file", fileFormData).then(() => console.log("файл загрузился"));
        
    };
    
    return (
        <div>
            {
                data?.map((value, index) =>
                    <div key={index}>
                        <OneMessageComponent
                            content={value.content}
                            metadata={value.metadata}
                            time={value.time}/>
                    </div>
                )
            }
            <p>input message:</p>
            <textarea value={msg} onChange={(e) => setMsg(e.target.value)}/>
            <br/>
            <p>input metadata:</p>
            <textarea value={metadataValue} onChange={(e) => {
                metadata.current = {
                    id: "",
                    name: "",
                    contentType: "",
                    value: e.target.value,
                    fileId: ""
                }
                setMetadataValue(e.target.value)
            }}/>
            <br/>
            <button onClick={sendMessage}>Отправить</button>
            <input type="file" onChange={handleChange}/>
        </div>
    );
}

export default Message;
