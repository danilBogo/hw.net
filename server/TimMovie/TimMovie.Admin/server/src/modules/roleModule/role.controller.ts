import {Controller, Get, Query} from '@nestjs/common';
import {RoleService} from "./RoleService";
import {Admin} from "../authModule/adminAuth";
import NameDto from "../../dto/NameDto";

@Admin()
@Controller('roles')
export class RoleController {
    constructor(private readonly roleService: RoleService) {
    }
    
    @Get("collection")
    getAllRoles(): NameDto[]{
        return this.roleService.getAllRoles();
    }
}
