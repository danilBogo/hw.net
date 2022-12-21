import { Column, Entity, Index, JoinColumn, ManyToOne } from "typeorm";
import { Comment } from "./Comment";
import { AspNetUser } from "./AspNetUser";

@Index("IX_CommentReports_CommentId", ["commentId"], {})
@Index("PK_CommentReports", ["id"], { unique: true })
@Index("IX_CommentReports_UserId", ["userId"], {})
@Entity("CommentReports", { schema: "public" })
export class CommentReport {
  @Column("uuid", { primary: true, name: "Id" })
  id: string;

  @Column("uuid", { name: "CommentId" })
  commentId: string;

  @Column("uuid", { name: "UserId" })
  userId: string;

  @ManyToOne(() => Comment, (comments) => comments.commentReports, {
    onDelete: "CASCADE",
  })
  @JoinColumn([{ name: "CommentId", referencedColumnName: "id" }])
  comment: Comment;

  @ManyToOne(() => AspNetUser, (aspNetUsers) => aspNetUsers.commentReports, {
    onDelete: "CASCADE",
  })
  @JoinColumn([{ name: "UserId", referencedColumnName: "id" }])
  user: AspNetUser;
}
