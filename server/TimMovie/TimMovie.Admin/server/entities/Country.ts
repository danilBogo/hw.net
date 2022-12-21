import { Column, Entity, Index, OneToMany } from "typeorm";
import { AspNetUser } from "./AspNetUser";
import { Film } from "./Film";

@Index("PK_Countries", ["id"], { unique: true })
@Entity("Countries", { schema: "public" })
export class Country {
  @Column("uuid", { primary: true, name: "Id" })
  id: string;

  @Column("character varying", { name: "Name", length: 100 })
  name: string;

  @OneToMany(() => AspNetUser, (aspNetUsers) => aspNetUsers.country)
  aspNetUsers: AspNetUser[];

  @OneToMany(() => Film, (films) => films.country)
  films: Film[];
}
