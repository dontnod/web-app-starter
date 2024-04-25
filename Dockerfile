ARG DOTNET_BUILDER_IMAGE=8.0
ARG DOTNET_IMAGE_FROM=mcr.microsoft.com/dotnet/aspnet
ARG NODE_VERSION=none

## ---------------------------------------------------------------------------------- ##
## -------------------------- Image base -------------------------------------------- ##
## ---------------------------------------------------------------------------------- ##
FROM ${DOTNET_IMAGE_FROM}:${DOTNET_BUILDER_IMAGE} as build-base
RUN apt-get update \
  && apt-get upgrade -y \
  && apt-get install -y --no-install-recommends git curl \
  && apt-get autoremove -y \
  && apt-get clean -y \
  && rm -rf /root/.cache \
  && rm -rf /var/apt/lists/* \
  && rm -rf /var/cache/apt/* \
  && apt-get purge -y --auto-remove -o APT::AutoRemove::RecommendsImportant=false\
  && mkdir -p /workspace

# Conditionally install Node.js based on the build argument
RUN if [ "$NODE_VERSION" != "none" ]; then \
  curl -fsSL https://deb.nodesource.com/setup_20.x | bash - && \
  apt-get install -y nodejs; \
  fi

## ---------------------------------------------------------------------------------- ##
## --------------------------- build base with dependencies ------------------------- ##
## ---------------------------------------------------------------------------------- ##
FROM build-base as build-base-with-dependencies

WORKDIR /workspace

# Copy source
COPY client/ client/
COPY service/ service/
COPY web-app-starter.sln README.md ./

# Install Client Dependencies
RUN cd client && yarn install

# Install Dot net Dependencies
RUN dotnet restore
RUN cd service/DNE.Todo.API && dotnet tool restore

RUN dotnet tool install --global dotnet-ef

## ---------------------------------------------------------------------------------- ##
## ----------------------------------- prod build ----------------------------------- ##
## ---------------------------------------------------------------------------------- ##
FROM build-base-with-dependencies as prod-image

WORKDIR /workspace

STOPSIGNAL SIGINT
EXPOSE 5197 7252
VOLUME /workspace

## ---------------------------------------------------------------------------------- ##
## -------------------------------- development build ------------------------------- ##
## ---------------------------------------------------------------------------------- ##
FROM build-base-with-dependencies as dev-image

WORKDIR /workspace

# Setup dev https certificates
RUN dotnet dev-certs https

STOPSIGNAL SIGINT
EXPOSE 5197 7252
VOLUME /workspace
