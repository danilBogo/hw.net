import React from 'react';
import LineWithSvg from "../LineWithSvg";
import ColumnTable from "../../tableWithUser/ColumnTable";

interface IHeaderProps{
    header:string,
    icon: string
}

const ContainerHeader = ({header, icon}: IHeaderProps) => {
    return (
        <div className={"ContainerHeader"}>
            <LineWithSvg icon={icon} line={header} isHeader={true} iconWidth={28} iconHeight={21}/>
        </div>
    );
};

export default ContainerHeader;