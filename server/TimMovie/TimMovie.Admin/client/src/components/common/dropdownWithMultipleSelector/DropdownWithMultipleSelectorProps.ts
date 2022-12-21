import {MutableRefObject} from "react";

export default interface DropdownWithMultipleSelectorProps{
    readonly values: MutableRefObject<Set<string>>;
    readonly urlRequestForEntity: string;
    readonly pagination: number;
    readonly title: string;
    readonly onClickDropdownButton?: () => void;
    readonly onChangeSelectedValues?: () => void; 
}