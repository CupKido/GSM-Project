#!/bin/bash
set -e

# Execute the migration command
dotnet ef database update

# Start the application
exec "$@"