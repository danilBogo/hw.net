import DropdownSearchOneValueProps from "./DropdownSearchOneValueProps";
import ValidationDropdown from "../../../hook/dropdown/ValidationDropdown";

export default interface DropdownSearchOneValueWithErrorProps{
    readonly dropdownSearchOneValue: DropdownSearchOneValueProps,
    readonly validations: ValidationDropdown
}