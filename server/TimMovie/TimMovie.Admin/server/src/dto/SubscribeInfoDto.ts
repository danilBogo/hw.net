export default class SubscribeInfoDto {
    id: string;
    name: string;
    isActive: boolean;
    price: number | null;
    films: string[];
    genres: string[];
}