import React, {useEffect, useState} from "react";
import {UploadFiles} from "../common/upload/UploadFiles";
import $api from "../../http";

interface ActorsProducersRowProps {
    id: string
    type: string,
    name: string,
    surname: string | null,
    photo: string,
    onDelete: (type: string, id: string) => void
    update: Function
    getTypeInRussian: (type: string) => string
}


function ActorsProducersRaw(person: ActorsProducersRowProps) {
    const [isEdit, setIsEdit] = useState<boolean>(false)
    const [originName, setOriginName] = useState(person.name)
    const [originSurname, setOriginSurname] = useState(person.surname)
    const [originPhoto, setOriginPhoto] = useState(person.photo)
    const [name, setName] = useState(originName)
    const [surname, setSurname] = useState(originSurname)
    const [file, setFile] = useState<File | null>()
    const [preview, setPreview] = useState<string | null>(originPhoto)

    function isFieldsValid(): boolean {
        return !!preview && name.length > 0
    }

    function cancelEdit() {
        setFile(null)
        setName(originName)
        setSurname(originSurname)
        setPreview(originPhoto)
    }

    function onClickEdit() {
        if (isEdit) {
            cancelEdit()
        } else {

        }
        setIsEdit(!isEdit)
    }

    function onClickSave() {
        if (!isFieldsValid()) {
            return
        }
        let formdata = new FormData()
        if (file) {
            formdata.append('img', file)
        }
        formdata.append('name', name)
        if (surname) {
            formdata.append('surname', surname)
        }
        formdata.append('type', person.type)
        $api.post(`/person/update/${person.id}`, formdata).then(response => {
            if (response.data.success) {
                alert(`${person.getTypeInRussian(person.type)} успешно обновлен`)
                updatePerson()
                setIsEdit(false)
            } else {
                alert(response.data.textError)
            }
        })
    }

    function updatePerson() {
        setName(name)
        setOriginName(name)
        setOriginSurname(surname)
        setOriginPhoto(preview!)
    }

    const photoSize = 155;
    const borderRadius = 15;

    return (<>
        <tr style={{verticalAlign: "middle"}}>
            <td width={photoSize}>
                <div className="justify-content-center d-flex">
                    {isEdit ? <UploadFiles uploadProps={{photoWidth: photoSize, photoHeight: photoSize, borderRadius: borderRadius}} uploadHooks={{
                        setFile: setFile,
                        setPreview: setPreview,
                        preview: preview
                    }}/> : <img src={originPhoto} style={{height: photoSize, width: photoSize,borderRadius:borderRadius, objectFit: "cover"}} className=""
                                alt=""/>}
                </div>
            </td>
            <td width={330}>
                {isEdit ? <input maxLength={150} type='text' className='form-control' value={name}
                                 onChange={e => setName(e.target.value)}/> : originName}
            </td>
            <td width={330}> {isEdit ? <input type='text' maxLength={150} className='form-control' value={surname ?? ''}
                                              onChange={e => setSurname(e.target.value)}/> : originSurname}</td>
            <td width={10}>{person.getTypeInRussian(person.type)}</td>
            <td width={50}>
                <div className="d-flex flex-column align-items-center">
                    <div title="Редактировать" style={{cursor: "pointer"}} onClick={onClickEdit}>
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16"
                             fill={isEdit ? 'blue' : 'gray'} className="bi bi-pencil" viewBox="0 0 16 16">
                            <path
                                d="M12.146.146a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1 0 .708l-10 10a.5.5 0 0 1-.168.11l-5 2a.5.5 0 0 1-.65-.65l2-5a.5.5 0 0 1 .11-.168l10-10zM11.207 2.5 13.5 4.793 14.793 3.5 12.5 1.207 11.207 2.5zm1.586 3L10.5 3.207 4 9.707V10h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.293l6.5-6.5zm-9.761 5.175-.106.106-1.528 3.821 3.821-1.528.106-.106A.5.5 0 0 1 5 12.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.468-.325z"/>
                        </svg>
                    </div>
                    {
                        isEdit ?
                            <div className="mt-5" title="Сохранить" onClick={onClickSave}
                                 style={{display: isFieldsValid() ? 'block' : 'none'}}>
                                <svg style={{cursor: "pointer"}} xmlns="http://www.w3.org/2000/svg" width="32"
                                     fill="green"
                                     className="bi bi-check" viewBox="0 0 16 16">
                                    <path
                                        d="M10.97 4.97a.75.75 0 0 1 1.07 1.05l-3.99 4.99a.75.75 0 0 1-1.08.02L4.324 8.384a.75.75 0 1 1 1.06-1.06l2.094 2.093 3.473-4.425a.267.267 0 0 1 .02-.022z"/>
                                </svg>
                            </div>
                            : <div onClick={() => person.onDelete(person.type, person.id)} className="mt-5"
                                   title="Удалить">
                                <svg style={{cursor: "pointer"}} xmlns="http://www.w3.org/2000/svg" width="20"
                                     fill="red" className="bi bi-trash" viewBox="0 0 16 16">
                                    <path
                                        d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z"/>
                                    <path fillRule="evenodd"
                                          d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z"/>
                                </svg>
                            </div>
                    }
                </div>
            </td>
        </tr>
    </>)
}

export default ActorsProducersRaw