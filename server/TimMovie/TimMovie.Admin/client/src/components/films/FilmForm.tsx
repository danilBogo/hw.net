import React, {SyntheticEvent, useEffect, useRef, useState} from "react";
import {UploadFiles} from "../common/upload/UploadFiles";
import filmFormClasses from "./filmForm.module.css";
import {classNameConcat} from "../../common/classNameConcat";
import DropdownWithMultipleSelector from "../common/dropdownWithMultipleSelector/DropdownWithMultipleSelector";
import {useInput} from "../../hook/input/useInput";
import InputWithValidation from "../common/input/validation/InputWithValidation";
import TextareaWithValidation from "../common/textarea/validation/TextareaWithValidation";
import {checkOnEmpty} from "../../templeteForValidationWithError/templetes";
import {useValidationDropdown} from "../../hook/dropdown/useValidationDropdown";
import DropdownWithMultipleSelectorWithError
    from "../common/dropdownWithMultipleSelector/DropdownWithMultipleSelectorWithError";
import DropdownSearchOneValueWithError from "../common/dropdownSearchOneValue/DropdownSearchOneValueWithError";
import RequiredFieldIcon from "../common/symbols/RequiredFieldIcon";
import errorMessageClasses from "../common/css/messageError.module.css";
import FilmFormProps from "./FilmFormProps";

const initialValueForCountry = "Выбрать";

function FilmForm({formInitialization, ...props}: FilmFormProps) {
    const [preview, setPreview] = useState<string | null>(null);
    const [file, setFile] = useState<File>();
    const [imgIsValid, setImgIsValid] = useState(true);
    const selectedActors = useRef(new Set<string>());
    const selectedProducers = useRef(new Set<string>());
    const selectedGenres = useRef(new Set<string>());
    const validationGenres = useValidationDropdown(selectedGenres.current, {validations: [{
            valueIsValid: genres => genres.size != 0,
            errorMessage: "Нужно выбрать хотя бы один жанр"
        }]});
    const title = useInput({validations: [
            checkOnEmpty,
            {
                valueIsValid: title => title.length < 200,
                errorMessage: "Максимальная длина 200 символов"
            }
        ]});
    const iframeLink = useInput({validations: [checkOnEmpty]});
    const description = useInput({});
    const [isFree, setIsFree] = useState(false);
    const year = useInput({validations: [
            {
                valueIsValid: value => Number(value) >= 1900 && Number(value) <= new Date().getFullYear(),
                errorMessage: `Дата должна находится в промежутке от 1900 до ${new Date().getFullYear()}`
            },
            checkOnEmpty
        ]});
    const [selectedCountry, setSelectedCountry] = useState(initialValueForCountry);
    const validationCountry = useValidationDropdown(selectedCountry, {validations: [{
            valueIsValid: country => country != initialValueForCountry,
            errorMessage: "Нужно выбрать страну"
        }]});
    const [formIsValid, setFormIsValid] = useState(false);

    function generateRequestData(): FormData{
        const formData = new FormData();
        if (file){
            formData.append("img", file);
        }
        formData.append("title", title.value);
        formData.append("filmLink", iframeLink.value);
        formData.append("description", description.value);
        formData.append("isFree", `${isFree}`);
        formData.append("year", year.value);
        formData.append("countryName", selectedCountry);
        appendCollection(formData, selectedActors.current, "actorNames");
        appendCollection(formData, selectedProducers.current, "producerNames");
        appendCollection(formData, selectedGenres.current, "genreNames");
        
        return formData;
    }
    
    useEffect(() => {
        setFormIsValid(checkFormOnValid());
    }, [title, iframeLink, description, year, validationGenres, validationCountry, file]);
    
    useEffect(() =>{
        if (!formInitialization){
            return;
        }

        setPreview(formInitialization.image);
        selectedActors.current= new Set<string>(formInitialization.actorNames);
        selectedProducers.current= new Set<string>(formInitialization.producerNames);
        selectedGenres.current= new Set<string>(formInitialization.genreNames);
        validationGenres.valueChange();
        setSelectedCountry(formInitialization.countryName);
        title.setValue(formInitialization.title);
        iframeLink.setValue(formInitialization.filmLink);
        description.setValue(formInitialization.description ?? "");
        year.setValue(formInitialization.year.toString());
        setIsFree(formInitialization.isFree);
    }, [formInitialization]);

    function onLoadImage(e: SyntheticEvent<HTMLImageElement>): void{
        let img = e.currentTarget;
        
        let validResult = img.naturalWidth >= 250 && img.naturalHeight >= 350;
        setImgIsValid(validResult);
        if (!validResult){
            setPreview(null);
            setFile(undefined);
        }
    }
    
    function appendCollection(formData: FormData, set: Iterable<string>, name: string){
        let i = 0;
        
        // @ts-ignore
        for (let value of set){
            formData.append(`${name}[]`, value);
            i++;
        }
    }
    
    async function trySaveFilm(): Promise<void>{
        if (!checkFormOnValid()){
            return;
        }
        let data = generateRequestData();
        
        if (await props.trySaveFilm(data) && !props.isEdit){
            resetValueFields();
        }
    }
    
    function resetValueFields(){
        setPreview(null);
        setFile(undefined);
        selectedActors.current.clear();
        selectedProducers.current.clear();
        selectedGenres.current.clear();
        title.resetInput();
        iframeLink.resetInput();
        description.resetInput();
        setIsFree(false);
        year.resetInput();
        setSelectedCountry(initialValueForCountry);
        validationGenres.setFirstClickOnDropdownButton(false);
        validationCountry.setFirstClickOnDropdownButton(false);
    }
    
    function checkFormOnValid(): boolean{
        return title.validationState.inputIsValid && iframeLink.validationState.inputIsValid
        && description.validationState.inputIsValid && year.validationState.inputIsValid 
            && validationGenres.validation.inputIsValid && validationCountry.validation.inputIsValid
            && !!preview;
    }
    
    return (
        <>
            <div className="d-flex">
                <div className="">
                    <UploadFiles
                        uploadProps={{photoWidth: 234, photoHeight:360, borderRadius: 10}}
                        uploadHooks={{preview:preview, setPreview: setPreview, setFile: setFile}}
                        onLoadPreview={onLoadImage}/>
                    {!imgIsValid && <div className={errorMessageClasses.messageError}>Минимальный размер по ширине 
                        <br/> 250px, а по высоте 350px</div>}
                </div>
                <form className={classNameConcat("d-flex flex-column flex-grow-1 mt-1", filmFormClasses.formWithFieldsFilm)}>
                    <div className="d-flex justify-content-between">
                        <div>
                            <InputWithValidation inputInfo={title} label="Название" isRequired={true}
                                                 inputClasses={"mt-2 form-control"} typeInput="text"/>
                        </div>
                        <div className="col-6">
                            <InputWithValidation inputInfo={iframeLink} label="Ссылка на iframe" isRequired={true}
                                inputClasses={"mt-2 form-control"} typeInput="text"/>
                        </div>
                    </div>
                    <div className="mt-4">
                        <TextareaWithValidation inputInfo={description} label="Описание"
                                                inputClasses={classNameConcat("mt-2 form-control", 
                                                    filmFormClasses.filmDescription)}/>
                    </div>
                    <div className="d-flex justify-content-between mt-4">
                        <div className="col-3">
                            <div className="mb-2">Актеры</div>
                            <DropdownWithMultipleSelector pagination={30} values={selectedActors}
                                                          urlRequestForEntity={"/actors/collection"}
                                                          title="Выбрать"/>
                        </div>
                        <div className="col-3">
                            <div className="mb-2">Продюсеры</div>
                            <DropdownWithMultipleSelector pagination={30} values={selectedProducers}
                                                          urlRequestForEntity={"/producers/collection"}
                                                          title="Выбрать"/>
                        </div>
                        <div className="col-3">
                            <div className="mb-2">Жанры <RequiredFieldIcon/></div>
                            <DropdownWithMultipleSelectorWithError 
                                dropdownWithMultipleSelector={{
                                    title: "Выбрать",
                                    values: selectedGenres,
                                    urlRequestForEntity: "/genres/collection",
                                    pagination: 30
                            }} 
                                validationDropdown={validationGenres}/>
                        </div>
                    </div>
                    <div className="d-flex justify-content-between mt-4">
                        <div className="col-4">
                            <InputWithValidation inputInfo={year} label="Год" isRequired={true}
                                                 inputClasses={"mt-2 form-control"} typeInput="number"/>
                        </div>
                        <div className="col-3">
                            <div className="mb-2">Страна <RequiredFieldIcon/></div>
                            <DropdownSearchOneValueWithError dropdownSearchOneValue={{
                               value: selectedCountry,
                               setValue: setSelectedCountry,
                               title: selectedCountry,
                               pagination: 30,
                               urlRequestForEntity: "/countries/collection"
                            }} validations={validationCountry}/>
                        </div>
                    </div>
                    <div className="d-flex justify-content-between mt-4 align-items-center">
                        <div className="d-flex ">
                            <div>Бесплатный?</div>
                            <input type="checkbox" checked={isFree} onChange={event => setIsFree(event.target.checked)} className="form-check ms-2" />
                        </div>
                    </div>
                    <div className="d-flex justify-content-center">
                        <button disabled={!formIsValid}
                                className="btn btn-outline-success mt-4 col-3"
                                type="button"
                        onClick={trySaveFilm}> Сохранить</button>
                    </div>
                </form>
            </div>
        </>
    );
}

export default FilmForm;