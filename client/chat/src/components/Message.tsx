import React, {useEffect, useState} from 'react';
import {format} from 'date-fns';
import $api from "../http";
import {MessageFileMetadataDto} from "../dto/MessageFileMetadataDto";
import {HubConnection, HubConnectionBuilder} from "@microsoft/signalr";
import {FileMetadata} from "../dto/FileMetadataDto";
import {MessageDto} from "../dto/MessageDto";
import {OneMessageComponent} from "./OneMessageComponent";

function Message() {
    const [data, setData] = useState<MessageFileMetadataDto[]>([]);
    const [msg, setMsg] = useState("");
    const [hubConnection, setConnection] = useState<HubConnection>();
    const [selectedFile, setSelectedFile] = useState<File>();

    useEffect(() => {
        const connection = new HubConnectionBuilder()
            .withUrl(`${process.env.REACT_APP_SERVER_URL}/chatSignalR`)
            .build();
        try {
            connection.start().then(() => {
                connection.on("Send", (message: string, fileMetadata: FileMetadata) => {
                        const newMsg: MessageFileMetadataDto = {
                            content: message,
                            time: format(new Date(Date.now()).setHours(new Date(Date.now()).getHours() - 3), 'yyyy/MM/dd kk:mm:ss'),
                            fileMetadata: fileMetadata
                        };
                        setData((prev) => !prev ? [newMsg] : [...prev, newMsg]);
                    }
                );
            })
        } catch (e) {
            console.log(e);
        }
        setConnection(connection);
        getMessagesHistory().then(value => {
            if (value && value.length > 0)
                value!.forEach(item => {
                    let messageFileMetadata: MessageFileMetadataDto = {
                        content: item.content,
                        time: item.time,
                        fileMetadata: null
                    };
                    if (item?.fileId?.length > 0) {
                        getFileForMessage(item.fileId).then(res => {
                            messageFileMetadata.fileMetadata = res!
                            setData((prev) => !prev ? [messageFileMetadata] : [...prev, messageFileMetadata])
                        });
                    } else
                        setData((prev) => !prev ? [messageFileMetadata] : [...prev, messageFileMetadata])
                });
        });
    }, [])

    const sendMessage = async () => {
        const formData = new FormData();
        let fileMetadata: FileMetadata = {
            id: "",
            extension: "",
            contentType: "",
            size: 0,
            name: "",
            fileId: ""
        };
        if (selectedFile) {
            formData.set('file', selectedFile!);
            $api.post("/filemetadata", formData).then((res) => {
                fileMetadata = {
                    id: res.data.id,
                    extension: res.data.extension,
                    contentType: res.data.contentType,
                    size: res.data.size,
                    name: res.data.name,
                    fileId: res.data.fileId
                }
                hubConnection!.invoke("Send", msg, fileMetadata).then(() => {
                    setMsg("");
                });
                setSelectedFile(undefined);
            });
        } else {
            hubConnection!.invoke("Send", msg, fileMetadata).then(() => {
                setMsg("");
            });
        }
    }

    function getMessagesHistory() {
        return $api.get<MessageDto[]>("/chat").then((res) => {
            if (res.status === 200) {
                if (res.data.length > 0)
                    return res.data;
            } else
                console.error('ашипка палучения истори')
        });
    }

    function getFileForMessage(fileId: string) {
        return $api.get<FileMetadata>(`/filemetadata`, {params: {fileId: fileId}})
            .then((res) => {
                if (res.status === 200) {
                    return res.data;
                } else
                    console.error('ашипка палучения файла')
            });
    }

    const handleChange = (event: any) => {
        setSelectedFile(event.target.files[0]);
    };


    return (
        <div>
            {
                data?.map((value, index) =>
                        <div key={index}>
                            <OneMessageComponent
                                content={value.content}
                                fileMetadata={value.fileMetadata}
                                time={value.time}/>
                        </div>

                    /*<div key={index}>
                        <p>Message: {value.content}</p>
                        <p>Time: {format(new Date(value.time).setHours(new Date(value.time).getHours() + 3), 'yyyy/MM/dd kk:mm:ss')}</p>
                        {value.fileMetadata && value.fileMetadata.id.length > 0 ? (<>
                            <a href={getUrl(value.fileMetadata.id, value.fileMetadata.contentType)} download>Скачать
                                файл</a>
                            <p>Size: {value.fileMetadata.size}</p>
                            <p>ContentType: {value.fileMetadata.contentType}</p>
                            <p>Extension: {value.fileMetadata.extension}</p>
                        </>) : null}
                        <br/>
                        <br/>
                    </div>*/
                )
            }
            <textarea value={msg} onChange={(e) => setMsg(e.target.value)}/>
            <br/>
            <button onClick={sendMessage}>Отправить</button>
            <input type="file" onChange={handleChange} accept={".png,.txt"}/>
        </div>
    );
}

export default Message;
