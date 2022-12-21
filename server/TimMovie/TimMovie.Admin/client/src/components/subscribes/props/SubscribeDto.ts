export default interface SubscribeDto {
    readonly name: string;
    readonly price: number;
    readonly description: string;
    readonly isActive: boolean;
    readonly films: string[];
    readonly genres: string[];
}