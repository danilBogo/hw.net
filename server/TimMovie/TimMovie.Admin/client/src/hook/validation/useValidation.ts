import {useEffect, useState} from "react";
import Validation from "./Validation";
import ValidationState from "./ValidationState";


export function useValidation<TValue>(value: TValue, validations: Validation<TValue>): ValidationState{
    const [errorMessage, setErrorMessage] = useState("");
    const [inputIsValid, setInputIsValid] = useState(false);
    
    useEffect(() => {
        let inputIsValid = true;
        let errorMessage = "";
        if (validations.validations){
            validations.validations.forEach(predicate => {
                let isValid = predicate.valueIsValid(value);
                
                if (!isValid){
                    errorMessage = predicate.errorMessage ?? "";
                }
                inputIsValid &&= isValid;
            })
        }
        
        setErrorMessage(errorMessage);
        setInputIsValid(inputIsValid);
    }, [value]);
    
    return {
        inputIsValid,
        errorMessage
    }
}