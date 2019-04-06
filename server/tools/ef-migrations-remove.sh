#!/usr/bin/env bash
dotnet ef migrations remove \
    --startup-project src/Annium.IdentityServer \
    --project src/Annium.IdentityServer \
    --context Context \
    --no-build