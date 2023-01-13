PROJECT_NAME := id
TAG_PREFIX := registry.annium.com/$(PROJECT_NAME)
TFM := net7.0
BIN_DEBUG := bin/Debug/$(TFM)

configure:
	@# host
	$(call copy,shared,application.yml email.yml,run/server/configuration server/src/Server.Host/configuration)
	$(call copy,docker,db.yml,run/server/configuration)
	$(call copy,local,db.yml,server/src/Server.Host/configuration)
	$(call copy,shared,private.key public.key,run/server/keys server/src/Server.Host/keys)

	@# db
	$(call copy,docker,db.env,run/db)

	@# migrator
	$(call copy,local,db.yml,server/src/Server.Db.Migrator/configuration)

	@# server tests
	$(call copy,shared,private.key public.key,server/test/Server.IntegrationTests/keys)

	@# demo host
	$(call copy,shared,private.key public.key,server/test/Server.DemoHost/keys)

	@# core tests
	$(call copy,shared,private.key public.key,lib/test/Annium.Id.Core.Tests/keys)

deconfigure:
	rm -rf run
	$(call clean,/configuration/ /keys/)


db-drop db-migrate-up db-migrate-down migrations-add migrations-list migrations-remove:
	@pwsh tools/ef/$@.ps1 \
		-startup server/src/Server.Db.Migrator \
 		-project server/src/Server.Db.Migrator \
		-context Context


start: start-server start-demo

start-server:
	$(call start-dotnet,server/src,Server.Host,9501)

start-demo:
	$(call start-dotnet,server/test,Server.DemoHost,9502)

stop: stop-server stop-demo

stop-server:
	$(call stop-dotnet,Server.Host)

stop-demo:
	$(call stop-dotnet,Server.DemoHost)


gwc: gwc-server

gwc-server:
	xrest dotnet gen \
		-s http://localhost:9501 \
		-a server/src/Server.Host/$(BIN_DEBUG)/Server.Host.dll \
		-o web/src/Site/Shared/Api/Server \
		-ns Site.Shared.Api.Server

gtc: gtc-server gtc-demo

gtc-server:
	xrest dotnet gen \
		-s http://localhost:9501 \
		-a server/src/Server.Host/$(BIN_DEBUG)/Server.Host.dll \
		-o server/test/Server.Host.TestClient \
		-t

gtc-demo:
	xrest dotnet gen \
		-s http://localhost:9502 \
		-a server/test/Server.DemoHost/$(BIN_DEBUG)/Server.DemoHost.dll \
		-o server/test/Server.DemoHost.TestClient \
		-t


publish: publish-server publish-site

publish-server:
	$(call publish,server,.,server/src/Server.Host/app.dockerfile)

publish-site:
	$(call publish,site,.,web/src/Site/app.dockerfile)


# control
define start-dotnet
	@$(eval folder := $(1))
	@$(eval project := $(2))
	@$(eval port := $(3))
	cd $(folder)/$(project) && dotnet $(BIN_DEBUG)/$(project).dll -port $(port) &
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