import React from "react";
import DropdownWithMultipleSelector from "./DropdownWithMultipleSelector";
import DropdownWithMultipleSelectorWithErrorProps from "./DropdownWithMultipleSelectorWithErrorProps";
import messageError from "../css/messageError.module.css"

function DropdownWithMultipleSelectorWithError({dropdownWithMultipleSelector, validationDropdown}: DropdownWithMultipleSelectorWithErrorProps){

    return (
        <>
            <DropdownWithMultipleSelector pagination={dropdownWithMultipleSelector.pagination} 
                                          values={dropdownWithMultipleSelector.values}
                                          urlRequestForEntity={dropdownWithMultipleSelector.urlRequestForEntity}
                                          title={dropdownWithMultipleSelector.title}
                                          onChangeSelectedValues={validationDropdown.valueChange}
                                          onClickDropdownButton={() => validationDropdown.setFirstClickOnDropdownButton(true)}/>
            {validationDropdown.firstClickOnDropdownButton && !validationDropdown.validation.inputIsValid
                && <div className={messageError.messageError}>{validationDropdown.validation.errorMessage}</div>}
        </>
    );
}

export default DropdownWithMultipleSelectorWithError;