create table public.company_users (
	company_id uuid not null,
	user_id uuid not null,
	constraint pk_company_users primary key (company_id, user_id),
	constraint fk_company_users_companies_company_id foreign key (company_id) references public.companies(id) on delete restrict,
	constraint fk_company_users_users_user_id foreign key (user_id) references public.users(id) on delete restrict
);
create index ix_company_users_user_id on public.company_users using btree (user_id);