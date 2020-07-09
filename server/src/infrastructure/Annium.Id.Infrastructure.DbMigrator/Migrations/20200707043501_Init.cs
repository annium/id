using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Annium.Id.Infrastructure.DbMigrator.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "users",
                table => new
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
                        "fk_users_users_referral_id",
                        x => x.referral_id,
                        "users",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "apps",
                table => new
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
                        "fk_apps_users_owner_id",
                        x => x.owner_id,
                        "users",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "companies",
                table => new
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
                        "fk_companies_users_owner_id",
                        x => x.owner_id,
                        "users",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "fk_companies_companies_parent_id",
                        x => x.parent_id,
                        "companies",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "claims",
                table => new
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
                        "fk_claims_apps_app_id",
                        x => x.app_id,
                        "apps",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "company_claims",
                table => new
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
                        "fk_company_claims_apps_app_id",
                        x => x.app_id,
                        "apps",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "company_roles",
                table => new
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
                        "fk_company_roles_apps_app_id",
                        x => x.app_id,
                        "apps",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "roles",
                table => new
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
                        "fk_roles_apps_app_id",
                        x => x.app_id,
                        "apps",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "user_logins",
                table => new
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
                        "fk_user_logins_apps_app_id",
                        x => x.app_id,
                        "apps",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "fk_user_logins_users_user_id",
                        x => x.user_id,
                        "users",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "company_users",
                table => new
                {
                    company_id = table.Column<Guid>(nullable: false),
                    user_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_company_users", x => new { x.company_id, x.user_id });
                    table.ForeignKey(
                        "fk_company_users_companies_company_id",
                        x => x.company_id,
                        "companies",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "fk_company_users_users_user_id",
                        x => x.user_id,
                        "users",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "user_claims",
                table => new
                {
                    user_id = table.Column<Guid>(nullable: false),
                    claim_id = table.Column<Guid>(nullable: false),
                    value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_claims", x => new { x.user_id, x.claim_id });
                    table.ForeignKey(
                        "fk_user_claims_claims_claim_id",
                        x => x.claim_id,
                        "claims",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "fk_user_claims_users_user_id",
                        x => x.user_id,
                        "users",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "company_user_claims",
                table => new
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
                        "fk_company_user_claims_company_claims_claim_id",
                        x => x.claim_id,
                        "company_claims",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "fk_company_user_claims_companies_company_id",
                        x => x.company_id,
                        "companies",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "fk_company_user_claims_users_user_id",
                        x => x.user_id,
                        "users",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "company_role_claims",
                table => new
                {
                    role_id = table.Column<Guid>(nullable: false),
                    claim_id = table.Column<Guid>(nullable: false),
                    value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_company_role_claims", x => new { x.role_id, x.claim_id });
                    table.ForeignKey(
                        "fk_company_role_claims_company_claims_claim_id",
                        x => x.claim_id,
                        "company_claims",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "fk_company_role_claims_company_roles_role_id",
                        x => x.role_id,
                        "company_roles",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "company_user_roles",
                table => new
                {
                    company_id = table.Column<Guid>(nullable: false),
                    user_id = table.Column<Guid>(nullable: false),
                    role_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_company_user_roles", x => new { x.company_id, x.user_id, x.role_id });
                    table.ForeignKey(
                        "fk_company_user_roles_companies_company_id",
                        x => x.company_id,
                        "companies",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "fk_company_user_roles_company_roles_role_id",
                        x => x.role_id,
                        "company_roles",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "fk_company_user_roles_users_user_id",
                        x => x.user_id,
                        "users",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "role_claims",
                table => new
                {
                    role_id = table.Column<Guid>(nullable: false),
                    claim_id = table.Column<Guid>(nullable: false),
                    value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_claims", x => new { x.role_id, x.claim_id });
                    table.ForeignKey(
                        "fk_role_claims_claims_claim_id",
                        x => x.claim_id,
                        "claims",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "fk_role_claims_roles_role_id",
                        x => x.role_id,
                        "roles",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "user_roles",
                table => new
                {
                    user_id = table.Column<Guid>(nullable: false),
                    role_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        "fk_user_roles_roles_role_id",
                        x => x.role_id,
                        "roles",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "fk_user_roles_users_user_id",
                        x => x.user_id,
                        "users",
                        "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                "users",
                new[] { "id", "email", "login", "password_hash", "referral_id" },
                new object[]
                {
                    new Guid("baa0ad0f-91c5-4c19-963c-ea369048e67a"), "a.kreskiyan@gmail.com", "alex",
                    "ohraPG8QMZiOnXX+MWh/45aZDwjtv/7FQMFzXxSRxQjLdSMBHpELKDSznF6cSUalufovlgCfFkn4mtR7eXB+8w==", null
                });

            migrationBuilder.InsertData(
                "apps",
                new[] { "id", "api_token", "name", "owner_id" },
                new object[]
                {
                    new Guid("278e20ae-00c7-4ba5-8db3-55df7af12d44"), new Guid("b62acd2a-2f1b-4da1-9273-abab4b9da7f7"), "Annium ID",
                    new Guid("baa0ad0f-91c5-4c19-963c-ea369048e67a")
                });

            migrationBuilder.CreateIndex(
                "ix_apps_owner_id",
                "apps",
                "owner_id");

            migrationBuilder.CreateIndex(
                "ix_claims_app_id_key",
                "claims",
                new[] { "app_id", "key" },
                unique: true);

            migrationBuilder.CreateIndex(
                "ix_companies_owner_id",
                "companies",
                "owner_id");

            migrationBuilder.CreateIndex(
                "ix_companies_parent_id",
                "companies",
                "parent_id");

            migrationBuilder.CreateIndex(
                "ix_company_claims_app_id_key",
                "company_claims",
                new[] { "app_id", "key" },
                unique: true);

            migrationBuilder.CreateIndex(
                "ix_company_role_claims_claim_id",
                "company_role_claims",
                "claim_id");

            migrationBuilder.CreateIndex(
                "ix_company_roles_app_id_key",
                "company_roles",
                new[] { "app_id", "key" },
                unique: true);

            migrationBuilder.CreateIndex(
                "ix_company_user_claims_claim_id",
                "company_user_claims",
                "claim_id");

            migrationBuilder.CreateIndex(
                "ix_company_user_claims_user_id",
                "company_user_claims",
                "user_id");

            migrationBuilder.CreateIndex(
                "ix_company_user_roles_role_id",
                "company_user_roles",
                "role_id");

            migrationBuilder.CreateIndex(
                "ix_company_user_roles_user_id",
                "company_user_roles",
                "user_id");

            migrationBuilder.CreateIndex(
                "ix_company_users_user_id",
                "company_users",
                "user_id");

            migrationBuilder.CreateIndex(
                "ix_role_claims_claim_id",
                "role_claims",
                "claim_id");

            migrationBuilder.CreateIndex(
                "ix_roles_app_id_key",
                "roles",
                new[] { "app_id", "key" },
                unique: true);

            migrationBuilder.CreateIndex(
                "ix_user_claims_claim_id",
                "user_claims",
                "claim_id");

            migrationBuilder.CreateIndex(
                "ix_user_logins_app_id",
                "user_logins",
                "app_id");

            migrationBuilder.CreateIndex(
                "ix_user_logins_refresh_token",
                "user_logins",
                "refresh_token",
                unique: true);

            migrationBuilder.CreateIndex(
                "ix_user_logins_user_id",
                "user_logins",
                "user_id");

            migrationBuilder.CreateIndex(
                "ix_user_roles_role_id",
                "user_roles",
                "role_id");

            migrationBuilder.CreateIndex(
                "ix_users_email",
                "users",
                "email",
                unique: true);

            migrationBuilder.CreateIndex(
                "ix_users_login",
                "users",
                "login",
                unique: true);

            migrationBuilder.CreateIndex(
                "ix_users_referral_id",
                "users",
                "referral_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "company_role_claims");

            migrationBuilder.DropTable(
                "company_user_claims");

            migrationBuilder.DropTable(
                "company_user_roles");

            migrationBuilder.DropTable(
                "company_users");

            migrationBuilder.DropTable(
                "role_claims");

            migrationBuilder.DropTable(
                "user_claims");

            migrationBuilder.DropTable(
                "user_logins");

            migrationBuilder.DropTable(
                "user_roles");

            migrationBuilder.DropTable(
                "company_claims");

            migrationBuilder.DropTable(
                "company_roles");

            migrationBuilder.DropTable(
                "companies");

            migrationBuilder.DropTable(
                "claims");

            migrationBuilder.DropTable(
                "roles");

            migrationBuilder.DropTable(
                "apps");

            migrationBuilder.DropTable(
                "users");
        }
    }
}