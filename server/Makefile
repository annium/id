db-drop db-update migrations-add migrations-list migrations-remove:
	@pwsh tools/ef/$@.ps1 -startup src/Api/Annium.Id.Api -project src/Shared/Annium.Id.Db -context Context

.PHONY: $(MAKECMDGOALS)
