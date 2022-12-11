import {MessageMetadataDto} from "../dto/MessageMetadataDto";
import React from "react";
import {format} from "date-fns";

export const OneMessageComponent = (props: MessageMetadataDto) => {
    const {
        content,
        time,
        metadata
    } = props

    return (
        <div>
            <p>Message: {content}</p>
            <p>Time: {format(new Date(time).setHours(new Date(time).getHours() + 3), 'yyyy/MM/dd kk:mm:ss')}</p>
            {metadata && metadata.fileId.length > 0 ? (<>
                <a href={`${process.env.REACT_APP_SERVER_URL}/file?fileId=${metadata.fileId}&contentType=${metadata.contentType}&name=${metadata.name}`}
                   download>Скачать
                    файл</a>
                <p>ContentType: {metadata.contentType}</p>
                <p>Metadata: {metadata.value}</p>
            </>) : null}
            <br/>
            <br/>
        </div>
    )
} 