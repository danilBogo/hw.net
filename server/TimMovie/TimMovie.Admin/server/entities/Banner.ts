import { Column, Entity, Index, JoinColumn, ManyToOne } from "typeorm";
import { Film } from "./Film";

@Index("IX_Banners_FilmId", ["filmId"], {})
@Index("PK_Banners", ["id"], { unique: true })
@Entity("Banners", { schema: "public" })
export class Banner {
  @Column("uuid", { primary: true, name: "Id" })
  id: string;

  @Column("text", { name: "Description", nullable: true })
  description: string | null;

  @Column("text", { name: "Image" })
  image: string;

  @Column("uuid", { name: "FilmId" })
  filmId: string;

  @ManyToOne(() => Film, (films) => films.banners, { onDelete: "CASCADE" })
  @JoinColumn([{ name: "FilmId", referencedColumnName: "id" }])
  film: Film;
}
