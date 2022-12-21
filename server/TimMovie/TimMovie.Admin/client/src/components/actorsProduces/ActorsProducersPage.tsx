import AddActorsProducers from "./AddActorsProdcers";
import ActorsProducersTable from "./ActorsProducersTable";
import usePagination from "../../hook/dynamicLoading/usePagination";
import {PersonDto} from "../../dto/PersonDto";
import NamePart from "../../common/interfaces/NamePart";
import {useRef} from "react";

function ActorsProducersPage() {
    let urlQuery = useRef<NamePart>({namePart: ""});
    const personPagination = usePagination<PersonDto, NamePart>({pagination: 15, url: `/person/collection`, urlQuery})

    function update() {
        personPagination.reset()
        personPagination.setFetching(true)
    }

    return (
        <div className="mt-4">
            <div className="">
                <h1 className='text-center'>Добавить актера/режиссера</h1>
                <div className='mt-5'>
                    <AddActorsProducers update={update}/>
                </div>
                <div className='mt-5 mb-5'>
                    <hr/>
                </div>
                <ActorsProducersTable update={update} paginationResult={personPagination} urlQuery={urlQuery}/>
            </div>
        </div>
    );
}

export default ActorsProducersPage