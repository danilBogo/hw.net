import SubscribeInfoDto from "../../../dto/SubscribeInfoDto";

export default interface TableRowWithSubscribeProps {
    readonly subscribe: SubscribeInfoDto;
    readonly onDeleteSubscribe: (id: string) => void;
}