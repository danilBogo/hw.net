import DynamicScrolling from "../common/dynamicLoading/DynamicScrolling";
import SearchForPagination from "../common/search/SearchForPagination";
import ActorsProducersRaw from "./ActorsProducersRaw";
import {Table} from "react-bootstrap";
import $api from "../../http";
import {PersonDto} from "../../dto/PersonDto";
import PaginationResult from "../../hook/dynamicLoading/PaginationResult";
import {MutableRefObject, useState} from "react";
import NamePart from "../../common/interfaces/NamePart";

interface ActorsProducersTableProps {
    update: Function,
    readonly paginationResult: PaginationResult<PersonDto>,
    readonly urlQuery: MutableRefObject<NamePart>
}

function getTypeInRussian(type: string): string{
    return type == 'actor' ? 'Актер' : 'Режиссер'
}

function ActorsProducersTable(props: ActorsProducersTableProps) {
    function onDelete(type: string, id: string) {
        $api.delete(`${type}s/${id}`).then(response => {
            if (response.data.success) {
                props.update()
                alert(`${getTypeInRussian(type)} успешно удален`)
            } else {
                alert(`не удалось удалить ${getTypeInRussian(type)}а: ${response.data.textError}`)
            }
        })

    }


    return (<>
        <SearchForPagination pagination={props.paginationResult} label={"Поиск по имени/фамилии"}
                             className={"form-control"}
                             stringForSearch={props.urlQuery}/>
        <DynamicScrolling paginationResult={props.paginationResult} amountScreenBeforeLoading={1.5}>
            <Table bordered hover className="mt-5">
                <thead>
                <tr className="">
                    <th/>
                    <th>Имя</th>
                    <th>Фамилия</th>
                    <th>Тип</th>
                    <th/>
                </tr>
                </thead>
                <tbody>
                {props.paginationResult.records.map(person =>
                    <ActorsProducersRaw id={person.id} photo={person.photo} name={person.name} surname={person.surname}
                                        update={() => props.update()}
                                        key={person.id} type={person.type}
                                        getTypeInRussian={getTypeInRussian}
                                        onDelete={() => onDelete(person.type, person.id)}/>
                )}
                </tbody>
            </Table>
        </DynamicScrolling>
    </>)
}

export default ActorsProducersTable