import React, {EventHandler, FC, SyntheticEvent, useRef, useState} from "react";
import styles from './styles/preview.module.css'
import {UploadProps} from "./UploadProps";

interface Props{
    setPreview: Function,
    setFile: Function
    preview: string,
    uploadProps: UploadProps,
    onLoadPreview?: (e: SyntheticEvent<HTMLImageElement>) => void
}

export const Preview: FC<Props> = ({setPreview, preview,uploadProps, setFile, onLoadPreview}) => {

    function removePreview() {
        setFile(null)
        setPreview(null)
    }

    return(<>
        <div className="d-flex flex-column align-items-center">
            <img src={preview} alt="" onLoad={onLoadPreview} className={styles.preview} width={uploadProps.photoWidth} height={uploadProps.photoHeight} style={{borderRadius: uploadProps.borderRadius, objectFit:"cover"}} />
            <input type="button" className="btn btn-outline-danger w-100 mt-2" value="Отмена" onClick={removePreview} />
        </div>
    </>)
}