import {useState} from "react";
import ModalWindowControl from "./ModalWindowControl";

export default function useModalWindow(initialMessage?: string): ModalWindowControl{
    const [messageIsShow, setMessageIsShow] = useState(false);
    const [messageText, setMessageText] = useState(initialMessage ?? "");
    
    return {
        messageIsShow,
        setMessageIsShow,
        messageText,
        setMessageText
    }
} 