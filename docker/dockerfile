FROM ubuntu:latest

# Install curl
RUN apt-get update && apt-get install -y curl

# Add script to container
COPY apirun.sh /apirun.sh
RUN chmod +x /apirun.sh

# Run script indefinitely
CMD ["/bin/bash", "-c", "/apirun.sh"]
