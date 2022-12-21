export default interface DropdownWithSearchProps<TResponse>{
    readonly url: string;
    readonly parsedResultFunction: (value: TResponse) => JSX.Element;
    readonly pagination: number; 
    readonly title: string;
    showMenu: boolean;
    setShowMenu: (value: boolean) => void;
    onClickDropdown?: () => void
}