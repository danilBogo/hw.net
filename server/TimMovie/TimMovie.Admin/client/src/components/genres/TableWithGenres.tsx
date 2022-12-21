import SearchForPagination from "../common/search/SearchForPagination";
import DynamicScrolling from "../common/dynamicLoading/DynamicScrolling";
import {Table} from "react-bootstrap";
import React, {useState} from "react";
import TableWithGenresProps from "./props/TableWithGenresProps";
import TableRowWithGenre from "./TableRowWithGenre";
import useModalWindow from "../../hook/modal/useModalWindow";
import $api from "../../http";
import Result from "../../dto/Result";
import ModalWindow from "../common/modal/ModalWindow";
import ModalForEditGenre from "./ModalForEditGenre";

function TableWithGenres({paginationResult, urlQuery}: TableWithGenresProps) {
    const notification = useModalWindow();
    const [headerText, setHeaderText] = useState("");
    const [idEdit, setIdEdit] = useState("");
    const [nameEdit, setNameEdit] = useState("");
    const [modalForEditIsShow, setModalForEditIsShow] = useState(false);
    
    async function deleteGenre(id: string){
        let url = `/genres/${id}`;
        $api.delete<Result<string>>(url)
            .then(response => {
                if (!response.data.success){
                    showMessage("Не удалось удалить фильм, попробуйте перезагрузить страницу.",
                        "Ошибка!");
                    return;
                }

                showMessage("Жанр успешно удален.", "Успех!");
                
                updateTable();
            })
            .catch(() => {
                showMessage("Ошибка связи с сервером, попробуйте позже",
                    "Ошибка")
            });
    }
    
    function showMessage(message: string, header: string){
        setHeaderText(header);
        notification.setMessageText(message);
        notification.setMessageIsShow(true);
    }
    
    function updateTable(){
        paginationResult.reset();
        paginationResult.setFetching(true);
    }
    
    function editGenre(id: string, name: string){
        setIdEdit(id);
        setNameEdit(name);
        setModalForEditIsShow(true);
    }
    
    return(
        <>
            <ModalForEditGenre setIsShow={setModalForEditIsShow} isShow={modalForEditIsShow} 
                               id={idEdit} genreName={nameEdit} updateTable={updateTable}/>
            <ModalWindow modalControl={notification} headerText={headerText}/>
            <div className="mt-5 mb-5">
                <SearchForPagination pagination={paginationResult} label={"Поиск по названию"} className={"form-control"} stringForSearch={urlQuery}/>
                <DynamicScrolling paginationResult={paginationResult} amountScreenBeforeLoading={1.5} className="mt-5">
                    <div className="w-50">
                        <Table hover bordered size="sm">
                            <thead>
                            <tr className="">
                                <th className="text-center">#</th>
                                <th>Название</th>
                                <th/>
                            </tr>
                            </thead>
                            <tbody>
                            {paginationResult.records.map((genre, i) =>
                                <TableRowWithGenre key={genre.id} genre={genre} serialNumber={i + 1}
                                deleteGenre={deleteGenre} editGenre={editGenre}/>
                            )}
                            </tbody>
                        </Table>
                    </div>
                </DynamicScrolling>
            </div>
        </>
    );
}

export default TableWithGenres;