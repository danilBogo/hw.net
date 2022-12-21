export default interface ModalWindowControl {
    readonly messageIsShow: boolean;
    readonly setMessageIsShow: (value: boolean) => void;
    readonly messageText: string;
    readonly setMessageText: (value: string) => void;
}