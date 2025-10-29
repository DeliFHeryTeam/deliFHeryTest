#!/bin/bash
set -e

# Check that token exists
if [ -z "$RUNNER_TOKEN" ]; then
  echo "RUNNER_TOKEN not set"
  exit 1
fi

echo "Configuring GitHub Actions Runner..."

# Use the official config.sh that exists in the base image
/home/runner/config.sh --unattended \
  --url "$RUNNER_REPO_URL" \
  --token "$RUNNER_TOKEN" \
  --name "$RUNNER_NAME" \
  --work "$RUNNER_WORKDIR" \
  --replace

echo "Runner configured. Starting..."
exec /home/runner/run.sh


