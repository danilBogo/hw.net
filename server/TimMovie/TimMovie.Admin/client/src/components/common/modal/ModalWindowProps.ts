import ModalWindowControl from "../../../hook/modal/ModalWindowControl";

export default interface ModalWindowProps {
    readonly modalControl: ModalWindowControl;
    readonly headerText: string;
    readonly bodyClass?: string;
    readonly headerClass?: string;
    readonly onHide?: () => void;
}
