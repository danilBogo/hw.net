import { Module } from '@nestjs/common';
import {RoleController} from "./role.controller";
import {RoleService} from "./RoleService";

@Module({
    controllers: [RoleController],
    providers: [RoleService],
    exports: [RoleService]
})
export class RoleModule {}
