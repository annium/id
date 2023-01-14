create table public.claims (
	id uuid not null,
	app_id uuid not null,
	"key" text not null,
	"name" text not null,
	constraint pk_claims primary key (id),
	constraint fk_claims_apps_app_id foreign key (app_id) references public.apps(id) on delete restrict
);
create unique index ix_claims_app_id_key on public.claims using btree (app_id, key);