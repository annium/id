#!/usr/bin/env bash
dotnet ef database drop -f \
    --startup-project src/Annium.IdentityServer \
    --project src/Annium.IdentityServer.Db \
    --context Context \
    --no-build