import {BannerDto} from "../../dto/BannerDto";
import styles from "./banner.module.css"
import $api from "../../http";
import React, {useState} from "react";
import {throws} from "assert";
import Result from "../../dto/Result";
import useModalWindow from "../../hook/modal/useModalWindow";
import {FileInput} from "../common/upload/FileInput";
import {UploadFiles} from "../common/upload/UploadFiles";

interface Props {
    banner: BannerDto,
    onDelete: Function
}


function Banner(props: Props) {

    const [isEdit, setIsEdit] = useState<boolean>(false)
    const [originDescription, setOriginDescription] = useState(props.banner.description)
    const [description, setDescription] = useState<string>('')
    const initialTextAreaHeight = description.length / 82;
    const [textareaHeight, setTextAreaHeight] = useState(initialTextAreaHeight)
    const [file, setFile] = useState<File | null>()
    const [preview, setPreview] = useState<string | null>()
    const [image, setImage] = useState<string>(process.env.REACT_APP_FILESERVER + props.banner.image)

    function onCancelClick() {
        setFile(null)
        setIsEdit(false)
        setPreview(null)
        setDescription(originDescription)
        setTextAreaHeight(description.length / 82 + 1)
    }

    function validateForm(): Result<string> {
        if(file != null && !(file?.name.includes('.jpg') || file?.name.includes('.png'))){
            return {success:false, textError:'неверное расширение фото'}
        }
        if (description?.length <= 0) {
            return {success: false, textError: 'требуется ввести описание'}
        }

        return {success: true}
    }

    function onSaveClick() {
        const validationResult = validateForm()
        if (!validationResult.success) {
            alert(validationResult.textError)
            return
        }
        let data = generateRequestData()
        $api.post('/banners/update', data).then(response => {
            if (response.data.success) {
                setDescription(description)
                setOriginDescription(description)
                setFile(null)
                setIsEdit(false)
                if(preview != null){
                    setImage(preview)
                }
                alert('баннер обновлен')
            } else {
                alert(response.data.textError)
            }
        })
    }

    function onEditClick(currentDescription: string) {
        setIsEdit(true)
        setDescription(currentDescription)
    }

    function generateRequestData() {
        let formData = new FormData()
        if (file != null) {
            formData.append("img", file)
        }
        formData.append("description", description!);
        formData.append("bannerId", props.banner.bannerId);
        return formData
    }

    function handleTextAreaChange(e: any) {
        const value = e.target.value
        setDescription(value)
        const potentialHeight = value.length / 82 + 1
        setTextAreaHeight(potentialHeight > 3 ? 3 : potentialHeight)
    }

    function onLoadFromInputHandler(e: any){
        const files = e.target.files
        if(!(files[0].name.includes('.jpg') || files[0].name.includes('.png'))){
            alert('неверное расширение фото')
            return
        }
        setFile(files[0]);
        setPreview(URL.createObjectURL(files[0]));
    }

    return (<div className="mt-5 d-inline-flex">
        <div className={styles.bannerImage}
             style={{backgroundImage: preview ? `url(${preview})` : `url(${image})`}}>
            <div className={styles.bannerContainer}>
                <h1 className={styles.bannerFilmTitle}>{props.banner.filmTitle}</h1>
                {isEdit ? <textarea rows={textareaHeight} maxLength={250}
                                    className={"bannerFilmDescription form-control-plaintext"}
                                    style={{
                                        resize: 'none',
                                        color: "white",
                                        fontSize: "20px",
                                        position: "relative",
                                        bottom: "8px"
                                    }}
                                    onChange={(e) => handleTextAreaChange(e)}
                                    value={description}/> :
                    <p className={styles.bannerFilmDescription}>{originDescription}</p>}
                {
                    isEdit ?
                        <div>
                            <input onClick={onCancelClick} type="button" className="btn btn-danger btn-lg"
                                   value="Отмена"/>
                            <input onClick={onSaveClick} type="button" className="ms-3 btn btn-success btn-lg"
                                   value="Сохранить"/>
                            <label className="btn btn-lg btn-primary ms-3">
                                <input type="file" multiple accept="image/*" onChange={e => onLoadFromInputHandler(e)} hidden/>
                                Выбрать фото
                            </label>
                            <div>
                            </div>
                        </div> :
                        <input type="button" className="btn btn-danger btn-lg" value="Удалить"
                               onClick={() => props.onDelete(props.banner.bannerId)}/>
                }
            </div>
        </div>
        {!isEdit ? <div title="Редактировать" className="h-25 mt-5 ms-3" style={{cursor: "pointer"}}
                        onClick={() => onEditClick(originDescription)}>
            <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="currentColor"
                 className="bi bi-pencil" viewBox="0 0 16 16">
                <path
                    d="M12.146.146a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1 0 .708l-10 10a.5.5 0 0 1-.168.11l-5 2a.5.5 0 0 1-.65-.65l2-5a.5.5 0 0 1 .11-.168l10-10zM11.207 2.5 13.5 4.793 14.793 3.5 12.5 1.207 11.207 2.5zm1.586 3L10.5 3.207 4 9.707V10h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.293l6.5-6.5zm-9.761 5.175-.106.106-1.528 3.821 3.821-1.528.106-.106A.5.5 0 0 1 5 12.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.468-.325z"/>
            </svg>
        </div> : ''}
    </div>)
}

export default Banner