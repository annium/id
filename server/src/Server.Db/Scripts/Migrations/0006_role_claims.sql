create table public.role_claims (
	role_id uuid not null,
	claim_id uuid not null,
	value text not null,
	constraint pk_role_claims primary key (role_id, claim_id),
	constraint fk_role_claims_claims_claim_id foreign key (claim_id) references public.claims(id) on delete restrict,
	constraint fk_role_claims_roles_role_id foreign key (role_id) references public.roles(id) on delete restrict
);
create index ix_role_claims_claim_id on public.role_claims using btree (claim_id);