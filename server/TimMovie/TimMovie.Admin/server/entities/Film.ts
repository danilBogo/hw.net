import {
  Column,
  Entity,
  Index,
  JoinColumn,
  JoinTable,
  ManyToMany,
  ManyToOne,
  OneToMany,
} from "typeorm";
import { Actor } from "./Actor";
import { AspNetUser } from "./AspNetUser";
import { Banner } from "./Banner";
import { Comment } from "./Comment";
import { Genre } from "./Genre";
import { Producer } from "./Producer";
import { Subscribe } from "./Subscribe";
import { Country } from "./Country";
import { WatchedFilm } from "./WatchedFilm";

@Index("IX_Films_CountryId", ["countryId"], {})
@Index("PK_Films", ["id"], { unique: true })
@Entity("Films", { schema: "public" })
export class Film {
  @Column("uuid", { primary: true, name: "Id" })
  id: string;

  @Column("character varying", { name: "Title", length: 200 })
  title: string;

  @Column("text", { name: "Description", nullable: true })
  description: string | null;

  @Column("uuid", { name: "CountryId", nullable: true })
  countryId: string | null;

  @Column("text", { name: "Image", nullable: true })
  image: string | null;

  @Column("text", { name: "FilmLink", nullable: true })
  filmLink: string | null;

  @Column("integer", { name: "Year", default: () => "0" })
  year: number;

  @Column("boolean", { name: "IsFree" })
  isFree: boolean;

  @ManyToMany(() => Actor, (actors) => actors.films)
  actors: Actor[];

  @OneToMany(() => AspNetUser, (aspNetUsers) => aspNetUsers.watchingFilm)
  aspNetUsers: AspNetUser[];

  @OneToMany(() => Banner, (banners) => banners.film)
  banners: Banner[];

  @OneToMany(() => Comment, (comments) => comments.film)
  comments: Comment[];

  @ManyToMany(() => Genre, (genres) => genres.films)
  @JoinTable({
    name: "FilmGenre",
    joinColumns: [{ name: "FilmsId", referencedColumnName: "id" }],
    inverseJoinColumns: [{ name: "GenresId", referencedColumnName: "id" }],
    schema: "public",
  })
  genres: Genre[];

  @ManyToMany(() => Producer, (producers) => producers.films)
  @JoinTable({
    name: "FilmProducer",
    joinColumns: [{ name: "FilmsId", referencedColumnName: "id" }],
    inverseJoinColumns: [{ name: "ProducersId", referencedColumnName: "id" }],
    schema: "public",
  })
  producers: Producer[];

  @ManyToMany(() => Subscribe, (subscribes) => subscribes.films)
  @JoinTable({
    name: "FilmSubscribe",
    joinColumns: [{ name: "FilmsId", referencedColumnName: "id" }],
    inverseJoinColumns: [{ name: "SubscribesId", referencedColumnName: "id" }],
    schema: "public",
  })
  subscribes: Subscribe[];

  @ManyToMany(() => AspNetUser, (aspNetUsers) => aspNetUsers.films)
  @JoinTable({
    name: "FilmUser",
    joinColumns: [{ name: "FilmsWatchLaterId", referencedColumnName: "id" }],
    inverseJoinColumns: [
      { name: "UsersWatchLaterId", referencedColumnName: "id" },
    ],
    schema: "public",
  })
  aspNetUsers2: AspNetUser[];

  @ManyToOne(() => Country, (countries) => countries.films, {
    onDelete: "SET NULL",
  })
  @JoinColumn([{ name: "CountryId", referencedColumnName: "id" }])
  country: Country;

  @OneToMany(() => WatchedFilm, (watchedFilms) => watchedFilms.film)
  watchedFilms: WatchedFilm[];
}
