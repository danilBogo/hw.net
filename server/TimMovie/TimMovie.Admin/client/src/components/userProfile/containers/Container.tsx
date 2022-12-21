import React, {useEffect, useState} from 'react';
import LineWithSvg from "../LineWithSvg";
import $api from "../../../http";
import NameDto from "../../../dto/NameDto";
import {AxiosResponseValidator} from "../../../common/AxiosResponseValidator";

interface IContainerProps{
    readonly urlForLoadAllInfo: string;
    readonly namesIncludedInUser: Set<string>;
    readonly iconName: string;
    readonly isEdit: boolean;
}

const Container = ({namesIncludedInUser, isEdit, ...props} :IContainerProps) => {
    const [infoAbout, setInfoAbout] = useState<NameDto[]>();
    const [namesIsChanged, setNamesIsChanged] = useState(false);
    
    useEffect(() => {
        $api.get<NameDto[]>(props.urlForLoadAllInfo)
            .then(response => {
                if (!AxiosResponseValidator.checkSuccessResponseStatusAndLogIfError(response)){
                    return;
                }
                
                setInfoAbout(response.data);
            })
    }, []);

    return (
        <div>
            {
                infoAbout?.map(item => 
                    <div key={item.id}>
                        {(namesIncludedInUser.has(item.name) || isEdit) &&
                            <LineWithSvg line={item.name} isEdit={isEdit} icon={props.iconName} 
                            onAdd={() => {
                              namesIncludedInUser.add(item.name);
                              setNamesIsChanged(!namesIsChanged);
                            }}
                            onRemove={() => {
                                namesIncludedInUser.delete(item.name);
                                setNamesIsChanged(!namesIsChanged);
                            }}
                            isAdded={namesIncludedInUser.has(item.name)}/>
                        }
                    </div>
                )
            }
        </div>
    );
};

export default Container;