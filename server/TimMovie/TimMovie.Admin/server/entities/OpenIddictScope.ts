import { Column, Entity, Index } from "typeorm";

@Index("PK_OpenIddictScopes", ["id"], { unique: true })
@Index("IX_OpenIddictScopes_Name", ["name"], { unique: true })
@Entity("OpenIddictScopes", { schema: "public" })
export class OpenIddictScope {
  @Column("text", { primary: true, name: "Id" })
  id: string;

  @Column("character varying", {
    name: "ConcurrencyToken",
    nullable: true,
    length: 50,
  })
  concurrencyToken: string | null;

  @Column("text", { name: "Description", nullable: true })
  description: string | null;

  @Column("text", { name: "Descriptions", nullable: true })
  descriptions: string | null;

  @Column("text", { name: "DisplayName", nullable: true })
  displayName: string | null;

  @Column("text", { name: "DisplayNames", nullable: true })
  displayNames: string | null;

  @Column("character varying", { name: "Name", nullable: true, length: 200 })
  name: string | null;

  @Column("text", { name: "Properties", nullable: true })
  properties: string | null;

  @Column("text", { name: "Resources", nullable: true })
  resources: string | null;
}
