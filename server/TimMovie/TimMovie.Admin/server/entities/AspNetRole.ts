import {
  Column,
  Entity,
  Index,
  JoinTable,
  ManyToMany,
  OneToMany,
} from "typeorm";
import { AspNetRoleClaim } from "./AspNetRoleClaim";
import { AspNetUser } from "./AspNetUser";

@Index("PK_AspNetRoles", ["id"], { unique: true })
@Index("RoleNameIndex", ["normalizedName"], { unique: true })
@Entity("AspNetRoles", { schema: "public" })
export class AspNetRole {
  @Column("uuid", { primary: true, name: "Id" })
  id: string;

  @Column("character varying", { name: "Name", nullable: true, length: 256 })
  name: string | null;

  @Column("character varying", {
    name: "NormalizedName",
    nullable: true,
    length: 256,
  })
  normalizedName: string | null;

  @Column("text", { name: "ConcurrencyStamp", nullable: true })
  concurrencyStamp: string | null;

  @OneToMany(
    () => AspNetRoleClaim,
    (aspNetRoleClaims) => aspNetRoleClaims.role
  )
  aspNetRoleClaims: AspNetRoleClaim[];

  @ManyToMany(() => AspNetUser, (aspNetUsers) => aspNetUsers.aspNetRoles)
  @JoinTable({
    name: "AspNetUserRoles",
    joinColumns: [{ name: "RoleId", referencedColumnName: "id" }],
    inverseJoinColumns: [{ name: "UserId", referencedColumnName: "id" }],
    schema: "public",
  })
  aspNetUsers: AspNetUser[];
}
