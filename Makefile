PROJECT_NAME := id
TAG_PREFIX := registry.annium.com/$(PROJECT_NAME)

db-up api-up:
	@pwsh setup/scripts/net-up.ps1 -project $(PROJECT_NAME)
	@pwsh setup/scripts/up.ps1 -project $(PROJECT_NAME) -component $(subst -up,,$@)

db-down api-down:
	@pwsh setup/scripts/down.ps1 -project $(PROJECT_NAME) -component $(subst -down,,$@)
	@pwsh setup/scripts/net-down.ps1 -project $(PROJECT_NAME)

db-log api-log:
	@docker logs -f $(PROJECT_NAME)_$(subst -log,,$@)


db-drop db-update migrations-add migrations-list migrations-remove:
	@cd server && pwsh tools/ef-$@.ps1 -startup src/Annium.Id.Api -project src/Annium.Id.Db -context Context

publish-api:
	@cp $$(find $$(dirname $$(realpath $$(which dotnet)))/sdk -type f -name ef.dll | grep netcoreapp2.2) server/src
	$(call publish,api,server/src,Annium.Id.Api/Dockerfile)
	@rm -f server/src/ef.dll


define publish
	@$(eval image := $(1))
	@$(eval context := $(2))
	@$(eval dockerfile := $(3))
	@docker build -t $(TAG_PREFIX)/$(image) -f $(context)/$(dockerfile) $(context)
	@docker push $(TAG_PREFIX)/$(image)
endef

.PHONY: $(MAKECMDGOALS)