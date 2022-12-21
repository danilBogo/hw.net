import TableRowWithSubscribeProps from "./props/TableRowWithSubscribeProps";
import {Link} from "react-router-dom";
import React from "react";
import tableClasses from "./styles/tableRowForGenre.module.css"

function TableRowWithSubscribe({onDeleteSubscribe, subscribe}: TableRowWithSubscribeProps){
    return (
        <tr style={{verticalAlign: "middle"}} className="text-center">
            <td width={220} className={tableClasses.tableSubName}>{subscribe.name}</td>
            <td width={110}>{subscribe.price}</td>
            <td >
                <ul className="list-group" style={{overflow: "auto", height:150}}>
                    {subscribe.films.map(film =>
                        <li key={film} className="list-group-item">{film}</li>)}
                </ul>
            </td>
            <td width={350}>
                <ul className="list-group" style={{overflow: "auto", height:150}}>
                    {subscribe.genres.map(genre =>
                        <li key={genre} className="list-group-item">{genre}</li>)}
                </ul>
            </td>
            <td width={80} className="text-center">
                {subscribe.isActive ?
                    <div title="Активна">
                        <svg xmlns="http://www.w3.org/2000/svg" width="25"
                             className={tableClasses.isActive} viewBox="0 0 16 16">
                            <path
                                d="M12.354 4.354a.5.5 0 0 0-.708-.708L5 10.293 1.854 7.146a.5.5 0 1 0-.708.708l3.5 3.5a.5.5 0 0 0 .708 0l7-7zm-4.208 7-.896-.897.707-.707.543.543 6.646-6.647a.5.5 0 0 1 .708.708l-7 7a.5.5 0 0 1-.708 0z"/>
                            <path d="m5.354 7.146.896.897-.707.707-.897-.896a.5.5 0 1 1 .708-.708z"/>
                        </svg>
                    </div> :
                    <div title="Не активна">
                        <svg xmlns="http://www.w3.org/2000/svg" width="25"
                             className={tableClasses.isNotActive} viewBox="0 0 16 16">
                            <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
                            <path
                                d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z"/>
                        </svg>
                    </div>
                }
            </td>
            <td width={50}>
                <div className="d-flex flex-column align-items-center">
                    <Link title="Редактировать" style={{cursor: "pointer"}} to={`/subscribes/${subscribe.id}`}>
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16"
                             fill="currentColor" className="bi bi-pencil" viewBox="0 0 16 16">
                            <path
                                d="M12.146.146a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1 0 .708l-10 10a.5.5 0 0 1-.168.11l-5 2a.5.5 0 0 1-.65-.65l2-5a.5.5 0 0 1 .11-.168l10-10zM11.207 2.5 13.5 4.793 14.793 3.5 12.5 1.207 11.207 2.5zm1.586 3L10.5 3.207 4 9.707V10h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.293l6.5-6.5zm-9.761 5.175-.106.106-1.528 3.821 3.821-1.528.106-.106A.5.5 0 0 1 5 12.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.468-.325z"/>
                        </svg>
                    </Link>
                    <div onClick={() => onDeleteSubscribe(subscribe.id)} className="mt-5" title="Удалить">
                        <svg style={{cursor: "pointer"}} xmlns="http://www.w3.org/2000/svg" width="20"
                             fill="red" className="bi bi-trash" viewBox="0 0 16 16">
                            <path
                                d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z"/>
                            <path fillRule="evenodd"
                                  d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z"/>
                        </svg>
                    </div>
                </div>
            </td>
        </tr>
    );
}

export default TableRowWithSubscribe;