export default interface ModalForEditGenreProps {
    readonly setIsShow: (value: boolean) => void;
    readonly isShow: boolean;
    readonly id: string;
    readonly genreName: string;
    readonly updateTable: () => void;
}