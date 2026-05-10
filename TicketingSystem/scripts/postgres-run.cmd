@echo off

REM === Check for container runtime
set _command=docker
where /q %_command%
if errorlevel 1 (
  set _command=podman
)
echo Runtime to be used: %_command%
REM ==============

echo Starting database

%_command% run --name local_pg ^
  --rm ^
  -e POSTGRES_PASSWORD="local_pg_pass" ^
  -e POSTGRES_DB="ticketing-cache" ^
  -p 5433:5432 ^
  -d postgres  