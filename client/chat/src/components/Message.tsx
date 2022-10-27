import React, {useEffect, useState} from 'react';
import {format} from 'date-fns';
import $api from "../http";
import {MessageFileMetadataDto} from "../dto/MessageFileMetadataDto";
import {HubConnection, HubConnectionBuilder} from "@microsoft/signalr";
import {FileMetadata} from "../dto/FileMetadataDto";
import {MessageDto} from "../dto/MessageDto";

function Message() {
    const [data, setData] = useState<MessageFileMetadataDto[]>([]);
    const [msg, setMsg] = useState("");
    const [hubConnection, setConnection] = useState<HubConnection>();

    const [selectedFile, setSelectedFile] = useState<File>();
    
    useEffect(() => {
        console.log(data, msg,  hubConnection, selectedFile);
    }, [data.length, msg, selectedFile])

    useEffect(() => {
        const connection = new HubConnectionBuilder()
            .withUrl(`${process.env.REACT_APP_SERVER_URL}/chatSignalR`)
            .build();
        try {
            connection.start().then(() => {
                console.log("я запустился");
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
            console.log("чел ты что то неправильно сделал...");
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
                    if (item?.fileId?.length > 0)
                        getFileForMessage(item.fileId).then(res => {
                            messageFileMetadata.fileMetadata = res!
                        });
                    setData((prev) => !prev ? [messageFileMetadata] : [...prev, messageFileMetadata])
                });
        });
    }, [])

    const sendMessage = async () => {
        const formData = new FormData();
        let fileMetadata : FileMetadata = {
            id: "",
            extension: "",
            contentType: "",
            size: 0,
            name: ""
        };
        if (selectedFile) {
            formData.set('file', selectedFile!);
            $api.post("/file", formData).then((res) => {
                fileMetadata = {
                    id: res.data.id, 
                    extension: res.data.extension,
                    contentType: res.data.contentType,
                    size: res.data.size,
                    name: res.data.name
                }
                console.log(fileMetadata);
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
                console.log("успешно получил историю");
                console.log(res.data);
                console.log("конец истории");
                if (res.data.length > 0)
                    return res.data;
            } else
                console.error('ашипка палучения истори')
        });
    }

    function getFileForMessage(fileId: string) {
        return $api.get<FileMetadata>(`/file?fileId=${fileId}`).then((res) => {
            if (res.status === 200) {
                return res.data;
            } else
                console.error('ашипка палучения файла')
        });
    }

    const handleChange = (event: any) => {
        console.log(event.target.files[0]);
        setSelectedFile(event.target.files[0]);
    };

    return (
        <div>
            {
                data?.map((value, index) =>
                    <div key={index}>
                        <p>Message: {value.content}</p>
                        <p>Time: {format(new Date(value.time).setHours(new Date(value.time).getHours() + 3), 'yyyy/MM/dd kk:mm:ss')}</p>
                        {value.fileMetadata ? (<>
                            <button id={value.fileMetadata.id}
                                    onClick={() => alert(value.fileMetadata?.id)}>File: {value.fileMetadata.name}</button>
                            <p>Size: {value.fileMetadata.size}</p>
                            <p>ContentType: {value.fileMetadata.contentType}</p>
                            <p>Extension: {value.fileMetadata.extension}</p>
                        </>) : null}
                        <br/>
                        <br/>
                    </div>
                )
            }
            <textarea value={msg} onChange={(e) => setMsg(e.target.value)}/>
            <br/>
            {/*<button onClick={sendMessage}>Отправить</button>*/}
            {/*<input type="file" onChange={handleChange} accept={".png,.txt"}/>*/}
        </div>
    );
}

export default Message;
