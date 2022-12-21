export default class FullInfoAboutFilmDto {
    title: string;
    description: string | null;
    countryName: string;
    filmLink: string;
    year: number;
    actorNames: string[] | null;
    producerNames: string[] | null;
    genreNames: string[];
    isFree: boolean;
    image: string;
}