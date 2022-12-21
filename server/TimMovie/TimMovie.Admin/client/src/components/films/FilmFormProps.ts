import FullInfoAboutFilmDto from "../../dto/FullInfoAboutFilmDto";

export default interface FilmFormProps {
    readonly isEdit: boolean;
    readonly trySaveFilm: (film: FormData) => Promise<boolean>;
    readonly formInitialization?: FullInfoAboutFilmDto;
}