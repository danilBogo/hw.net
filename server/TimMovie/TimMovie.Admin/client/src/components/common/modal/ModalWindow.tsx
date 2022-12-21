import {Modal} from "react-bootstrap";
import React from "react";
import ModalWindowProps from "./ModalWindowProps";

function ModalWindow({modalControl, headerText, ...props}: ModalWindowProps){
    return (
        <Modal
            size="sm"
            show={modalControl.messageIsShow}
            onHide={() => {
                modalControl.setMessageIsShow(false)
                
                if (props.onHide){
                    props.onHide();
                }
            }}>
            <Modal.Header closeButton className={props.headerClass}>
                <Modal.Title>
                    {headerText}
                </Modal.Title>
            </Modal.Header>
            <Modal.Body className={props.bodyClass}>{modalControl.messageText}</Modal.Body>
        </Modal>);
}

export default ModalWindow;