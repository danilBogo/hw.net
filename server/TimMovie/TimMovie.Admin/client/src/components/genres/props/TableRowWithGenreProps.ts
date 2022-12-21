import NameDto from "../../../dto/NameDto";

export default interface TableRowWithGenreProps {
    readonly genre: NameDto;
    readonly serialNumber: number;
    readonly deleteGenre: (id: string) => void;
    readonly editGenre: (id: string, name: string) => void;
}