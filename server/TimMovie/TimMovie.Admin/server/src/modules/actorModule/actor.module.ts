import {Module} from '@nestjs/common';
import {ActorController} from "./actor.controller";
import {ActorService} from "./ActorService";
import {FileService} from "../FileService";

@Module({
    controllers: [ActorController],
    providers: [ActorService, FileService],
    exports: [ActorService]
})
export class ActorModule {
}
