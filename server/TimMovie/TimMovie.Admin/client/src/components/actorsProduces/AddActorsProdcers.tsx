import {UploadFiles} from "../common/upload/UploadFiles";
import React, {useState} from "react";
import {useInput} from "../../hook/input/useInput";
import {checkOnEmpty} from "../../templeteForValidationWithError/templetes";
import InputWithValidation from "../common/input/validation/InputWithValidation";
import RequiredFieldIcon from "../common/symbols/RequiredFieldIcon";
import $api from "../../http";

interface AddPageProps{
    update: Function
}

function AddActorsProducers(props: AddPageProps) {
    const [file, setFile] = useState<File>()
    const name = useInput({
        validations: [checkOnEmpty, {
            valueIsValid: value => value.length <= 100,
            errorMessage: `максимальная длина имени: 100 символов`
        }]
    });
    const surname = useInput({
        validations: [
            {
                valueIsValid: value => value.length <= 100,
                errorMessage: `максимальная длина фамилии: 100 символов`
            }
        ]
    });

    const actorValueName: string = 'actor'
    const producerValueName: string = 'producer'

    const [preview, setPreview] = useState<string | null>(null)
    const [type, setType] = useState(actorValueName)

    function generateRequest() {
        let formData = new FormData()
        formData.append('img', file!)
        formData.append('name', name.value)
        formData.append('surname', surname.value)
        return formData
    }

    function fieldsIsValid() {
        return name.validationState.inputIsValid && surname.validationState.inputIsValid && !!preview && !!file
    }

    function onSave() {
        if (!fieldsIsValid()) {
            return
        }
        let url = `${type}s/add`
        let data = generateRequest()
        $api.post(url, data).then(response => {
            if (response.data.success) {
                let nameType = type === "producer" ? "Режиссер" : "Актер";
                alert(`${nameType} успешно добавлен`)
                resetFields()
                props.update()
            } else {
                alert(response.data.textError)
            }
        })
    }

    function resetFields() {
        name.resetInput()
        surname.resetInput()
        setPreview(null)
        setFile(undefined)
    }

    return (<>
        <div className="d-flex flex-row justify-content-center">
            <div className="">
                <UploadFiles uploadProps={{photoWidth: 300, photoHeight: 300, borderRadius: 15}}
                             uploadHooks={{setFile: setFile, setPreview: setPreview, preview: preview}}
                />
            </div>
            <div className="ms-5">
                <InputWithValidation inputInfo={name} label="Имя" isRequired={true}
                                     inputClasses={"mt-1 form-control"} typeInput="text"/>
                <div className="mt-4">
                    <InputWithValidation inputInfo={surname} label="Фамилия" typeInput="text" isRequired={false}
                                         inputClasses={"mt-1 form-control"}/>
                </div>
                <div className="mt-4">
                    <div className="mb-2">Тип<RequiredFieldIcon/>
                        <select name="" id="" className="form-select" onChange={(e) => setType(e.currentTarget.value)}>
                            <option value={`${actorValueName}`}>
                                Актер
                            </option>
                            <option value={`${producerValueName}`}>
                                Режиссер
                            </option>
                        </select>
                    </div>
                </div>
                {fieldsIsValid() ? <input type="button" className="mt-2 btn btn-outline-success" value="Сохранить"
                                          onClick={onSave}/> : ''}
            </div>
        </div>
    </>)
}

export default AddActorsProducers