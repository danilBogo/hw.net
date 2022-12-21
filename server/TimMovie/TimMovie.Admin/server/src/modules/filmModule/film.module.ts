import { Module } from '@nestjs/common';
import { FilmController } from './film.controller';
import {FilmService} from "./FilmService";
import {FileService} from "../FileService";
import {ProducerModule} from "../producerModule/producer.module";
import {GenreModule} from "../genreModule/genre.module";
import {CountryModule} from "../countryModule/countryModule";
import {ActorModule} from "../actorModule/actor.module";

@Module({
  controllers: [FilmController],
  providers: [FilmService, FileService],
  imports: [ActorModule, ProducerModule, GenreModule, CountryModule],
  exports: [FilmService]
})
export class FilmModule {}
