import React, {FC, SyntheticEvent} from "react";
import {FileInput} from "./FileInput";
import {Preview} from "./Preview";
import {UploadProps} from "./UploadProps";
import {UploadHooks} from "./UploadHooks";

interface Props {
    uploadProps: UploadProps,
    uploadHooks: UploadHooks,
    onLoadPreview?: (e: SyntheticEvent<HTMLImageElement>) => void;
}

export const UploadFiles: FC<Props> = ({uploadProps, uploadHooks, onLoadPreview}) => {

    return (
        <>
            {
                uploadHooks.preview ? <Preview onLoadPreview={onLoadPreview} uploadProps={uploadProps} 
                                               setPreview={uploadHooks.setPreview}
                                   preview={uploadHooks.preview} setFile={uploadHooks.setFile}/> :
                    <FileInput uploadProps={uploadProps} setPreview={uploadHooks.setPreview} setFile={uploadHooks.setFile}/>
            }
        </>
    );
}