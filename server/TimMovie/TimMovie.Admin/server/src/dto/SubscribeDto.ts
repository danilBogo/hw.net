export default interface SubscribeDto {
    readonly name: string;
    readonly price: number;
    readonly description: string;
    readonly films: string[];
    readonly genres: string[];
    readonly isActive: boolean;
}