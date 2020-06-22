PROJECT_NAME := id
TAG_PREFIX := registry.annium.com/$(PROJECT_NAME)

configure:
	@# api
	$(call copy,run/api/configuration server/src/api/Annium.Id.Api/configuration,application.yml db.yml email.yml)
	$(call copy,run/api/keys server/src/api/Annium.Id.Api/keys,private.key public.key)
	$(call copy,server/test/api/Annium.Id.IntegrationTests/keys,private.key public.key)

	@# db
	$(call copy,run/db,db.env)

	@# migrator
	$(call copy,server/src/infrastructure/Annium.Id.Infrastructure.DbMigrator/configuration,db.yml)

deconfigure:
	rm -rf run
	$(call clean,/configuration/ /keys/)


publish-api:
	$(call publish,api,server,src/Api/Annium.Id.Api/Dockerfile)
	$(call publish,migrations,server,src/Api/Annium.Id.Api/migrations.Dockerfile)

publish-site:
	$(call publish,site,web/site,Dockerfile)


define publish
	@$(eval image := $(1))
	@$(eval context := $(2))
	@$(eval dockerfile := $(3))
	@docker build -t $(TAG_PREFIX)/$(image) -f $(context)/$(dockerfile) $(context)
	@docker push $(TAG_PREFIX)/$(image)
endef

define copy
	$(foreach dir,$(1),rm -rf $(dir); mkdir -p $(dir);$(foreach file,$(2),cp cfg/$(file) $(dir);))
endef

define clean
	$(foreach pattern,$(1),git ls-files --others server | grep $(pattern) | xargs rm -f;)
endef

.PHONY: $(MAKECMDGOALS)

# https://github.com/aspnet/EntityFrameworkCore/issues/18292 - to cleanup migrations flow
# test targets for migrations
# migrations-build:
# 	docker build -t migrations -f server/src/Annium.Id.Api/migrations.Dockerfile server

# migrations-run:
# 	docker run --rm -d --name migrations \
# 		-v ~/projects/annium/id/run/configuration/:/code/src/Annium.Id.Api/configuration \
# 		--network id_net \
# 		migrations \
# 		sleep 3600
# 		# -v ~/projects/annium/id/server/src/Annium.Id.Api/configuration/:/app/configuration/ \

# migrations-test:
# 	docker exec -it migrations \
# 		dotnet ef migrations list \
# 		--project src/Annium.Id.Db \
# 		--startup-project src/Annium.Id.Api \
# 		--no-build

# migrations-kill:
# 	docker kill migrations
