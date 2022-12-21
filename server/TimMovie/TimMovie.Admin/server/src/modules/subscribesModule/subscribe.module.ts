import { Module } from '@nestjs/common';
import {SubscribeController} from "./subscribe.controller";
import {SubscribeService} from "./SubscribeService";
import {FilmModule} from "../filmModule/film.module";
import {GenreModule} from "../genreModule/genre.module";

@Module({
    imports: [FilmModule, GenreModule],
    controllers: [SubscribeController],
    providers: [SubscribeService],
    exports: [SubscribeService]
})
export class SubscribeModule {}
