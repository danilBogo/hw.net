import InputInfo from "../../../../hook/input/InputInfo";

export default interface ValidationTextareaProps{
    inputInfo: InputInfo,
    label: string;
    isRequired?: boolean;
    inputClasses?: string;
}