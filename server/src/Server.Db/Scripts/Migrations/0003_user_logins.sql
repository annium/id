create table public.user_logins (
	id uuid not null,
	app_id uuid not null,
	user_id uuid not null,
	logged_at timestamp not null,
	ip_address text not null,
	client text not null,
	refresh_token uuid not null,
	refresh_token_expires timestamp not null,
	constraint pk_user_logins primary key (id),
	constraint fk_user_logins_apps_app_id foreign key (app_id) references public.apps(id) on delete restrict,
	constraint fk_user_logins_users_user_id foreign key (user_id) references public.users(id) on delete restrict
);
create index ix_user_logins_app_id on public.user_logins using btree (app_id);
create unique index ix_user_logins_refresh_token on public.user_logins using btree (refresh_token);
create index ix_user_logins_user_id on public.user_logins using btree (user_id);