import { Module } from '@nestjs/common';
import {UserService} from "./UserService";
import {SubscribeModule} from "../subscribesModule/subscribe.module";
import {UserController} from "./user.controller";
import {RoleModule} from "../roleModule/role.module";

@Module({
    imports: [SubscribeModule, RoleModule],
    controllers: [UserController],
    providers: [UserService]
})
export class UserModule {}
