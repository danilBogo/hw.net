import DropdownWithMultipleSelectorProps from "./DropdownWithMultipleSelectorProps";
import ValidationDropdown from "../../../hook/dropdown/ValidationDropdown";

export default interface DropdownWithMultipleSelectorWithErrorProps{
    readonly dropdownWithMultipleSelector: DropdownWithMultipleSelectorProps
    readonly validationDropdown: ValidationDropdown
}