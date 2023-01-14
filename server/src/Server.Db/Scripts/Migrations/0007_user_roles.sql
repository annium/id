create table public.user_roles (
	user_id uuid not null,
	role_id uuid not null,
	constraint pk_user_roles primary key (user_id, role_id),
	constraint fk_user_roles_roles_role_id foreign key (role_id) references public.roles(id) on delete restrict,
	constraint fk_user_roles_users_user_id foreign key (user_id) references public.users(id) on delete restrict
);
create index ix_user_roles_role_id on public.user_roles using btree (role_id);