import React, {useContext} from 'react';
import ReactDOM from 'react-dom';
import Icon from "../../svg-icons/Icon";
import {ClickHook} from "./ClickHook/ClickHook"

interface EditButtonProps {
    change: ClickHook;
    onSave: () => void;
    onCancel: () => void;
}

const EditButtons: React.FC<EditButtonProps> = ({change, ...props}:EditButtonProps) => {
    return (
        <div className="row">
            {!change.clickState &&
                <div className="col-md-2 col-6">
                    <button className="btns" id="edit_btn"
                            onClick={() => change.setClickState(!change.clickState)}>Изменить
                    </button>
                </div>
            }
            {change.clickState &&  
                <>
                    <div className="col-md-2 col-6">
                        <button className="btns" onClick={props.onSave} id="save_btn">Сохранить</button>
                    </div>
                    <div className="col-md-2 col-6">
                        <button className="btns" onClick={props.onCancel} id="save_btn">Отмена</button>
                    </div>
                </>
            }
        </div>
    );
};

export default EditButtons;