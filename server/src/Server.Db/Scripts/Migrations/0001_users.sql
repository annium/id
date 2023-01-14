create table public.users (
	id uuid not null,
	login text not null,
	password_hash text not null,
	email text not null,
	referral_id uuid null,
	constraint pk_users primary key (id),
	constraint fk_users_users_referral_id foreign key (referral_id) references public.users(id) on delete restrict
);
create unique index ix_users_email on public.users using btree (email);
create unique index ix_users_login on public.users using btree (login);
create index ix_users_referral_id on public.users using btree (referral_id);