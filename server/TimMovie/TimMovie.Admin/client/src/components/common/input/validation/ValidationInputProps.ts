import InputInfo from "../../../../hook/input/InputInfo";

export default interface ValidationInputProps{
    inputInfo: InputInfo,
    label: string;
    inputClasses?: string;
    typeInput: string;
    isRequired: boolean;
}