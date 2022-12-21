import {Toast, ToastContainer} from "react-bootstrap";
import React from "react";
import {classNameConcat} from "../../../common/classNameConcat";
import ToastNotificationProps from "./ToastNotificationProps";

export default function ToastNotification({modalControl, ...props}: ToastNotificationProps){
    return (
        <ToastContainer className="p-3" position="top-center">
            <Toast delay={3000} show={modalControl.messageIsShow} 
                   animation bg="dark" onClose={() => modalControl.setMessageIsShow(false)}
                   autohide>
                <Toast.Header className={props.headerClass}>
                    <strong className="me-auto">TimMovie</strong>
                </Toast.Header>
                <Toast.Body className={classNameConcat("text-white", props.bodyClass ?? "")}>
                    {modalControl.messageText}
                </Toast.Body>
            </Toast>
        </ToastContainer>
    );
}