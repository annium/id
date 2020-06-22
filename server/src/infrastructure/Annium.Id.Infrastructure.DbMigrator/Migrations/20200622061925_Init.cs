using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Annium.Id.Infrastructure.DbMigrator.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    login = table.Column<string>(nullable: false),
                    password_hash = table.Column<string>(nullable: false),
                    email = table.Column<string>(nullable: false),
                    referral_id = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_users_referral_id",
                        column: x => x.referral_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "apps",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    owner_id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    api_token = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_apps", x => x.id);
                    table.ForeignKey(
                        name: "fk_apps_users_owner_id",
                        column: x => x.owner_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    owner_id = table.Column<Guid>(nullable: false),
                    parent_id = table.Column<Guid>(nullable: true),
                    name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_companies", x => x.id);
                    table.ForeignKey(
                        name: "fk_companies_users_owner_id",
                        column: x => x.owner_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_companies_companies_parent_id",
                        column: x => x.parent_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "claims",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    app_id = table.Column<Guid>(nullable: false),
                    key = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_claims_apps_app_id",
                        column: x => x.app_id,
                        principalTable: "apps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "company_claims",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    app_id = table.Column<Guid>(nullable: false),
                    key = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_company_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_company_claims_apps_app_id",
                        column: x => x.app_id,
                        principalTable: "apps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "company_roles",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    app_id = table.Column<Guid>(nullable: false),
                    key = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_company_roles", x => x.id);
                    table.ForeignKey(
                        name: "fk_company_roles_apps_app_id",
                        column: x => x.app_id,
                        principalTable: "apps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    app_id = table.Column<Guid>(nullable: false),
                    key = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                    table.ForeignKey(
                        name: "fk_roles_apps_app_id",
                        column: x => x.app_id,
                        principalTable: "apps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_logins",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    app_id = table.Column<Guid>(nullable: false),
                    user_id = table.Column<Guid>(nullable: false),
                    logged_at = table.Column<DateTime>(nullable: false),
                    ip_address = table.Column<string>(nullable: false),
                    client = table.Column<string>(nullable: false),
                    refresh_token = table.Column<Guid>(nullable: false),
                    refresh_token_expires = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_logins", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_logins_apps_app_id",
                        column: x => x.app_id,
                        principalTable: "apps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_logins_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "company_users",
                columns: table => new
                {
                    company_id = table.Column<Guid>(nullable: false),
                    user_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_company_users", x => new { x.company_id, x.user_id });
                    table.ForeignKey(
                        name: "fk_company_users_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_company_users_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_claims",
                columns: table => new
                {
                    user_id = table.Column<Guid>(nullable: false),
                    claim_id = table.Column<Guid>(nullable: false),
                    value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_claims", x => new { x.user_id, x.claim_id });
                    table.ForeignKey(
                        name: "fk_user_claims_claims_claim_id",
                        column: x => x.claim_id,
                        principalTable: "claims",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_claims_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "company_user_claims",
                columns: table => new
                {
                    company_id = table.Column<Guid>(nullable: false),
                    user_id = table.Column<Guid>(nullable: false),
                    claim_id = table.Column<Guid>(nullable: false),
                    value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_company_user_claims", x => new { x.company_id, x.user_id, x.claim_id });
                    table.ForeignKey(
                        name: "fk_company_user_claims_company_claims_claim_id",
                        column: x => x.claim_id,
                        principalTable: "company_claims",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_company_user_claims_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_company_user_claims_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "company_role_claims",
                columns: table => new
                {
                    role_id = table.Column<Guid>(nullable: false),
                    claim_id = table.Column<Guid>(nullable: false),
                    value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_company_role_claims", x => new { x.role_id, x.claim_id });
                    table.ForeignKey(
                        name: "fk_company_role_claims_company_claims_claim_id",
                        column: x => x.claim_id,
                        principalTable: "company_claims",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_company_role_claims_company_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "company_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "company_user_roles",
                columns: table => new
                {
                    company_id = table.Column<Guid>(nullable: false),
                    user_id = table.Column<Guid>(nullable: false),
                    role_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_company_user_roles", x => new { x.company_id, x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_company_user_roles_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_company_user_roles_company_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "company_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_company_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "role_claims",
                columns: table => new
                {
                    role_id = table.Column<Guid>(nullable: false),
                    claim_id = table.Column<Guid>(nullable: false),
                    value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_claims", x => new { x.role_id, x.claim_id });
                    table.ForeignKey(
                        name: "fk_role_claims_claims_claim_id",
                        column: x => x.claim_id,
                        principalTable: "claims",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_role_claims_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    user_id = table.Column<Guid>(nullable: false),
                    role_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "login", "password_hash", "referral_id" },
                values: new object[] { new Guid("baa0ad0f-91c5-4c19-963c-ea369048e67a"), "a.kreskiyan@gmail.com", "alex", "ohraPG8QMZiOnXX+MWh/45aZDwjtv/7FQMFzXxSRxQjLdSMBHpELKDSznF6cSUalufovlgCfFkn4mtR7eXB+8w==", null });

            migrationBuilder.InsertData(
                table: "apps",
                columns: new[] { "id", "api_token", "name", "owner_id" },
                values: new object[] { new Guid("278e20ae-00c7-4ba5-8db3-55df7af12d44"), new Guid("b62acd2a-2f1b-4da1-9273-abab4b9da7f7"), "Annium ID", new Guid("baa0ad0f-91c5-4c19-963c-ea369048e67a") });

            migrationBuilder.CreateIndex(
                name: "ix_apps_owner_id",
                table: "apps",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_claims_app_id_key",
                table: "claims",
                columns: new[] { "app_id", "key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_companies_owner_id",
                table: "companies",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_companies_parent_id",
                table: "companies",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_company_claims_app_id_key",
                table: "company_claims",
                columns: new[] { "app_id", "key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_company_role_claims_claim_id",
                table: "company_role_claims",
                column: "claim_id");

            migrationBuilder.CreateIndex(
                name: "ix_company_roles_app_id_key",
                table: "company_roles",
                columns: new[] { "app_id", "key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_company_user_claims_claim_id",
                table: "company_user_claims",
                column: "claim_id");

            migrationBuilder.CreateIndex(
                name: "ix_company_user_claims_user_id",
                table: "company_user_claims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_company_user_roles_role_id",
                table: "company_user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_company_user_roles_user_id",
                table: "company_user_roles",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_company_users_user_id",
                table: "company_users",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_claims_claim_id",
                table: "role_claims",
                column: "claim_id");

            migrationBuilder.CreateIndex(
                name: "ix_roles_app_id_key",
                table: "roles",
                columns: new[] { "app_id", "key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_claims_claim_id",
                table: "user_claims",
                column: "claim_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_logins_app_id",
                table: "user_logins",
                column: "app_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_logins_refresh_token",
                table: "user_logins",
                column: "refresh_token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_logins_user_id",
                table: "user_logins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_role_id",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_login",
                table: "users",
                column: "login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_referral_id",
                table: "users",
                column: "referral_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "company_role_claims");

            migrationBuilder.DropTable(
                name: "company_user_claims");

            migrationBuilder.DropTable(
                name: "company_user_roles");

            migrationBuilder.DropTable(
                name: "company_users");

            migrationBuilder.DropTable(
                name: "role_claims");

            migrationBuilder.DropTable(
                name: "user_claims");

            migrationBuilder.DropTable(
                name: "user_logins");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "company_claims");

            migrationBuilder.DropTable(
                name: "company_roles");

            migrationBuilder.DropTable(
                name: "companies");

            migrationBuilder.DropTable(
                name: "claims");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "apps");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
