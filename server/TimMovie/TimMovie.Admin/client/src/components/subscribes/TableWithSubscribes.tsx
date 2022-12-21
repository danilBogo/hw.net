import useModalWindow from "../../hook/modal/useModalWindow";
import $api from "../../http";
import ModalWindow from "../common/modal/ModalWindow";
import SearchForPagination from "../common/search/SearchForPagination";
import DynamicScrolling from "../common/dynamicLoading/DynamicScrolling";
import {Table} from "react-bootstrap";
import React from "react";
import TableWithSubscribesProps from "./props/TableWithSubscribesProps";
import TableRowWithSubscribe from "./TableRowWithSubscribe";
import {ResultActionForUser} from "../../dto/ResultActionForUser";

function TableWithSubscribes({paginationResult, urlQuery}: TableWithSubscribesProps) {
    const notificationModal = useModalWindow();

    async function deleteFilm(id: string){
        let url = `/subscribes/${id}`;
        $api.delete<ResultActionForUser>(url)
            .then(response => {
                if (!response.data.success){
                    notificationModal.setMessageText(response.data.textMessageForUser!);
                    notificationModal.setMessageIsShow(true);
                    return;
                }
                
                notificationModal.setMessageText("Подписка успешно удалена");
                notificationModal.setMessageIsShow(true);

                paginationResult.reset();
                paginationResult.setFetching(true);
            })
            .catch(reason => {
                
            });
    }

    return (
        <>
            <ModalWindow modalControl={notificationModal} headerText={"Уведомление"}/>
            <div className="mt-5 mb-5">
                <SearchForPagination pagination={paginationResult} label={"Поиск по названию"} className={"form-control"} stringForSearch={urlQuery}/>
                <DynamicScrolling paginationResult={paginationResult} amountScreenBeforeLoading={1.5} className="mt-5">
                    <Table bordered hover>
                        <thead>
                        <tr className="">
                            <th>Название</th>
                            <th>Цена</th>
                            <th>Фильмы</th>
                            <th>Жанры</th>
                            <th>Активна?</th>
                            <th/>
                        </tr>
                        </thead>
                        <tbody>
                        {paginationResult.records.map(value =>
                            <TableRowWithSubscribe subscribe={value} key={value.id} onDeleteSubscribe={deleteFilm}/>
                        )}
                        </tbody>
                    </Table>
                </DynamicScrolling>
            </div>
        </>
    );
}

export default TableWithSubscribes;