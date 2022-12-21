import {Body, Controller, Delete, Get, Param, Post, Query} from '@nestjs/common';
import NameDto from "../../dto/NameDto";
import {Admin} from "../authModule/adminAuth";
import {GenreService} from "./GenreService";
import {Result} from "../../dto/Result";

@Admin()
@Controller('genres')
export class GenreController {
    constructor(private readonly genreService: GenreService) {
    }

    @Get("collection")
    async getGenresByNamePart(
        @Query("namePart") namePart: string,
        @Query("skip") skip:number,
        @Query("take") take: number): Promise<NameDto[]>{
        return await this.genreService.getGenresByNamePart(namePart, skip, take);
    }

    @Post("add")
    async addNewGenre(@Body("genreName") genreName: string): Promise<Result<string>>{
        return await this.genreService.addIfNotExist(genreName);
    }

    @Delete(":id")
    async deleteGenre(@Param("id") id: string): Promise<Result<string>>{
        return await this.genreService.deleteGenre(id);
    }

    @Post(":id")
    async updateGenre(@Param("id") id: string, @Body("name") name: string): Promise<Result<string>>{
        return await this.genreService.updateGenreName(id, name);
    }
}
