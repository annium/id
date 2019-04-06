#!/usr/bin/env bash
dotnet ef migrations list \
    --startup-project src/Annium.IdentityServer \
    --project src/Annium.IdentityServer \
    --context Context \
    --no-build