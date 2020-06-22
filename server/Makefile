db-drop db-update migrations-add migrations-list migrations-remove:
	@pwsh tools/ef/$@.ps1 \
		-startup src/infrastructure/Annium.Id.Infrastructure.DbMigrator \
 		-project src/infrastructure/Annium.Id.Infrastructure.DbMigrator \
		-context Context

.PHONY: $(MAKECMDGOALS)
