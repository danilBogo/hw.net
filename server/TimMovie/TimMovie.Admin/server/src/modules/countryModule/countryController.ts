import {Controller, Get, Query} from '@nestjs/common';
import NameDto from "../../dto/NameDto";
import {CountryService} from "./country.service";
import {Admin} from "../authModule/adminAuth";

@Admin()
@Controller('countries')
export class CountryController {
    constructor(private readonly cityService: CountryService) {
    }

    @Get("collection")
    async getCountriesByNamePart(
        @Query("namePart") namePart: string,
        @Query("skip") skip:number,
        @Query("take") take: number): Promise<NameDto[]>{
        return await this.cityService.getCountriesByNamePart(namePart, skip, take);
    }
}
