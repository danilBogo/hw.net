import messageErrorClasses from "../../css/messageError.module.css"
import React from "react";
import ValidationInputProps from "./ValidationInputProps";
import RequiredFieldIcon from "../../symbols/RequiredFieldIcon";

function InputWithValidation({inputInfo, label, ...props}: ValidationInputProps){
    return (
        <>
            <div>{label} {props.isRequired && <RequiredFieldIcon/>}</div>
            <input onChange={inputInfo.onChange} onBlur={inputInfo.onBlur}
                   value={inputInfo.value} type={props.typeInput}  
                   className={props.inputClasses}/>
            {(inputInfo.isDirty && !inputInfo.validationState.inputIsValid) &&
                <div className={messageErrorClasses.messageError}>{inputInfo.validationState.errorMessage}</div>}
        </>  
    );
}

export default InputWithValidation;