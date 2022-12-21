import { Column, Entity, Index, JoinTable, ManyToMany } from "typeorm";
import { Film } from "./Film";

@Index("PK_Actors", ["id"], { unique: true })
@Entity("Actors", { schema: "public" })
export class Actor {
  @Column("uuid", { primary: true, name: "Id" })
  id: string;

  @Column("character varying", { name: "Name", length: 100 })
  name: string;

  @Column("character varying", { name: "Surname", nullable: true, length: 100 })
  surname: string | null;

  @Column("text", { name: "Photo", nullable: true })
  photo: string | null;

  @ManyToMany(() => Film, (films) => films.actors)
  @JoinTable({
    name: "ActorFilm",
    joinColumns: [{ name: "ActorsId", referencedColumnName: "id" }],
    inverseJoinColumns: [{ name: "FilmsId", referencedColumnName: "id" }],
    schema: "public",
  })
  films: Film[];
}
