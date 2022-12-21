import {Body, Controller, Delete, Get, Param, Post, Query, UploadedFile, UseInterceptors} from '@nestjs/common';
import NameDto from "../../dto/NameDto";
import {ProducerService} from "./ProducerService";
import {FileInterceptor} from "@nestjs/platform-express";
import {PersonDto} from "../../dto/PersonDto";
import {Result} from "../../dto/Result";
import {Admin} from "../authModule/adminAuth";


@Admin()
@Controller('producers')
export class ProducerController {
    constructor(private readonly producerService: ProducerService) {
    }

    @Get("collection")
    async getProducersByNamePart(
        @Query("namePart") namePart: string,
        @Query("skip") skip: number,
        @Query("take") take: number): Promise<NameDto[]> {
        return await this.producerService.getActorsByNamePart(namePart, skip, take);
    }

    @Post('add')
    @UseInterceptors(FileInterceptor('img'))
    async addNewActor(@Body() newProducer: PersonDto, @UploadedFile() image: Express.Multer.File): Promise<Result<string>> {
        return await this.producerService.addProducer(newProducer, image)
    }

    @Delete(':id')
    async deleteActor(@Param ('id') id: string): Promise<Result<string>>{
        return await this.producerService.deleteProducer(id)
    }
}
