import {Column, Entity, Index, JoinColumn, ManyToOne, PrimaryGeneratedColumn} from "typeorm";
import { Subscribe } from "./Subscribe";
import { AspNetUser } from "./AspNetUser";

@Index("PK_UserSubscribes", ["id"], { unique: true })
@Index("IX_UserSubscribes_SubscribeId", ["subscribeId"], {})
@Index("IX_UserSubscribes_SubscribedUserId", ["subscribedUserId"], {})
@Entity("UserSubscribes", { schema: "public" })
export class UserSubscribe {
  @PrimaryGeneratedColumn("uuid", {name: "Id"})
  id: string;

  @Column("uuid", { name: "SubscribedUserId" })
  subscribedUserId: string;

  @Column("uuid", { name: "SubscribeId" })
  subscribeId: string;

  @Column("timestamp with time zone", { name: "StartDay" })
  startDay: Date;

  @Column("timestamp with time zone", { name: "EndDate" })
  endDate: Date;

  @ManyToOne(() => Subscribe, (subscribes) => subscribes.userSubscribes, {
    onDelete: "CASCADE",
  })
  @JoinColumn([{ name: "SubscribeId", referencedColumnName: "id" }])
  subscribe: Subscribe;

  @ManyToOne(() => AspNetUser, (aspNetUsers) => aspNetUsers.userSubscribes, {
    onDelete: "CASCADE",
  })
  @JoinColumn([{ name: "SubscribedUserId", referencedColumnName: "id" }])
  subscribedUser: AspNetUser;
}
