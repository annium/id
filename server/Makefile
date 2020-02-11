db-drop db-update migrations-add migrations-list migrations-remove:
	@pwsh tools/ef/$@.ps1 -startup src/Annium.Id.Api -project src/Annium.Id.Db -context Context

.PHONY: $(MAKECMDGOALS)
