import DynamicScrolling from "../common/dynamicLoading/DynamicScrolling";
import React from "react";
import SearchForPagination from "../common/search/SearchForPagination";
import {Table} from "react-bootstrap";
import TableWithFilmsProps from "./TableWithFilmsProps";
import $api from "../../http";
import Result from "../../dto/Result";
import TableRowWithFilm from "./TableRowWithFilm";
import useModalWindow from "../../hook/modal/useModalWindow";
import ModalWindow from "../common/modal/ModalWindow";
import {AxiosResponseValidator} from "../../common/AxiosResponseValidator";

function TableWithFilms({paginationResult, urlQuery}: TableWithFilmsProps){
    const modalControl = useModalWindow();
    const messageOnFailDelete = "Не удалось удалить фильм, попробуйте перезагрузить страницу и повторить снова.";

    async function deleteFilm(id: string){
        let url = `/films/${id}`;
        let response = await $api.delete<Result<string>>(url);

        if(!AxiosResponseValidator.checkSuccessResponseStatusAndLogIfError(response)){
            return;
        }

        if (!response.data.success){
            modalControl.setMessageText(messageOnFailDelete);
            modalControl.setMessageIsShow(true);
            return;
        }

        modalControl.setMessageText("Фильм успешно удален.");
        modalControl.setMessageIsShow(true);
        
        paginationResult.reset();
        paginationResult.setFetching(true);
    }
    
    return (
        <>
            <ModalWindow modalControl={modalControl} headerText={"Результат удаления"}/>
            <div className="mt-5 mb-5">
                <SearchForPagination pagination={paginationResult} label={"Поиск по названию"} className={"form-control"} stringForSearch={urlQuery}/>
                <DynamicScrolling paginationResult={paginationResult} amountScreenBeforeLoading={1.5} className="mt-5">
                    <Table bordered hover>
                        <thead>
                        <tr className="">
                            <th/>
                            <th>Название</th>
                            <th>Страна</th>
                            <th>Год</th>
                            <th>Жанры</th>
                            <th>Продюсеры и актеры</th>
                            <th/>
                        </tr>
                        </thead>
                        <tbody>
                        {paginationResult.records.map(value =>
                            <TableRowWithFilm film={value} key={value.id} onDeleteFilm={deleteFilm}/>
                        )}
                        </tbody>
                    </Table>
                </DynamicScrolling>
            </div>
        </>
    );
}

export default TableWithFilms;