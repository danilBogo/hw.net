import {
  Column,
  Entity,
  Index,
  JoinColumn,
  ManyToOne,
  OneToMany,
} from "typeorm";
import { CommentReport } from "./CommentReport";
import { AspNetUser } from "./AspNetUser";
import { Film } from "./Film";

@Index("IX_Comments_AuthorId", ["authorId"], {})
@Index("IX_Comments_FilmId", ["filmId"], {})
@Index("PK_Comments", ["id"], { unique: true })
@Entity("Comments", { schema: "public" })
export class Comment {
  @Column("uuid", { primary: true, name: "Id" })
  id: string;

  @Column("uuid", { name: "FilmId" })
  filmId: string;

  @Column("uuid", { name: "AuthorId" })
  authorId: string;

  @Column("text", { name: "Content" })
  content: string;

  @Column("timestamp with time zone", { name: "Date" })
  date: Date;

  @OneToMany(() => CommentReport, (commentReports) => commentReports.comment)
  commentReports: CommentReport[];

  @ManyToOne(() => AspNetUser, (aspNetUsers) => aspNetUsers.comments, {
    onDelete: "CASCADE",
  })
  @JoinColumn([{ name: "AuthorId", referencedColumnName: "id" }])
  author: AspNetUser;

  @ManyToOne(() => Film, (films) => films.comments, { onDelete: "CASCADE" })
  @JoinColumn([{ name: "FilmId", referencedColumnName: "id" }])
  film: Film;
}
