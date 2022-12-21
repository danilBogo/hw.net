import {Body, Controller, Get, Param, Post, Query, UploadedFile, UseInterceptors} from "@nestjs/common";
import {Admin} from "../authModule/adminAuth";
import {PersonDto} from "../../dto/PersonDto";
import {PersonService} from "./personService";
import PaginationLoading from "../../dto/PaginationLoading";
import {FileInterceptor} from "@nestjs/platform-express";
import {UpdateBannerDto} from "../../dto/UpdateBannerDto";
import {Result} from "../../dto/Result";

@Admin()
@Controller('person')
export class PersonController {

    constructor(private readonly personService: PersonService) {
    }

    @Get("collection")
    async getPersonsByNamePart(@Query() pagination: PaginationLoading): Promise<PersonDto[]> {
        return await this.personService.getPersonsByNamePart(pagination);
    }

    @Post('update/:id')
    @UseInterceptors(FileInterceptor('img'))
    async updatePerson(@Param("id") id: string, @Body() person: PersonDto, @UploadedFile() image: Express.Multer.File | null): Promise<Result<string>> {
        return await this.personService.updatePersonById(id, person, image)
    }
}