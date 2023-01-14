create table public.companies (
	id uuid not null,
	owner_id uuid not null,
	parent_id uuid null,
	"name" text not null,
	constraint pk_companies primary key (id),
	constraint fk_companies_companies_parent_id foreign key (parent_id) references public.companies(id) on delete restrict,
	constraint fk_companies_users_owner_id foreign key (owner_id) references public.users(id) on delete restrict
);
create index ix_companies_owner_id on public.companies using btree (owner_id);
create index ix_companies_parent_id on public.companies using btree (parent_id);