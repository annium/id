#!/usr/bin/env bash
dotnet ef migrations remove \
    --startup-project src/Annium.Id.Api \
    --project src/Annium.Id.Db \
    --context Context \
    --no-build