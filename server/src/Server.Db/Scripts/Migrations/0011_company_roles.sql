create table public.company_roles (
	id uuid not null,
	app_id uuid not null,
	"key" text not null,
	"name" text not null,
	constraint pk_company_roles primary key (id),
	constraint fk_company_roles_apps_app_id foreign key (app_id) references public.apps(id) on delete restrict
);
create unique index ix_company_roles_app_id_key on public.company_roles using btree (app_id, key);