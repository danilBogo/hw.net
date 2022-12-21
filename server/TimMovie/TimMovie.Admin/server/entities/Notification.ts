import { Column, Entity, Index, JoinTable, ManyToMany } from "typeorm";
import { AspNetUser } from "./AspNetUser";

@Index("PK_Notifications", ["id"], { unique: true })
@Entity("Notifications", { schema: "public" })
export class Notification {
  @Column("uuid", { primary: true, name: "Id" })
  id: string;

  @Column("text", { name: "Content" })
  content: string;

  @Column("timestamp with time zone", { name: "Date", nullable: true })
  date: Date | null;

  @ManyToMany(() => AspNetUser, (aspNetUsers) => aspNetUsers.notifications)
  @JoinTable({
    name: "NotificationUser",
    joinColumns: [{ name: "NotificationsId", referencedColumnName: "id" }],
    inverseJoinColumns: [{ name: "UsersId", referencedColumnName: "id" }],
    schema: "public",
  })
  aspNetUsers: AspNetUser[];
}
