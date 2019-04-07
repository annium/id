#!/usr/bin/env bash
dotnet ef database drop -f \
    --startup-project src/Annium.Id \
    --project src/Annium.Id.Db \
    --context Context \
    --no-build