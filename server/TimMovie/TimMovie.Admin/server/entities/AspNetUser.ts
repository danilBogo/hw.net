import {
  Column,
  Entity,
  Index,
  JoinColumn,
  ManyToMany,
  ManyToOne,
  OneToMany,
} from "typeorm";
import { AspNetUserClaim } from "./AspNetUserClaim";
import { AspNetUserLogin } from "./AspNetUserLogin";
import { AspNetRole } from "./AspNetRole";
import { AspNetUserToken } from "./AspNetUserToken";
import { Country } from "./Country";
import { Film } from "./Film";
import { CommentReport } from "./CommentReport";
import { Comment } from "./Comment";
import { Notification } from "./Notification";
import { UserSubscribe } from "./UserSubscribe";
import { WatchedFilm } from "./WatchedFilm";

@Index("IX_AspNetUsers_CountryId", ["countryId"], {})
@Index("PK_AspNetUsers", ["id"], { unique: true })
@Index("EmailIndex", ["normalizedEmail"], {})
@Index("UserNameIndex", ["normalizedUserName"], { unique: true })
@Index("IX_AspNetUsers_WatchingFilmId", ["watchingFilmId"], {})
@Entity("AspNetUsers", { schema: "public" })
export class AspNetUser {
  @Column("uuid", { primary: true, name: "Id" })
  id: string;

  @Column("uuid", { name: "WatchingFilmId", nullable: true })
  watchingFilmId: string | null;

  @Column("uuid", { name: "CountryId", nullable: true })
  countryId: string | null;

  @Column("timestamp with time zone", { name: "RegistrationDate" })
  registrationDate: Date;

  @Column("character varying", {
    name: "UserName",
    nullable: true,
    length: 256,
  })
  userName: string | null;

  @Column("character varying", {
    name: "NormalizedUserName",
    nullable: true,
    length: 256,
  })
  normalizedUserName: string | null;

  @Column("character varying", { name: "Email", nullable: true, length: 256 })
  email: string | null;

  @Column("character varying", {
    name: "NormalizedEmail",
    nullable: true,
    length: 256,
  })
  normalizedEmail: string | null;

  @Column("boolean", { name: "EmailConfirmed" })
  emailConfirmed: boolean;

  @Column("text", { name: "PasswordHash", nullable: true })
  passwordHash: string | null;

  @Column("text", { name: "SecurityStamp", nullable: true })
  securityStamp: string | null;

  @Column("text", { name: "ConcurrencyStamp", nullable: true })
  concurrencyStamp: string | null;

  @Column("text", { name: "PhoneNumber", nullable: true })
  phoneNumber: string | null;

  @Column("boolean", { name: "PhoneNumberConfirmed" })
  phoneNumberConfirmed: boolean;

  @Column("boolean", { name: "TwoFactorEnabled" })
  twoFactorEnabled: boolean;

  @Column("timestamp with time zone", { name: "LockoutEnd", nullable: true })
  lockoutEnd: Date | null;

  @Column("boolean", { name: "LockoutEnabled" })
  lockoutEnabled: boolean;

  @Column("integer", { name: "AccessFailedCount" })
  accessFailedCount: number;

  @Column("date", { name: "BirthDate", default: () => "'-infinity'" })
  birthDate: string;

  @Column("character varying", {
    name: "DisplayName",
    length: 100,
    default: () => "''",
  })
  displayName: string;

  @OneToMany(
    () => AspNetUserClaim,
    (aspNetUserClaims) => aspNetUserClaims.user
  )
  aspNetUserClaims: AspNetUserClaim[];

  @OneToMany(
    () => AspNetUserLogin,
    (aspNetUserLogins) => aspNetUserLogins.user
  )
  aspNetUserLogins: AspNetUserLogin[];

  @ManyToMany(() => AspNetRole, (aspNetRoles) => aspNetRoles.aspNetUsers)
  aspNetRoles: AspNetRole[];

  @OneToMany(
    () => AspNetUserToken,
    (aspNetUserTokens) => aspNetUserTokens.user
  )
  aspNetUserTokens: AspNetUserToken[];

  @ManyToOne(() => Country, (countries) => countries.aspNetUsers, {
    onDelete: "SET NULL",
  })
  @JoinColumn([{ name: "CountryId", referencedColumnName: "id" }])
  country: Country;

  @ManyToOne(() => Film, (films) => films.aspNetUsers, {
    onDelete: "SET NULL",
  })
  @JoinColumn([{ name: "WatchingFilmId", referencedColumnName: "id" }])
  watchingFilm: Film;

  @OneToMany(() => CommentReport, (commentReports) => commentReports.user)
  commentReports: CommentReport[];

  @OneToMany(() => Comment, (comments) => comments.author)
  comments: Comment[];

  @ManyToMany(() => Film, (films) => films.aspNetUsers2)
  films: Film[];

  @ManyToMany(() => Notification, (notifications) => notifications.aspNetUsers)
  notifications: Notification[];

  @OneToMany(
    () => UserSubscribe,
    (userSubscribes) => userSubscribes.subscribedUser
  )
  userSubscribes: UserSubscribe[];

  @OneToMany(() => WatchedFilm, (watchedFilms) => watchedFilms.watchedUser)
  watchedFilms: WatchedFilm[];
}
