create table public.user_claims (
	user_id uuid not null,
	claim_id uuid not null,
	value text not null,
	constraint pk_user_claims primary key (user_id, claim_id),
	constraint fk_user_claims_claims_claim_id foreign key (claim_id) references public.claims(id) on delete restrict,
	constraint fk_user_claims_users_user_id foreign key (user_id) references public.users(id) on delete restrict
);
create index ix_user_claims_claim_id on public.user_claims using btree (claim_id);