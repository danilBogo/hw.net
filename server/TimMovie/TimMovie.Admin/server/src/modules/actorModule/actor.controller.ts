import {Body, Controller, Delete, Get, Param, Post, Query, UploadedFile, UseInterceptors} from '@nestjs/common';
import {ActorService} from "./ActorService";
import NameDto from "../../dto/NameDto";
import {Result} from "../../dto/Result";
import {FileInterceptor} from "@nestjs/platform-express";
import {PersonDto} from "../../dto/PersonDto";
import {Admin} from "../authModule/adminAuth";


@Admin()
@Controller('actors')
export class ActorController {
    constructor(private readonly actorService: ActorService) {
    }
    
    @Get("collection")
    async getActorsByNamePart(
        @Query("namePart") namePart: string,
        @Query("skip") skip:number,
        @Query("take") take: number): Promise<NameDto[]>{
        return await this.actorService.getActorsByNamePart(namePart, skip, take);
    }

    @Post('add')
    @UseInterceptors(FileInterceptor('img'))
    async addNewActor(@Body() newActor: PersonDto, @UploadedFile() image: Express.Multer.File): Promise<Result<string>>{
        return await this.actorService.addActor(newActor,image)
    }

    @Delete(':id')
    async deleteActor(@Param ('id') id: string): Promise<Result<string>>{
        return await this.actorService.deleteActor(id)
    }
}
