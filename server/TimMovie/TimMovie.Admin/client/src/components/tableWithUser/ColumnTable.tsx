import React from "react"
import classes from "./table.module.css";
import {IUserDto} from "../../dto/IUserDto";
import {Link} from "react-router-dom";
import {classNameConcat} from "../../common/classNameConcat";

interface IColumnTableProps{
    nameColumn: string;
    users: IUserDto[];
    userPropName: string;
    isLinked: boolean;
}

function ColumnTable({nameColumn, users, userPropName, isLinked}: IColumnTableProps){
    return (
        <div className="d-flex flex-column">
            <h5>{nameColumn}</h5>
            {users.map((user: any) => isLinked 
                ? <Link to={`/users/${user.id}`} key={user.id} className={classes.linkToUser}>
                    <div className={classes.userRecord}>{user[userPropName]}</div>
                </Link> 
                : <div className={classes.userRecord} key={user.id}>{user[userPropName]}</div>)}
        </div>
    );
}

export default ColumnTable;