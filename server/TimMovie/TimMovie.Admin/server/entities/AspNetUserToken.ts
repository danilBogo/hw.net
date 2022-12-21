import { Column, Entity, Index, JoinColumn, ManyToOne } from "typeorm";
import { AspNetUser } from "./AspNetUser";

@Index("PK_AspNetUserTokens", ["loginProvider", "name", "userId"], {
  unique: true,
})
@Entity("AspNetUserTokens", { schema: "public" })
export class AspNetUserToken {
  @Column("uuid", { primary: true, name: "UserId" })
  userId: string;

  @Column("text", { primary: true, name: "LoginProvider" })
  loginProvider: string;

  @Column("text", { primary: true, name: "Name" })
  name: string;

  @Column("text", { name: "Value", nullable: true })
  value: string | null;

  @ManyToOne(() => AspNetUser, (aspNetUsers) => aspNetUsers.aspNetUserTokens, {
    onDelete: "CASCADE",
  })
  @JoinColumn([{ name: "UserId", referencedColumnName: "id" }])
  user: AspNetUser;
}
