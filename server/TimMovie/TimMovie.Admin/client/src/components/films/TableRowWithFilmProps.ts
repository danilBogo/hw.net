import FilmForTableDto from "../../dto/FilmForTableDto";

export default interface TableRowWithFilmProps{
    readonly film: FilmForTableDto;
    readonly onDeleteFilm: (id: string) => void;
}