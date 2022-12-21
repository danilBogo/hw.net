import {Button, Modal} from "react-bootstrap";
import {useInput} from "../../hook/input/useInput";
import {checkOnEmpty} from "../../templeteForValidationWithError/templetes";
import useModalWindow from "../../hook/modal/useModalWindow";
import ModalWindow from "../common/modal/ModalWindow";
import ModalForEditGenreProps from "./props/ModalForEditGenreProps";
import {useEffect} from "react";
import InputWithValidation from "../common/input/validation/InputWithValidation";
import $api from "../../http";
import Result from "../../dto/Result";

function ModalForEditGenre({isShow, setIsShow, ...props}: ModalForEditGenreProps){
    const genreName = useInput({validations: [
            checkOnEmpty,
            {
                valueIsValid: value => value?.length < 50,
                errorMessage: "Максимальная длина 50"
            }
        ]});
    const message = useModalWindow();
    
    useEffect(() => {
        if(isShow){
            genreName.resetInput();
            genreName.setValue(props.genreName);
        }
    }, [isShow]);
    
    function trySaveChanges(){
        if (!genreName.validationState.inputIsValid){
            return;
        }

        $api.post<Result<string>>(`/genres/${props.id}`, {name: genreName.value})
            .then(response => {
                if (!response.data.success){
                    message.setMessageText(response.data.textError ?? "Неизвестная ошибка." +
                        " Попробуйте перезагрузить страницу.");
                    message.setMessageIsShow(true);
                    return;
                }

                message.setMessageText("Название жанра успешно обновлено.");
                message.setMessageIsShow(true);
                setIsShow(false);
                props.updateTable();
            })
    }
    
    function onKeypress(e: KeyboardEvent){
        if (e.key === "Enter" && isShow){
            trySaveChanges();
        }
    }
    
    useEffect(() => {
        window.addEventListener("keypress", onKeypress);
        
        return function () {
            window.removeEventListener("keypress", onKeypress);
        }
    })

    return (
        <>
            <ModalWindow modalControl={message} headerText={"Результат"}/>
            <Modal show={isShow} onHide={() => setIsShow(false)} centered backdrop="static">
                <Modal.Body>
                    <div className="d-flex flex-column">
                        <div className="mt-2">
                            <InputWithValidation inputInfo={genreName} label="Название жанра"
                                                 typeInput="text" isRequired={true}/>
                        </div>
                        <div className="mt-5">
                            <Button  variant="outline-danger" onClick={() => setIsShow(false)}>
                                Отмена
                            </Button >
                            <Button disabled={!genreName.validationState.inputIsValid} variant="outline-success" 
                                    className="ms-3" onClick={trySaveChanges}>
                                Сохранить
                            </Button>
                        </div>
                    </div>
                </Modal.Body>
            </Modal>
        </>
    );
}

export default ModalForEditGenre;