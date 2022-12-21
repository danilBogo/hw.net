import {Injectable} from "@nestjs/common";
import {FilmDto} from "../../dto/FilmDto";
import {Result} from "../../dto/Result";
import {FileService} from "../FileService";
import {getRepository} from "typeorm";
import {Film} from "../../../entities/Film";
import {Guid} from "guid-typescript";
import {plainToInstance} from "class-transformer";
import {ActorService} from "../actorModule/ActorService";
import {ProducerService} from "../producerModule/ProducerService";
import {CountryService} from "../countryModule/country.service";
import PaginationLoading from "../../dto/PaginationLoading";
import FilmForTableDto from "../../dto/FilmForTableDto";
import {includeNamePart} from "../../common/queryFunction";
import NameDto from "../../dto/NameDto";
import FullInfoAboutFilmDto from "../../dto/FullInfoAboutFilmDto";
import {GenreService} from "../genreModule/GenreService";


@Injectable()
export class FilmService {
    constructor(private readonly fileService: FileService,
                private readonly actorService: ActorService,
                private readonly producerService: ProducerService,
                private readonly genreService: GenreService,
                private readonly countryService: CountryService) {
    }

    async addNewFilm(newFilm: FilmDto, image: Express.Multer.File): Promise<Result<string>> {
        let resultSaveImage = await this.fileService.saveFilmImage(image);

        if (!resultSaveImage.success) {
            return {
                success: false,
                textError: "Во время сохранения  произошла ошибка",
            }
        }

        let filmRepo = getRepository(Film);
        let film = filmRepo.create(plainToInstance(Film, newFilm));

        film.id = Guid.create().toString();
        film.image = resultSaveImage.result;

        if (newFilm.actorNames != null) {

            let actors = await this.actorService.getActorsByFullName(newFilm.actorNames);
            film.actors = actors;
        }

        if (newFilm.producerNames != null) {
            let producers = await this.producerService.getProducersByFullName(newFilm.producerNames);
            film.producers = producers;
        }

        if (newFilm.genreNames != null) {
            let genres = await this.genreService.getGenresByFullName(newFilm.genreNames);
            film.genres = genres;
        }

        let country = await this.countryService.getCountryFullName(newFilm.countryName);
        film.country = country;

        await filmRepo.save(film);

        return {
            success: true,
        };
    }
    
    async getFilmByNamePart(pagination: PaginationLoading): Promise<FilmForTableDto[]> {
        let films = await getRepository(Film)
            .find({
                where: {
                    title: includeNamePart(pagination.namePart)
                },
                relations: ["country", "genres", "actors", "producers"],
                take: pagination.take,
                skip: pagination.skip
            });

        let filmsDto = films
            .map(film => {
                film.image = process.env.FILE_SERVICE_URL + film.image;
                let dto = plainToInstance(FilmForTableDto, film);
                dto.countryName = film.country.name;
                dto.genreNames = film.genres.map(value => value.name);
                dto.actorsAndProducers = film.producers.map(producer => `${producer.name} ${producer.surname}`)
                    .concat(film.actors.map(actor => `${actor.name} ${actor.surname}`))
                return dto;
            });

        return filmsDto;
    }

    async getFilmsTitlesByNamePart(namePart: string, skip: number, take: number): Promise<NameDto[]> {
        if (namePart == null) {
            return [];
        }

        let films = await getRepository(Film)
            .find({
                where: [
                    {title: includeNamePart(namePart)}
                ], take, skip,
                order: {title: 'ASC'}
            });

        return films.map(film => {
            return {
                name: `${film.title}`,
                id: `${film.id}`
            }
        });
    }
    
    async deleteFilmById(id: string): Promise<Result<string>>{
        let result = await getRepository(Film)
            .delete({
                id: id
            });

        if (result.affected == 0){
            return {
                success:false,
                textError:`Фильм с id ${id} не найден`
            }
        }

        return {
            success:true,
        }
    }
    
    async getFilmById(id: string): Promise<FullInfoAboutFilmDto | null>{
        let film = await getRepository(Film)
            .findOne({
                where: {
                    id: id,
                },
                relations: ["country", "genres", "actors", "producers"],
            });
        console.log(film);
        
        if (!film){
            return null;
        }

        film.image = process.env.FILE_SERVICE_URL + film.image;
        let filmsDto = plainToInstance(FullInfoAboutFilmDto, film);
        filmsDto.countryName = film.country.name;
        filmsDto.genreNames = film.genres.map(value => value.name);
        filmsDto.actorNames = film.actors.map(actor => `${actor.name} ${actor.surname}`);
        filmsDto.producerNames = film.producers.map(producer => `${producer.name} ${producer.surname}`);
        
        return filmsDto;
    }

    async updateFilm(id: string, updatedFilm: FilmDto, image?: Express.Multer.File): Promise<Result<string>> {
        let film = plainToInstance(Film, updatedFilm);
        film.id = id;
        
        if (!!image){
            let resultSaveImage = await this.fileService.saveImage(image, "film");

            if (!resultSaveImage.success) {
                return {
                    success: false,
                    textError: "Во время сохранения обложки фильма произошла ошибка",
                }
            }
            
            film.image = resultSaveImage.result;
        }


        film.actors = [];
        if (updatedFilm.actorNames != null) {

            let actors = await this.actorService.getActorsByFullName(updatedFilm.actorNames);
            film.actors = actors;
        }

        film.producers = [];
        if (updatedFilm.producerNames != null) {
            let producers = await this.producerService.getProducersByFullName(updatedFilm.producerNames);
            film.producers = producers;
        }

        if (updatedFilm.genreNames != null) {
            let genres = await this.genreService.getGenresByFullName(updatedFilm.genreNames);
            film.genres = genres;
        }

        let country = await this.countryService.getCountryFullName(updatedFilm.countryName);
        film.country = country;
        
        await getRepository(Film)
            .save(film);

        return {
            success: true,
        };
    }
    
    
    async getFilmsByNames(names: string[]): Promise<Film[]>{
        return await getRepository(Film)
            .find({
                where: names.map(name => {
                    return {
                        title: name
                    }
                })
            })
    }
}