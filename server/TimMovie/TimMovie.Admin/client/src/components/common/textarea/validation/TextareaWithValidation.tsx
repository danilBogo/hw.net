import messageErrorClasses from "../../css/messageError.module.css"
import React from "react";
import ValidationTextareaProps from "./ValidationTextareaProps";
import RequiredFieldIcon from "../../symbols/RequiredFieldIcon";

function TextareaWithValidation({inputInfo, label, isRequired=false, ...props}: ValidationTextareaProps){
    return (
        <>
            <div>{label} {isRequired && <RequiredFieldIcon/>}</div>
            <textarea onChange={inputInfo.onChange} onBlur={inputInfo.onBlur} value={inputInfo.value} 
                      className={props.inputClasses}/>
            {(inputInfo.isDirty && !inputInfo.validationState.inputIsValid) &&
                <div className={messageErrorClasses.messageError}>{inputInfo.validationState.errorMessage}</div>}
        </>
    );
}

export default TextareaWithValidation;