import {useInput} from "../../hook/input/useInput";
import {checkOnEmpty} from "../../templeteForValidationWithError/templetes";
import InputWithValidation from "../common/input/validation/InputWithValidation";
import GenreFormAddProps from "./props/GenreFormAddProps";
import $api from "../../http";
import Result from "../../dto/Result";
import ModalWindow from "../common/modal/ModalWindow";
import ToastNotification from "../common/modal/ToastNotification";
import useModalWindow from "../../hook/modal/useModalWindow";

function GenreFormAdd({setFetching, resetTable}: GenreFormAddProps){
    const genreName = useInput({validations: [
            checkOnEmpty,
            {
                valueIsValid: value => value?.length < 50,
                errorMessage: "Максимальная длина 50"
            }
        ]});
    const errorMessage = useModalWindow();
    const messageAboutAddGenre = useModalWindow("Жанр успешно добавлен"); 
    
    function addNewGenre(){
        if (!genreName.validationState.inputIsValid){
            return;
        }
        
        $api.post<Result<string>>("/genres/add", {genreName: genreName.value})
            .then(response => {
                if (!response.data.success){
                    errorMessage.setMessageText(response.data.textError ?? "Неизвестная ошибка." +
                        " Попробуйте перезагрузить страницу.");
                    errorMessage.setMessageIsShow(true);
                    return;
                }
                
                messageAboutAddGenre.setMessageIsShow(true);
                updatePageAfterAdd();
            })
    }
    
    function resetForm(){
        genreName.resetInput();
    }
    
    function updatePageAfterAdd(){
        resetTable();
        setFetching(true);
        resetForm();
    }

    return (
        <>
            <ToastNotification modalControl={messageAboutAddGenre}/>
            <ModalWindow modalControl={errorMessage} headerText={"Ошибка"} headerClass="bg-danger"/>
            <div className="d-flex">
                <div>
                    <InputWithValidation inputInfo={genreName} label={"Название"} typeInput={"text"} isRequired={true}/>
                </div>
                <div className="d-flex flex-column-reverse ms-5">
                    <button disabled={!genreName.validationState.inputIsValid}
                            className="btn btn-outline-success ms-5" onClick={addNewGenre}>Добавить</button>
                </div>
            </div>
        </>
    );
}

export default GenreFormAdd;