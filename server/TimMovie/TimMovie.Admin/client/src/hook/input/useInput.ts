import {FocusEvent, useState} from "react";
import InputInfo from "./InputInfo";
import Validation from "../validation/Validation";
import {useValidation} from "../validation/useValidation";

export function useInput(validations: Validation<string>): InputInfo{
    const [value, setValue] = useState("");
    const [isDirty, setIsDirty] = useState(false);
    const valid = useValidation(value, validations);
    
    function onChange(e: any): void {
        setValue(e.target.value);
    }
    
    function onBlur(e: FocusEvent<Node>): void {
        setIsDirty(true);
    }
    
    function resetInput(){
        setIsDirty(false);
        setValue("");
    }
    
    return {
        value,
        onChange,
        onBlur,
        resetInput,
        setValue,
        isDirty,
        validationState: valid
    }
}