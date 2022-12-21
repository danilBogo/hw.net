import {useInput} from "../../hook/input/useInput";
import {checkOnEmpty} from "../../templeteForValidationWithError/templetes";
import InputWithValidation from "../common/input/validation/InputWithValidation";
import RequiredFieldIcon from "../common/symbols/RequiredFieldIcon";
import DropdownWithMultipleSelectorWithError
    from "../common/dropdownWithMultipleSelector/DropdownWithMultipleSelectorWithError";
import React, {useEffect, useRef, useState} from "react";
import {useValidationDropdown} from "../../hook/dropdown/useValidationDropdown";
import TextareaWithValidation from "../common/textarea/validation/TextareaWithValidation";
import {Button} from "react-bootstrap";
import "./styles/genreForm.css"
import SubscribeFormProps from "./props/SubscribeFormProps";
import SubscribeDto from "./props/SubscribeDto";

function SubscribeForm({saveSubscribe, initializationForm, isEdit}: SubscribeFormProps) {
    const nameSubscribe = useInput({
        validations: [
            checkOnEmpty,
            {
                valueIsValid: value => value.length < 70,
                errorMessage: "Максимальная длина 70 символов"
            }
        ]
    });
    const price = useInput({
        validations: [
            {
                valueIsValid: value =>  +value > 0,
                errorMessage: "Цена должна быть больше нуля"
            },
            {
                valueIsValid: value =>  +value <= 1_000_000_000,
                errorMessage: "Цена превышает 1 млрд"
            },
            checkOnEmpty
        ]
    });
    const films = useRef(new Set<string>());
    const genres = useRef(new Set<string>());
    const validationGenresAndFilms = useValidationDropdown(films, {
        validations: [{
            valueIsValid: () => filmsOrGenreIsSelected(),
            errorMessage: "Выберите хотя бы один жанр или фильм"
        }]
    });
    const description = useInput({
        validations: [checkOnEmpty]
    })
    const [isActive, setIsActive] = useState(false);
    
    function filmsOrGenreIsSelected(){
        return films.current.size !== 0 || genres.current.size !== 0;
    }
    
    function formIsValid(){
        return nameSubscribe.validationState.inputIsValid && price.validationState.inputIsValid 
            && validationGenresAndFilms.validation.inputIsValid && description.validationState.inputIsValid;
    }
    
    function generateData(): SubscribeDto{
        return {
            name: nameSubscribe.value,
            price: +price.value,
            description: description.value,
            films: Array.from(films.current),
            genres: Array.from(genres.current),
            isActive: isActive
        }
    }
    
    async function save(){
        if (!formIsValid()){
            return;
        }
        
        let dataSub = generateData();
        if(await saveSubscribe(dataSub) && !isEdit){
            resetForm();
        }
    }
    
    useEffect(() => {
        if (!initializationForm){
            return;
        }
        
        nameSubscribe.setValue(initializationForm.name);
        price.setValue(initializationForm.price.toString());
        description.setValue(initializationForm.description);
        films.current = new Set(initializationForm.films);
        genres.current = new Set(initializationForm.genres);
        validationGenresAndFilms.valueChange();
        setIsActive(initializationForm.isActive);
    }, [initializationForm])
    
    function resetForm(){
        nameSubscribe.resetInput();
        price.resetInput();
        description.resetInput();
        films.current.clear();
        genres.current.clear();
        validationGenresAndFilms.setFirstClickOnDropdownButton(false);
        validationGenresAndFilms.valueChange();
        setIsActive(false);
    }
    
    return (
        <>
            <div className="mt-5 d-flex flex-column">
                <div className={"d-flex justify-content-between align-items-end"}>
                    <div>
                        <InputWithValidation inputInfo={nameSubscribe} label={"Название"} typeInput={"text"} isRequired={true}/>
                    </div>
                    <div>
                        <InputWithValidation inputInfo={price} label={"Цена"} typeInput={"number"} isRequired={true}/>
                    </div>
                    <div className="col-3">
                        <div className="mb-2">Фильмы {!filmsOrGenreIsSelected() && <RequiredFieldIcon/>}</div>
                        <DropdownWithMultipleSelectorWithError
                            dropdownWithMultipleSelector={{
                                title: "Выбрать",
                                values: films,
                                urlRequestForEntity: "/films/collection",
                                pagination: 30
                        }}
                            validationDropdown={validationGenresAndFilms}/>
                    </div>
                    <div className="col-3">
                        <div className="mb-2">Жанры {!filmsOrGenreIsSelected() && <RequiredFieldIcon/>}</div>
                        <DropdownWithMultipleSelectorWithError
                            dropdownWithMultipleSelector={{
                                title: "Выбрать",
                                values: genres,
                                urlRequestForEntity: "/genres/collection",
                                pagination: 30
                            }}
                            validationDropdown={validationGenresAndFilms}/>
                    </div>
                </div>
                <div className={"mt-4 col-6"}>
                    <TextareaWithValidation inputInfo={description} label={"Описание"} 
                                            inputClasses={"form-control genre-form_description"} isRequired/>
                </div>
                <div className="d-flex align-items-center mt-4">
                    <div>Подписка активна</div>
                    <input type="checkbox" checked={isActive} 
                           onChange={event => setIsActive(event.target.checked)} className="form-check-input ms-3" />
                </div>
                <Button type="button" disabled={!formIsValid()} onClick={save} 
                        className={"col-3"} variant="outline-success mt-5">
                    {isEdit ? "Сохранить" : "Добавить"}
                </Button>
            </div> 
        </>
    );
}

export default SubscribeForm;