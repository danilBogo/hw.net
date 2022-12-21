import { Column, Entity, Index, JoinColumn, ManyToOne } from "typeorm";
import { Film } from "./Film";
import { AspNetUser } from "./AspNetUser";

@Index("IX_WatchedFilms_FilmId", ["filmId"], {})
@Index("PK_WatchedFilms", ["id"], { unique: true })
@Index("IX_WatchedFilms_WatchedUserId", ["watchedUserId"], {})
@Entity("WatchedFilms", { schema: "public" })
export class WatchedFilm {
  @Column("uuid", { primary: true, name: "Id" })
  id: string;

  @Column("uuid", { name: "WatchedUserId" })
  watchedUserId: string;

  @Column("uuid", { name: "FilmId" })
  filmId: string;

  @Column("integer", { name: "Grade", nullable: true })
  grade: number | null;

  @Column("timestamp with time zone", { name: "Date" })
  date: Date;

  @ManyToOne(() => Film, (films) => films.watchedFilms, {
    onDelete: "CASCADE",
  })
  @JoinColumn([{ name: "FilmId", referencedColumnName: "id" }])
  film: Film;

  @ManyToOne(() => AspNetUser, (aspNetUsers) => aspNetUsers.watchedFilms, {
    onDelete: "CASCADE",
  })
  @JoinColumn([{ name: "WatchedUserId", referencedColumnName: "id" }])
  watchedUser: AspNetUser;
}
