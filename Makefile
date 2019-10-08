PROJECT_NAME := id
TAG_PREFIX := registry.annium.com/$(PROJECT_NAME)

db-up api-up site-up:
	@pwsh setup/scripts/net-up.ps1 -project $(PROJECT_NAME)
	@pwsh setup/scripts/up.ps1 -project $(PROJECT_NAME) -component $(subst -up,,$@)

db-down api-down site-down:
	@pwsh setup/scripts/down.ps1 -project $(PROJECT_NAME) -component $(subst -down,,$@)
	@pwsh setup/scripts/net-down.ps1 -project $(PROJECT_NAME)

db-log api-log site-log:
	@docker logs -f $(PROJECT_NAME)_$(subst -log,,$@)


db-drop db-update migrations-add migrations-list migrations-remove:
	@cd server && pwsh tools/ef/$@.ps1 -startup src/Annium.Id.Api -project src/Annium.Id.Db -context Context

publish-api:
	$(call publish,migrations,server,src/Annium.Id.Api/migrations.Dockerfile)
	$(call publish,api,server,src/Annium.Id.Api/api.Dockerfile)

publish-site:
	$(call publish,site,web/src/site,Dockerfile)


define publish
	@$(eval image := $(1))
	@$(eval context := $(2))
	@$(eval dockerfile := $(3))
	@docker build -t $(TAG_PREFIX)/$(image) -f $(context)/$(dockerfile) $(context)
	@docker push $(TAG_PREFIX)/$(image)
endef

.PHONY: $(MAKECMDGOALS)