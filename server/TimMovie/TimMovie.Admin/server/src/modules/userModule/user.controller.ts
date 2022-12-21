import {Body, Controller, Get, Post, Query} from '@nestjs/common';
import {ShortInformationAboutUserDto} from 'src/dto/ShortInformationAboutUserDto';
import {AllInformationAboutUserDto} from "../../dto/AllInformationAboutUserDto";
import {UserService} from "./UserService";
import {SubscribeService} from "../subscribesModule/SubscribeService";
import {Admin} from "../authModule/adminAuth";
import {RoleService} from "../roleModule/RoleService";
import {Result} from "../../dto/Result";
import {UserInfoDto} from "../../dto/UserInfoDto";

@Admin()
@Controller('users')
export class UserController{
    constructor(private readonly userService: UserService,
                private readonly subscribeService: SubscribeService,
                private readonly roleService: RoleService) {}
    
    @Get('collection')
    public async getUsersWithFilterByLogin(
            @Query("incomingText") incomingText: string, 
            @Query("skip") skip: number, 
            @Query("take") take: number): Promise<ShortInformationAboutUserDto[]> {
        console.log(`${skip} ${take} ${incomingText}`);
        
        const users = await this.userService.getUsersWithFilterByLogin(incomingText, skip, take);
        return users;
    }

    @Get('user')
    public async getAllInfoAboutUser(@Query('id') id: string): Promise<AllInformationAboutUserDto | null> {
        const user: AllInformationAboutUserDto = await this.userService.getAllInfoAboutUser(id);
        return user;
    }

    @Post("updateRolesAndSub")
    public async updateRolesAndSubscribes(@Body() userInfo: UserInfoDto): Promise<Result<string>>{
        console.log(userInfo);
        await this.subscribeService.updateSubscribeForUser(userInfo.userId, userInfo.subscribeNames);
        await this.roleService.updateRolesForUser(userInfo.userId, userInfo.roleNames);
        
        return {
            success: true
        }
    }

    @Get("isExist")
    public async userIsExisted(
        @Query("userId")userId: string): Promise<{isExisted}>{
        let isExisted = await this.userService.userIsExisted(userId);
        
        return {
            isExisted
        };
    }
}
