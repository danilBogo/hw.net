import React from 'react';
import classes from './userProfile.module.css'
import './userProfile.css';
import Icon from "../../svg-icons/Icon";

interface LineProps {
    icon: string;
    line: string | null | undefined;
    onAdd?: () => void;
    onRemove?: () => void;
    isAdded?: boolean;
    isEdit?: boolean;
    title?: string;
    isBold?: boolean;
    isHeader?: boolean;
    iconWidth?: number;
    iconHeight?: number;
}

const LineWithSvg: React.FC<LineProps> = ({isEdit = false, iconWidth = 22, ...props}: LineProps) => {

    return (
        <div title={props.title} className={`${classes.line} d-flex align-items-center`}>
            <div className={classes.circleForIcon}>
                <Icon name={props.icon} width={iconWidth} height={props.iconHeight}/>
            </div>
            <span className={classes.content_str}
                  style={{
                      fontWeight: props.isBold ? 'bold' : 'normal',
                      textDecoration: props.isHeader ? 'underline' : 'normal',
                      textDecorationThickness: '2px',
                      textUnderlineOffset: "6px"
                  }}>
                    {props.line}
            </span>

            {isEdit &&
                    <div className="ms-auto ">
                        <div hidden={props.isAdded} onClick={props.onAdd} className="add_remove_btns add_btn">
                            <Icon height={iconWidth} name={"Add"}/>
                        </div>
                        <div hidden={!props.isAdded} onClick={props.onRemove} className="add_remove_btns remove_btn">
                            <Icon height={iconWidth} name={"Remove"}/>
                        </div>
                    </div>}
        </div>
    );
};

export default LineWithSvg;