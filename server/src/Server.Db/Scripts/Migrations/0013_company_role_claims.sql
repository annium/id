create table public.company_role_claims (
	role_id uuid not null,
	claim_id uuid not null,
	value text not null,
	constraint pk_company_role_claims primary key (role_id, claim_id),
	constraint fk_company_role_claims_company_claims_claim_id foreign key (claim_id) references public.company_claims(id) on delete restrict,
	constraint fk_company_role_claims_company_roles_role_id foreign key (role_id) references public.company_roles(id) on delete restrict
);
create index ix_company_role_claims_claim_id on public.company_role_claims using btree (claim_id);