import SubscribeDto from "./SubscribeDto";
import SubscribeAllInfoDto from "../../../dto/SubscribeAllInfoDto";

export default interface SubscribeFormProps {
    readonly saveSubscribe: (data: SubscribeDto) => Promise<boolean>;
    readonly initializationForm?: SubscribeAllInfoDto;
    readonly isEdit: boolean;
}