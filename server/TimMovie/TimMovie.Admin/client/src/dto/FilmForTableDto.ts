export default interface FilmForTableDto{
    readonly id: string;
    readonly title: string;
    readonly image: string;
    readonly countryName: string;
    readonly year: number;
    readonly genreNames: string[];
    actorsAndProducers: string[];
} 