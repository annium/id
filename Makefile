PROJECT_NAME := id
TAG_PREFIX := registry.annium.com/$(PROJECT_NAME)

configure:
	@# core api
	$(call copy,run/api-core/configuration server/src/core/Crypted.Core.Api/configuration,db.yml id.yml mbus.yml)
	$(call copy,run/api-core/keys server/src/core/Crypted.Core.Api/keys,public.key)
	$(call copy,server/test/core/Crypted.Core.Api.IntegrationTests/keys,private.key public.key)

	@# assets api
	$(call copy,run/api-assets/configuration server/src/assets/Crypted.Assets.Api/configuration,db.yml id.yml mbus.yml)
	$(call copy,run/api-assets/keys server/src/assets/Crypted.Assets.Api/keys,public.key)
	$(call copy,server/test/assets/Crypted.Assets.Api.IntegrationTests/keys,private.key public.key)

	@# flow
	$(call copy,run/api-flow/configuration server/src/flow/Crypted.Flow.Api/configuration,db.yml id.yml mbus.yml)
	$(call copy,run/api-flow/keys server/src/flow/Crypted.Flow.Api/keys,public.key)
	$(call copy,run/service-flow/configuration server/src/flow/Crypted.Flow.Service/configuration,db.yml mbus.yml)
	$(call copy,server/test/flow/Crypted.Flow.Api.IntegrationTests/keys,private.key public.key)

	@# providers api
	$(call copy,run/api-providers/configuration server/src/providers/Crypted.Providers.Api/configuration,db.yml id.yml mbus.yml)
	$(call copy,run/api-providers/keys server/src/providers/Crypted.Providers.Api/keys,public.key)
	$(call copy,server/test/providers/Crypted.Providers.Api.IntegrationTests/keys,private.key public.key)

	@# uni api
	$(call copy,run/api-providers/configuration server/src/uni/Crypted.Uni.Api/configuration,db.yml id.yml mbus.yml)
	$(call copy,run/api-providers/keys server/src/uni/Crypted.Uni.Api/keys,public.key)

	@# api (old)
	$(call copy,server/test/api/Crypted.Api.IntegrationTests/keys,private.key public.key)

	@# db
	$(call copy,run/db,db.env)

	@# migrator
	$(call copy,server/src/shared/Crypted.Infrastructure.DbMigrator/configuration,db.yml)

	@# msink
	$(call copy,run/msink/configuration server/src/mbus/Crypted.MessageBus.Sink/configuration,mbus.yml)

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
