#!/usr/bin/env bash
read -p "Migration name? " migration_name
dotnet ef database update $migration_name \
    --startup-project src/Annium.Id \
    --project src/Annium.Id.Db \
    --context Context \
    --no-build