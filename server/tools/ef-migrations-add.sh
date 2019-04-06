#!/usr/bin/env bash
read -p "Migration name? " migration_name
dotnet ef migrations add $migration_name \
    --startup-project src/Annium.IdentityServer \
    --project src/Annium.IdentityServer \
    --context Context \
    --no-build