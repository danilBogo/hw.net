import {Body, Controller, Delete, Get, Param, Post, Query} from '@nestjs/common';
import {SubscribeService} from "./SubscribeService";
import {Admin} from "../authModule/adminAuth";
import NameDto from "../../dto/NameDto";
import {Result} from "../../dto/Result";
import SubscribeDto from "../../dto/SubscribeDto";
import SubscribeInfoDto from "../../dto/SubscribeInfoDto";
import ResultActionForUser from "../../dto/ResultActionForUser";
import SubscribeAllInfoDto from "../../dto/SubscribeAllInfoDto";

@Admin()
@Controller('subscribes')
export class SubscribeController {
    constructor(private readonly subscribeService: SubscribeService) {}
    
    @Get("collection")
    async getAllSubscribes(): Promise<NameDto[]>{
        return await this.subscribeService.getAllSubscribes();
    }
    
    @Post("add")
    async tryAddSubscribe(@Body() subscribe: SubscribeDto): Promise<Result<string>>{
        return await this.subscribeService.tryAddSubscribe(subscribe);
    }
    
    @Get("pagination")
    async getSubscribeInfoByNamePart(
        @Query("namePart") namePart, 
        @Query("skip") skip: number,
        @Query("take") take: number): Promise<SubscribeInfoDto[]>{
        return await this.subscribeService.getSubscribesInfoByNamePart(namePart, take, skip);
    }
    
    @Delete(":id")
    async tryDeleteSubscribeById(@Param("id") id: string): Promise<ResultActionForUser>{
        console.log(id);
        return await this.subscribeService.tryDeleteSubscribeById(id);
    }

    @Get("collection/:id")
    async tryGetAllInfoAboutSubscribe(@Param("id") id: string): Promise<Result<SubscribeAllInfoDto>>{
        return await this.subscribeService.tryGetAllInfoAboutSubscribe(id);
    }

    @Post("update/:id")
    async tryUpdateSubscribe(@Param("id") id: string, @Body() subscribe: SubscribeDto): Promise<ResultActionForUser>{
        return await this.subscribeService.tryUpdateSubscribe(id, subscribe);
    }
}
