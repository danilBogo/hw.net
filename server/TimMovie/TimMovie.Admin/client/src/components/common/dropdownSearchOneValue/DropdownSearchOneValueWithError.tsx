import messageError from "../css/messageError.module.css";
import React, {useEffect} from "react";
import DropdownSearchOneValue from "./DropdownSearchOneValue";
import DropdownSearchOneValueWithErrorProps from "./DropdownSearchOneValueWithErrorProps";

function DropdownSearchOneValueWithError({dropdownSearchOneValue, validations}: DropdownSearchOneValueWithErrorProps){
    useEffect(() => {
        validations.valueChange();
    }, [dropdownSearchOneValue.value])

    return (
        <>
            <DropdownSearchOneValue pagination={dropdownSearchOneValue.pagination}
                                          value={dropdownSearchOneValue.value}
                                          urlRequestForEntity={dropdownSearchOneValue.urlRequestForEntity}
                                          title={dropdownSearchOneValue.title}
                                          onClickDropdownButton={() => validations.setFirstClickOnDropdownButton(true)} 
                                    setValue={dropdownSearchOneValue.setValue}/>
            {validations.firstClickOnDropdownButton && !validations.validation.inputIsValid
                && <div className={messageError.messageError}>{validations.validation.errorMessage}</div>}
        </>
    );
}

export default DropdownSearchOneValueWithError;