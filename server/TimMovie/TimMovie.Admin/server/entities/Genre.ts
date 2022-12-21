import { Column, Entity, Index, JoinTable, ManyToMany } from "typeorm";
import { Film } from "./Film";
import { Subscribe } from "./Subscribe";

@Index("PK_Genres", ["id"], { unique: true })
@Entity("Genres", { schema: "public" })
export class Genre {
  @Column("uuid", { primary: true, name: "Id" })
  id: string;

  @Column("character varying", { name: "Name", length: 50 })
  name: string;

  @ManyToMany(() => Film, (films) => films.genres)
  films: Film[];

  @ManyToMany(() => Subscribe, (subscribes) => subscribes.genres)
  @JoinTable({
    name: "GenreSubscribe",
    joinColumns: [{ name: "GenresId", referencedColumnName: "id" }],
    inverseJoinColumns: [{ name: "SubscribesId", referencedColumnName: "id" }],
    schema: "public",
  })
  subscribes: Subscribe[];
}