import {ChangeEvent, Dispatch, FocusEvent} from "react";
import ValidationState from "../validation/ValidationState";

export default interface InputInfo{
    value: string;
    onChange: (e: ChangeEvent<Node>) => void;
    onBlur: (e: FocusEvent<Node>) => void;
    resetInput: () => void;
    isDirty: boolean;
    validationState: ValidationState;
    setValue: (value: string) => void;
}