import ModalWindowControl from "../../../hook/modal/ModalWindowControl";

export default interface ToastNotificationProps {
    readonly modalControl: ModalWindowControl;
    readonly bodyClass?: string;
    readonly headerClass?: string;
}
