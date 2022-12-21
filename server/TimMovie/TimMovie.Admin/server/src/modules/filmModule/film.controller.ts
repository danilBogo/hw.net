import {Body, Controller, Delete, Get, Param, Post, Query, UploadedFile, UseInterceptors} from '@nestjs/common';
import {FilmService} from "./FilmService";
import {FilmDto} from "../../dto/FilmDto";
import {FileInterceptor} from "@nestjs/platform-express";
import {Result} from "../../dto/Result";
import PaginationLoading from "../../dto/PaginationLoading";
import FilmForTableDto from "../../dto/FilmForTableDto";
import NameDto from "../../dto/NameDto";
import {getRepository} from "typeorm";
import {Film} from "../../../entities/Film";
import FullInfoAboutFilmDto from "../../dto/FullInfoAboutFilmDto";
import {Admin} from "../authModule/adminAuth";

@Admin()
@Controller('films')
export class FilmController {
    constructor(private readonly filmService: FilmService) {
    }
    
    @Post("add")
    @UseInterceptors(FileInterceptor('img'))
    async addNewFilm(@Body() newFilmInfo: FilmDto, @UploadedFile() image: Express.Multer.File): Promise<Result<string>>{
        if (image == null){
            return {
                success: false,
                textError: "Необходимо добавить обложку для фильма",
            }     
        }
        
        return await this.filmService.addNewFilm(newFilmInfo, image);
    }

    @Get("pagination")
    async getFilmByNamePart(@Query() pagination: PaginationLoading): Promise<FilmForTableDto[]> {
        return await this.filmService.getFilmByNamePart(pagination);
    }
    
    @Get("collection")
    async getFilmsTitlesByNamePart(
        @Query("namePart") namePart: string,
        @Query("skip") skip:number,
        @Query("take") take: number): Promise<NameDto[]>{
        return await this.filmService.getFilmsTitlesByNamePart(namePart, skip, take);
    }

    @Get(":id")
    async getFilmById(@Param("id") id: string): Promise<Result<FullInfoAboutFilmDto>>{
        let film = await this.filmService.getFilmById(id);
        
        if (film == null) {
            return {
                success: false,
                textError: `Фильма с id: ${id} не существует`
            };
        }
        
        return {
            success: true,
            result: film
        }
    }
    
    @Delete(":id")
    async deleteFilmById(@Param("id") id: string): Promise<Result<string>>{
        return await this.filmService.deleteFilmById(id);
    }

    @Post(":id")
    @UseInterceptors(FileInterceptor('img'))
    async updateFilm(@Param("id") id: string, @Body() updatedFilm: FilmDto, @UploadedFile() image?: Express.Multer.File): Promise<Result<string>>{
        return await this.filmService.updateFilm(id, updatedFilm, image);
    }
}
