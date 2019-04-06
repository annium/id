#!/usr/bin/env bash
dotnet ef migrations remove \
    --startup-project src/Annium.IdentityServer \
    --project src/Annium.IdentityServer.Db \
    --context Context \
    --no-build