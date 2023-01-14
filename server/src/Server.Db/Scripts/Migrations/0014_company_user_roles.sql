create table public.company_user_roles (
	company_id uuid not null,
	user_id uuid not null,
	role_id uuid not null,
	constraint pk_company_user_roles primary key (company_id, user_id, role_id),
	constraint fk_company_user_roles_companies_company_id foreign key (company_id) references public.companies(id) on delete restrict,
	constraint fk_company_user_roles_company_roles_role_id foreign key (role_id) references public.company_roles(id) on delete restrict,
	constraint fk_company_user_roles_users_user_id foreign key (user_id) references public.users(id) on delete restrict
);
create index ix_company_user_roles_role_id on public.company_user_roles using btree (role_id);
create index ix_company_user_roles_user_id on public.company_user_roles using btree (user_id);