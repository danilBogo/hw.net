import { Module } from '@nestjs/common';
import {ProducerController} from "./producer.controller";
import {ProducerService} from "./ProducerService";
import {FileService} from "../FileService";

@Module({
    controllers: [ProducerController],
    providers: [ProducerService, FileService],
    exports: [ProducerService]
})
export class ProducerModule {}
