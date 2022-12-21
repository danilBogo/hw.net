export default interface SubscribeInfoDto {
    readonly id: string;
    readonly name: string;
    readonly price: number;
    readonly films: string[];
    readonly genres: string[];
    readonly isActive: boolean;
}