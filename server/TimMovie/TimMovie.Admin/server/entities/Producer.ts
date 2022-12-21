import { Column, Entity, Index, ManyToMany } from "typeorm";
import { Film } from "./Film";

@Index("PK_Producers", ["id"], { unique: true })
@Entity("Producers", { schema: "public" })
export class Producer {
  @Column("uuid", { primary: true, name: "Id" })
  id: string;

  @Column("character varying", { name: "Name", length: 100 })
  name: string;

  @Column("character varying", { name: "Surname", nullable: true, length: 100 })
  surname: string | null;

  @Column("text", { name: "Photo", nullable: true })
  photo: string | null;

  @ManyToMany(() => Film, (films) => films.producers)
  films: Film[];
}
