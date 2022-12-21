import $api from "../../http";
import SubscribeDto from "./props/SubscribeDto";
import Result from "../../dto/Result";
import ModalWindow from "../common/modal/ModalWindow";
import useModalWindow from "../../hook/modal/useModalWindow";
import ToastNotification from "../common/modal/ToastNotification";
import SubscribeForm from "./SubscribeForm";
import SubscribeAddFormProps from "./props/SubscribeAddFormProps";

function SubscribeAddForm({setFetching, resetTable}: SubscribeAddFormProps) {
    const errorMessage = useModalWindow();
    const messageOnAddSubscribe = useModalWindow("Подписка успешно добавлена"); 
    
    async function addSubscribe(data: SubscribeDto){
        let response = await $api.post<Result<string>>("/subscribes/add", data);
        if (!response.data.success){
            errorMessage.setMessageText(response.data.textError!);
            errorMessage.setMessageIsShow(true);
            return false;
        }
        
        messageOnAddSubscribe.setMessageIsShow(true);
        resetTable();
        setFetching(true);
        return true;
    }
   
    return (
        <>
            <ToastNotification modalControl={messageOnAddSubscribe}/>
            <ModalWindow modalControl={errorMessage} headerText={"Ошибка"} headerClass={"bg-danger"}/>
            <SubscribeForm saveSubscribe={addSubscribe} isEdit={false}/>
        </>
    );
}

export default SubscribeAddForm;