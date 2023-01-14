create table public.company_user_claims (
	company_id uuid not null,
	user_id uuid not null,
	claim_id uuid not null,
	value text not null,
	constraint pk_company_user_claims primary key (company_id, user_id, claim_id),
	constraint fk_company_user_claims_companies_company_id foreign key (company_id) references public.companies(id) on delete restrict,
	constraint fk_company_user_claims_company_claims_claim_id foreign key (claim_id) references public.company_claims(id) on delete restrict,
	constraint fk_company_user_claims_users_user_id foreign key (user_id) references public.users(id) on delete restrict
);
create index ix_company_user_claims_claim_id on public.company_user_claims using btree (claim_id);
create index ix_company_user_claims_user_id on public.company_user_claims using btree (user_id);