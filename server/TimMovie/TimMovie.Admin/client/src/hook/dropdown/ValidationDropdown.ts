import ValidationState from "../validation/ValidationState";

export default interface ValidationDropdown{
    readonly valueChange: () => void;
    readonly setFirstClickOnDropdownButton: (value: boolean) => void;
    readonly validation: ValidationState;
    readonly firstClickOnDropdownButton: boolean;
} 