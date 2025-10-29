FROM ghcr.io/actions/actions-runner:latest

USER root
RUN apt-get update && \
    apt-get install -y docker.io && \
    apt-get clean

# Copy start script
COPY start.sh /actions-runner/start.sh
RUN chmod +x /actions-runner/start.sh

USER runner
WORKDIR /actions-runner
ENTRYPOINT ["/actions-runner/start.sh"]


