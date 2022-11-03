import {MessageFileMetadataDto} from "../dto/MessageFileMetadataDto";
import React from "react";
import {format} from "date-fns";

export const OneMessageComponent = (props: MessageFileMetadataDto) => {
    const {
        content,
        time,
        fileMetadata
    } = props

    return (
        <div>
            <p>Message: {content}</p>
            <p>Time: {format(new Date(time).setHours(new Date(time).getHours() + 3), 'yyyy/MM/dd kk:mm:ss')}</p>
            {fileMetadata && fileMetadata.id.length > 0 ? (<>
                <a href={`${process.env.REACT_APP_SERVER_URL}/file?fileId=${fileMetadata.fileId}&contentType=${fileMetadata.contentType}&name=${fileMetadata.name}`}
                   download>Скачать
                    файл</a>
                <p>Size: {fileMetadata.size}</p>
                <p>ContentType: {fileMetadata.contentType}</p>
                <p>Extension: {fileMetadata.extension}</p>
            </>) : null}
            <br/>
            <br/>
        </div>
    )
} 