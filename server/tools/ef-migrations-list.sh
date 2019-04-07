#!/usr/bin/env bash
dotnet ef migrations list \
    --startup-project src/Annium.Id \
    --project src/Annium.Id.Db \
    --context Context \
    --no-build