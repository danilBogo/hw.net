import Icon from "../../svg-icons/Icon";
import React from "react";
import TableRowWithGenreProps from "./props/TableRowWithGenreProps";

function TableRowWithGenre({genre, serialNumber, ...props}: TableRowWithGenreProps) {
    return (
        <tr key={genre.id}  style={{verticalAlign: "middle"}}>
            <td width={80} className="text-center">{serialNumber}</td>
            <td width={350}>{genre.name}</td>
            <td className="justify-content-center d-flex">
                <div onClick={() => props.editGenre(genre.id, genre.name)} className="me-3">
                    <Icon name="Edit" width={16} height={16} styles={{cursor: "pointer"}}/>
                </div>
                <div onClick={() => props.deleteGenre(genre.id)} className="ms-5">
                    <Icon name="Trash" width={16} height={16}
                          styles={{cursor: "pointer", fill: "red"}}/>
                </div>
            </td>
        </tr>
    );
}

export default TableRowWithGenre;