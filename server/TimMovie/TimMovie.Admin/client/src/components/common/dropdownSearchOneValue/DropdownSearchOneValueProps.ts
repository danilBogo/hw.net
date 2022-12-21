export default interface DropdownSearchOneValueProps{
    readonly value: string;
    readonly setValue: (newValue: string) => void;
    readonly urlRequestForEntity: string;
    readonly pagination: number;
    readonly title: string;
    readonly onClickDropdownButton?: () => void;
}