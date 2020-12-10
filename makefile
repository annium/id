PROJECT_NAME := id
TAG_PREFIX := registry.annium.com/$(PROJECT_NAME)
TFM := net5.0
BIN_DEBUG := bin/Debug/$(TFM)

configure:
	@# api
	$(call copy,shared,application.yml email.yml,run/api/configuration src/api/Annium.Id.Api/configuration)
	$(call copy,docker,db.yml,run/api/configuration)
	$(call copy,local,db.yml,src/api/Annium.Id.Api/configuration)
	$(call copy,shared,private.key public.key,run/api/keys src/api/Annium.Id.Api/keys)

	@# db
	$(call copy,docker,db.env,run/db)

	@# migrator
	$(call copy,local,db.yml,src/infrastructure/Annium.Id.Infrastructure.DbMigrator/configuration)

	@# tests api
	$(call copy,shared,private.key public.key,test/api/Annium.Id.Api.IntegrationTests/keys)

	@# tests demo
	$(call copy,shared,private.key public.key,test/api/Annium.Id.DemoClient/keys)

	@# tests core
	$(call copy,shared,private.key public.key,test/lib/Annium.Id.Core.Tests/keys)

deconfigure:
	rm -rf run
	$(call clean,/configuration/ /keys/)


db-drop db-migrate-up db-migrate-down migrations-add migrations-list migrations-remove:
	@pwsh tools/ef/$@.ps1 \
		-startup src/infrastructure/Annium.Id.Infrastructure.DbMigrator \
 		-project src/infrastructure/Annium.Id.Infrastructure.DbMigrator \
		-context Context


start: start-api start-demo

start-api:
	$(call start-dotnet,src,api,Annium.Id.Api,9501)

start-demo:
	$(call start-dotnet,test,api,Annium.Id.Demo,9502)

stop: stop-api stop-demo

stop-api:
	$(call stop-dotnet,Annium.Id.Api)

stop-demo:
	$(call stop-dotnet,Annium.Id.Demo)


gwc: gwc-api

gwc-api:
	xrest dotnet gen \
		-s http://localhost:9501 \
		-a src/api/Annium.Id.Api/$(BIN_DEBUG)/Annium.Id.Api.dll \
		-o src/web/Annium.Id.Site/Shared/Api/Server \
		-ns Annium.Id.Site.Shared.Api.Server

gtc: gtc-api gtc-demo

gtc-api:
	xrest dotnet gen \
		-s http://localhost:9501 \
		-a src/api/Annium.Id.Api/$(BIN_DEBUG)/Annium.Id.Api.dll \
		-o test/api/Annium.Id.Api.TestClient \
		-t

gtc-demo:
	xrest dotnet gen \
		-s http://localhost:9502 \
		-a test/api/Annium.Id.Demo/$(BIN_DEBUG)/Annium.Id.Demo.dll \
		-o test/api/Annium.Id.Demo.TestClient \
		-t


publish: publish-api publish-migrations publish-site

publish-api:
	$(call publish,api,.,src/api/Annium.Id.Api/app.dockerfile)

publish-migrations:
	$(call publish,migrations,.,src/infrastructure/Annium.Id.Infrastructure.DbMigrator/migrations.dockerfile)

publish-site:
	$(call publish,site,.,src/web/Annium.Id.Site/app.dockerfile)


# control
define start-dotnet
	@$(eval section := $(1))
	@$(eval component := $(2))
	@$(eval project := $(3))
	@$(eval port := $(4))
	cd $(section)/$(component)/$(project) && dotnet $(BIN_DEBUG)/$(project).dll -port $(port) &
endef

define stop-dotnet
	@$(eval project := $(1))
	ps -ax | grep $(project).dll | grep -v src | grep -v grep | sed -e 's#^ *##' | cut -d ' ' -f 1 | xargs -I% kill %
endef


define publish
	@$(eval image := $(1))
	@$(eval context := $(2))
	@$(eval dockerfile := $(3))
	@docker build -t $(TAG_PREFIX)/$(image) -f $(context)/$(dockerfile) $(context)
	@docker push $(TAG_PREFIX)/$(image)
endef

define copy
	$(foreach dir,$(3),mkdir -p $(dir);$(foreach file,$(2),cp cfg/$(1)/$(file) $(dir);))
endef

define clean
	$(foreach pattern,$(1),git ls-files --others . | grep $(pattern) | xargs rm -f;)
endef

.PHONY: $(MAKECMDGOALS)

# https://github.com/aspnet/EntityFrameworkCore/issues/18292 - to cleanup migrations flow
# test targets for migrations
# migrations-build:
# 	docker build -t migrations -f src/Annium.Id.Api/migrations.Dockerfile server

# migrations-run:
# 	docker run --rm -d --name migrations \
# 		-v ~/projects/annium/id/run/configuration/:/code/src/Annium.Id.Api/configuration \
# 		--network id_net \
# 		migrations \
# 		sleep 3600
# 		# -v ~/projects/annium/id/src/Annium.Id.Api/configuration/:/app/configuration/ \

# migrations-test:
# 	docker exec -it migrations \
# 		dotnet ef migrations list \
# 		--project src/Annium.Id.Db \
# 		--startup-project src/Annium.Id.Api \
# 		--no-build

# migrations-kill:
# 	docker kill migrations
