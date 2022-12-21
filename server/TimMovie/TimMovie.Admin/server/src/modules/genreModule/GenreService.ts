import {Injectable} from "@nestjs/common";
import NameDto from "../../dto/NameDto";
import {getRepository, Raw} from "typeorm";
import {includeNamePart} from "../../common/queryFunction";
import {Producer} from "../../../entities/Producer";
import {Genre} from "../../../entities/Genre";
import {Result} from "../../dto/Result";
import {Guid} from "guid-typescript";

@Injectable()
export class GenreService{
    async getGenresByNamePart(namePart: string, skip: number, take: number): Promise<NameDto[]>{
        if (namePart == null){
            return [];
        }

        let genres = await getRepository(Genre)
            .find({
                where: [
                    {name: includeNamePart(namePart)}
                ],
                take,
                skip,
                order: {
                    name: 'ASC',
                }
            });

        let genresDto = genres.map(genre => {
            return {
                id: genre.id,
                name: genre.name
            }
        });
        return genresDto;
    }

    async getGenresByFullName(genreNames: string[]): Promise<Genre[]>{
        return await getRepository(Genre)
            .find({
                where: genreNames.map(value => {
                    return {
                        name: value
                    }
                })
            });
    }

    async addIfNotExist(genreName: string): Promise<Result<string>>{
        if (await this.genreIsExisted(genreName)){
            return {
                success: false,
                textError: "Жанр с таким именем уже существует."
            }
        }
        
        await getRepository(Genre).insert({
                id: Guid.create().toString(),
                name: genreName
            });
        
        return {
            success: true
        }
    }
    
    async updateGenreName(id: string, genreName: string){
        if (await this.genreIsExisted(genreName)){
            return {
                success: false,
                textError: "Жанр с таким именем уже существует."
            }
        }

        await getRepository(Genre).update(id, {
            name: genreName
        });

        return {
            success: true
        }
    }
    
    async genreIsExisted(genreName: string): Promise<boolean>{
        let lowerGenreName = genreName.toLowerCase();
        
        return !!(await getRepository(Genre)
            .findOne({
                where: {
                    name: Raw(alias => `lower(${alias}) = '${lowerGenreName}'`)
                }}));
    }
    
    async deleteGenre(genreId: string): Promise<Result<string>>{
        let result = await getRepository(Genre)
            .delete({
                id: genreId
            });
        
        if (result.affected == 0){
            return {
                success: false,
                textError: "Удаляемый фильм не найден."
            }
        }
        
        return {
            success: true
        }
    }
}