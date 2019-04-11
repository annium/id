#!/usr/bin/env bash
read -p "Migration name? " migration_name
dotnet ef migrations add $migration_name \
    --startup-project src/Annium.Id.Api \
    --project src/Annium.Id.Db \
    --context Context \
    --no-build