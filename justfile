set shell := ["bash", "-cu"]
set positional-arguments

project_name := "id"
tag_prefix := "registry.annium.com/" + project_name
tfm := "net9.0"
bin_debug := "bin/Debug/" + tfm

[private]
default:
    @just --list

# base

setup:
    @echo "=== $0 ==="
    dotnet tool restore

format:
    @echo "=== $0 ==="
    dotnet csharpier format .
    xs format -sc -ic

update:
    @echo "=== $0 ==="
    xs update all dotnet -sc -ic

clean:
    @echo "=== $0 ==="
    xs clean -sc -ic
    find . -type f -name '*.nupkg' | xargs rm

build buildNumber="0":
    @echo "=== $0 ==="
    dotnet build -c Release --nologo -v q -p:BuildNumber={{buildNumber}}

test:
    @echo "=== $0 ==="
    dotnet test -c Release --no-build --nologo -v q

pack:
    @echo "=== $0 ==="
    dotnet pack --no-build -o . -c Release -p:SymbolPackageFormat=snupkg

publish:
    @echo "=== $0 ==="
    dotnet nuget push "*.nupkg" --source https://api.nuget.org/v3/index.json --api-key $(cat .xs.credentials)
    find . -type f -name '*.nupkg' | xargs rm

# configuration

configure:
    #!/usr/bin/env bash
    set -e
    echo "=== configure ==="
    # host
    just _copy shared "application.yml email.yml" "run/server/configuration server/src/Server.Host/configuration"
    just _copy docker db.yml run/server/configuration
    just _copy local db.yml server/src/Server.Host/configuration
    just _copy shared "private.key public.key" "run/server/keys server/src/Server.Host/keys"
    # db
    just _copy docker db.env run/db
    # server tests
    just _copy shared "private.key public.key" server/tests/Server.IntegrationTests/keys
    # demo host
    just _copy shared "private.key public.key" server/tests/Server.DemoHost/keys
    # core tests
    just _copy shared "private.key public.key" lib/tests/Annium.Id.Core.Tests/keys

deconfigure:
    #!/usr/bin/env bash
    set -e
    echo "=== deconfigure ==="
    rm -rf run
    for pattern in /configuration/ /keys/; do
        git ls-files --others . | grep "$pattern" | xargs -r rm -f
    done

db-drop:
    @echo "=== $0 ==="
    docker-compose rm -vfs db
    docker volume rm -f id_db
    docker-compose up -d db

# run

start: start-server start-demo

start-server:
    @echo "=== $0 ==="
    @just _start-dotnet server/src Server.Host 9501

start-demo:
    @echo "=== $0 ==="
    @just _start-dotnet server/tests Server.DemoHost 9502

stop: stop-server stop-demo

stop-server:
    @echo "=== $0 ==="
    @just _stop-dotnet Server.Host

stop-demo:
    @echo "=== $0 ==="
    @just _stop-dotnet Server.DemoHost

# api generation

gwc-server:
    @echo "=== $0 ==="
    xrest cs gen \
        -s http://localhost:5000 \
        -o web/src/Site/Shared/Api/Server \
        -ns Site.Shared.Api.Server

gtc-server:
    @echo "=== $0 ==="
    xrest cs gen \
        -s http://localhost:5000 \
        -o server/tests/Server.Host.TestClient \
        -ns Server.Host.TestClient \
        -t

gtc-demo:
    @echo "=== $0 ==="
    xrest cs gen \
        -s http://localhost:5000 \
        -o server/tests/Server.DemoHost.TestClient \
        -ns Server.DemoHost.TestClient \
        -t

# publish

publish-all: publish-server publish-site

publish-server:
    @echo "=== $0 ==="
    @just _publish server . server/src/Server.Host/app.dockerfile

publish-site:
    @echo "=== $0 ==="
    @just _publish site . web/src/Site/app.dockerfile

# private helpers

_start-dotnet folder project port:
    cd {{folder}}/{{project}} && dotnet {{bin_debug}}/{{project}}.dll -port {{port}} &

_stop-dotnet project:
    ps -ax | grep {{project}}.dll | grep -v src | grep -v grep | sed -e 's#^ *##' | cut -d ' ' -f 1 | xargs -I% kill %

_copy source files dests:
    #!/usr/bin/env bash
    set -e
    for dir in {{dests}}; do
        mkdir -p "$dir"
        for file in {{files}}; do
            cp "cfg/{{source}}/$file" "$dir"
        done
    done

_publish image context dockerfile:
    docker build -t {{tag_prefix}}/{{image}} -f {{context}}/{{dockerfile}} {{context}}
    docker push {{tag_prefix}}/{{image}}
