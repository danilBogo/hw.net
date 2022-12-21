import {Module} from "@nestjs/common";
import {PersonService} from "./personService";
import {PersonController} from "./personController";
import {FileService} from "../FileService";

@Module({
    controllers: [PersonController],
    providers: [PersonService, FileService],
    exports: [PersonModule]
})
export class PersonModule {
}