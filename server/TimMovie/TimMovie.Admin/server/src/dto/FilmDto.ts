export class FilmDto {
    title: string;
    description: string | null;
    countryName: string | null;
    filmLink: string | null;
    year: number;
    actorNames: string[];
    producerNames: string[];
    genreNames: string[];
    isFree: boolean
}