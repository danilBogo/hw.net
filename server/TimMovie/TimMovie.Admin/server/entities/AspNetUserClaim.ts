import {
  Column,
  Entity,
  Index,
  JoinColumn,
  ManyToOne,
  PrimaryGeneratedColumn,
} from "typeorm";
import { AspNetUser } from "./AspNetUser";

@Index("PK_AspNetUserClaims", ["id"], { unique: true })
@Index("IX_AspNetUserClaims_UserId", ["userId"], {})
@Entity("AspNetUserClaims", { schema: "public" })
export class AspNetUserClaim {
  @PrimaryGeneratedColumn({ type: "integer", name: "Id" })
  id: number;

  @Column("uuid", { name: "UserId" })
  userId: string;

  @Column("text", { name: "ClaimType", nullable: true })
  claimType: string | null;

  @Column("text", { name: "ClaimValue", nullable: true })
  claimValue: string | null;

  @ManyToOne(() => AspNetUser, (aspNetUsers) => aspNetUsers.aspNetUserClaims, {
    onDelete: "CASCADE",
  })
  @JoinColumn([{ name: "UserId", referencedColumnName: "id" }])
  user: AspNetUser;
}
