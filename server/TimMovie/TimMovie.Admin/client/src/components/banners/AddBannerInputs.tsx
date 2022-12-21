import styles from "../common/css/adminStyles.module.css";
import React from "react";
import RequiredFieldIcon from "../common/symbols/RequiredFieldIcon";
import DropdownSearchOneValueWithError from "../common/dropdownSearchOneValue/DropdownSearchOneValueWithError";
import {useValidationDropdown} from "../../hook/dropdown/useValidationDropdown";

interface Props{
    preview: string | null,
    description: string | null,
    setDescription: Function
    film: string,
    setFilm: (newValue: string) => void,
    initialValueForFilm: string
}
function AddBannerInputs(props : Props) {

    const validationFilm = useValidationDropdown(props.film, {validations: [{
            valueIsValid: film => film != props.initialValueForFilm,
            errorMessage: "Нужно выбрать фильм"
        }]});

    function onDescriptionChange(description:any){
        description.preventDefault()
        props.setDescription(description.target.value)
    }

    return (<>
        <div className="mt-4 w-100 d-flex flex-row justify-content-around">
            <div className="d-flex flex-column">
                <label htmlFor="bannerDescriptionInput" className="text-center"><h5>Описание<RequiredFieldIcon/></h5></label>
                <textarea maxLength={250} id="bannerDescriptionInput" style={{resize: "none"}}
                          className={styles.adminInput} cols={60} rows={5} value={props.description!}  onChange={desc => onDescriptionChange(desc)}/>
            </div>
            <div style={{color:'black'}}>
                    <div className="mb-2">Фильм<RequiredFieldIcon/></div>
                        <DropdownSearchOneValueWithError dropdownSearchOneValue={{
                            value: props.film,
                            setValue: props.setFilm,
                            title: props.film,
                            pagination: 30,
                            urlRequestForEntity: "/films/collection"
                        }} validations={validationFilm}/>
            </div>
        </div>
    </>)
}

export default AddBannerInputs
