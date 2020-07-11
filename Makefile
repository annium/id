PROJECT_NAME := id
TAG_PREFIX := registry.annium.com/$(PROJECT_NAME)

configure:
	@# api
	$(call copy,shared,application.yml email.yml,run/api/configuration server/src/api/Annium.Id.Api/configuration)
	$(call copy,docker,db.yml,run/api/configuration)
	$(call copy,local,db.yml,server/src/api/Annium.Id.Api/configuration)
	$(call copy,shared,private.key public.key,run/api/keys server/src/api/Annium.Id.Api/keys)

	@# db
	$(call copy,docker,db.env,run/db)

	@# migrator
	$(call copy,local,db.yml,server/src/infrastructure/Annium.Id.Infrastructure.DbMigrator/configuration)

	@# tests api
	$(call copy,shared,private.key public.key,server/test/api/Annium.Id.Api.IntegrationTests/keys)

	@# tests demo
	$(call copy,shared,private.key public.key,server/test/api/Annium.Id.DemoClient/keys)

	@# tests core
	$(call copy,shared,private.key public.key,server/test/lib/Annium.Id.Core.Tests/keys)

deconfigure:
	rm -rf run
	$(call clean,/configuration/ /keys/)


gen-api-site-client:
	xrest ts gen -s http://localhost:9501 -a server/src/api/Annium.Id.Api/bin/Debug/netcoreapp3.1/Annium.Id.Api.dll -o web/site/src/shared/api/server/client -trace

gen-api-test-client:
	xrest dotnet gen -s http://localhost:5000 -a server/src/api/Annium.Id.Api/bin/Debug/netcoreapp3.1/Annium.Id.Api.dll -o server/test/api/Annium.Id.Api.TestClient -t -trace

gen-demo-test-client:
	xrest dotnet gen -s http://localhost:5000 -a server/test/api/Annium.Id.Demo/bin/Debug/netcoreapp3.1/Annium.Id.Demo.dll -o server/test/api/Annium.Id.Demo.TestClient -t -trace


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
	$(foreach dir,$(3),mkdir -p $(dir);$(foreach file,$(2),cp cfg/$(1)/$(file) $(dir);))
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
