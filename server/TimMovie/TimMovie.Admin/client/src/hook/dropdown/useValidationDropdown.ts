import Validation from "../validation/Validation";
import {useValidation} from "../validation/useValidation";
import {useState} from "react";
import ValidationDropdown from "./ValidationDropdown";

export function useValidationDropdown<TValue>(values: TValue, validations: Validation<TValue>): ValidationDropdown{
    const [valueIsChange, setValueIsChange] = useState(true);
    const validation = useValidation(valueIsChange, {validations: 
        validations.validations?.map(value => {
            return {
                valueIsValid: _ => value.valueIsValid(values),
                errorMessage: value.errorMessage
            }
        })});
    const [firstClickOnDropdownButton, setFirstClickOnDropdownButton] = useState(false);

    function valueChange(): void{
        setValueIsChange(!valueIsChange);
    }
    
    return {
        valueChange,
        setFirstClickOnDropdownButton,
        validation,
        firstClickOnDropdownButton
    }
}

