

FROM ghcr.io/actions/actions-runner:latest

# Install Docker inside the runner container to allow building images
USER root
RUN apt-get update && \
    apt-get install -y docker.io && \
    apt-get clean

# Optional: Install Node, .NET SDK, Angular, etc. später je nach Bedarf






