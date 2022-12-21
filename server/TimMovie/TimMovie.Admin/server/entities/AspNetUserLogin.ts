import { Column, Entity, Index, JoinColumn, ManyToOne } from "typeorm";
import { AspNetUser } from "./AspNetUser";

@Index("PK_AspNetUserLogins", ["loginProvider", "providerKey"], {
  unique: true,
})
@Index("IX_AspNetUserLogins_UserId", ["userId"], {})
@Entity("AspNetUserLogins", { schema: "public" })
export class AspNetUserLogin {
  @Column("text", { primary: true, name: "LoginProvider" })
  loginProvider: string;

  @Column("text", { primary: true, name: "ProviderKey" })
  providerKey: string;

  @Column("text", { name: "ProviderDisplayName", nullable: true })
  providerDisplayName: string | null;

  @Column("uuid", { name: "UserId" })
  userId: string;

  @ManyToOne(() => AspNetUser, (aspNetUsers) => aspNetUsers.aspNetUserLogins, {
    onDelete: "CASCADE",
  })
  @JoinColumn([{ name: "UserId", referencedColumnName: "id" }])
  user: AspNetUser;
}
