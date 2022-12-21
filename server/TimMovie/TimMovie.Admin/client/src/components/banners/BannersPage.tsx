import React, {useEffect, useRef, useState} from "react";
import {UploadProps} from "../common/upload/UploadProps";
import {UploadFiles} from "../common/upload/UploadFiles";
import AddBannerInputs from "./AddBannerInputs";
import $api from "../../http";
import {BannerDto} from "../../dto/BannerDto";
import styles from "./banner.module.css";
import NamePart from "../../common/interfaces/NamePart";
import usePagination from "../../hook/dynamicLoading/usePagination";
import DynamicScrolling from "../common/dynamicLoading/DynamicScrolling";
import useModalWindow from "../../hook/modal/useModalWindow";
import ToastNotification from "../common/modal/ToastNotification";
import ModalWindow from "../common/modal/ModalWindow";
import Banner from "./Banner";

function BannersPage(uploadProps: UploadProps) {

    const initialValueForFilm = 'Фильм';
    const [preview, setPreview] = useState<string | null>(null);
    const [description, setDescription] = useState<string>('')
    const [file, setFile] = useState<File>()
    const [film, setFilm] = useState(initialValueForFilm)
    const [isSubmitted, setIsSubmitted] = useState<boolean>(false)
    let urlQuery = useRef<NamePart>({namePart: ""});
    const pagination = usePagination<BannerDto, NamePart>({pagination: 5, url: `/banners/pagination`, urlQuery})
    const bannerAddedText = 'Баннер успешно добавлен'
    const bannerDeletedText = 'Банер успешно удален'
    const bannerNotification = useModalWindow('')
    const bannerModalNotification = useModalWindow()

    useEffect(() => {
        if (isSubmitted) {
            update()
            setIsSubmitted(false)
        }
    }, [isSubmitted])

    function update() {
        pagination.reset()
        pagination.setFetching(true)
    }

    function notify(text: string) {
        bannerNotification.setMessageText(text)
        bannerNotification.setMessageIsShow(true)
    }

    function notifyModal(text: string) {
        bannerModalNotification.setMessageText(text)
        bannerModalNotification.setMessageIsShow(true)
    }

    async function trySaveBanner(): Promise<void> {
        if (!description) {
            return;
        }
        let data = generateRequestDataForAdd()
        $api.post('/banners/add', data).then(response => {
            if (!response.data.success) {
                alert(response.data.textError ?? 'произошла ошибка')
            } else {
                setIsSubmitted(true)
                resetFields()
                notify(bannerAddedText)
            }
        })
    }

    function resetFields() {
        setFilm(initialValueForFilm)
        setDescription('')
        setPreview(null)
        setFile(undefined)
    }

    function generateRequestDataForAdd() {
        let formData = new FormData()
        formData.append("img", file!);
        formData.append("description", description!);
        formData.append("filmTitle", film!);
        return formData
    }

    function onDelete(bannerId: string) {
        let formData = new FormData()
        formData.append('bannerId', bannerId)
        $api.delete(`/banners/${bannerId}`).then(response => {
            if (response.data.success) {
                setIsSubmitted(true)
                notifyModal(bannerDeletedText)
            } else {
                notifyModal(response.data.textError)
            }
        })
    }

    return (<>
        <div className="justify-content-center d-flex flex-column align-items-center">
            <ToastNotification modalControl={bannerNotification} headerClass={'text-success'}/>
            <ModalWindow modalControl={bannerModalNotification} headerText={'Удаление баннера'}/>
            <h1 className="mt-4">Добавить новый баннер</h1>
            <div className="mt-2 position-relative text-break">
                <UploadFiles uploadProps={uploadProps}
                             uploadHooks={{preview: preview, setPreview: setPreview, setFile: setFile}}/>
                {preview && (description || film) ?
                    <div className={styles.bannerContainer} style={{position: "absolute", bottom: "13%"}}>
                        <h1 className={styles.bannerFilmTitle}>{film}</h1>
                        <p className={styles.bannerFilmDescription}>{description}</p>
                        {film != initialValueForFilm && description ?
                            <input type="button" className="btn btn-lg btn-success" value="Сохранить"
                                   onClick={trySaveBanner}/> : ''}
                    </div> : ''}
            </div>
            <AddBannerInputs preview={preview} description={description} setDescription={setDescription} film={film}
                             setFilm={setFilm} initialValueForFilm={initialValueForFilm}/>
            <hr className="w-100"/>
            <DynamicScrolling paginationResult={pagination} amountScreenBeforeLoading={1.5}>
                <div className="d-flex flex-column">
                    {pagination.records.map(banner =>
                        <Banner onDelete={onDelete} banner={banner} key={banner.bannerId}/>
                    )}
                </div>
            </DynamicScrolling>
        </div>
    </>)
}

export default BannersPage