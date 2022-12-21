import SubscribeInfoDto from "./SubscribeInfoDto";

export default interface SubscribeAllInfoDto extends SubscribeInfoDto {
    readonly description: string;
}