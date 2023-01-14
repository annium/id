create table public.apps (
	id uuid not null,
	owner_id uuid not null,
	"name" text not null,
	api_token uuid not null,
	constraint pk_apps primary key (id),
	constraint fk_apps_users_owner_id foreign key (owner_id) references public.users(id) on delete restrict
);
create index ix_apps_owner_id on public.apps using btree (owner_id);