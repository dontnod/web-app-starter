ARG DOTNET_BUILDER_IMAGE=8.0
ARG DOTNET_IMAGE_FROM=mcr.microsoft.com/dotnet/sdk
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
COPY WebAppStarter.sln README.md ./

# Install Client Dependencies
RUN cd client && npm ci

# Install Dot net Dependencies
RUN dotnet restore
RUN cd service/DNE.Todo.API && dotnet tool restore

RUN dotnet tool install --global dotnet-ef

## ---------------------------------------------------------------------------------- ##
## ----------------------------------- prod build ----------------------------------- ##
## ---------------------------------------------------------------------------------- ##
FROM build-base-with-dependencies as prod-build

ARG VITE_AZURE_CLIENT_ID
ARG VITE_AZURE_AUTHORITY
ARG VITE_TODO_API_ENDPOINT_URL
ARG VITE_TODO_API_SCOPES_READ
ARG VITE_TODO_API_SCOPES_WRITE

WORKDIR /workspace

ENV VITE_AZURE_CLIENT_ID=${VITE_AZURE_CLIENT_ID}
ENV VITE_AZURE_AUTHORITY=${VITE_AZURE_AUTHORITY}
ENV VITE_TODO_API_ENDPOINT_URL=${VITE_TODO_API_ENDPOINT_URL}
ENV VITE_TODO_API_SCOPES_READ=${VITE_TODO_API_SCOPES_READ}
ENV VITE_TODO_API_SCOPES_WRITE=${VITE_TODO_API_SCOPES_WRITE}

# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as prod-image

WORKDIR /app

COPY --from=prod-build /workspace/out .

EXPOSE 8080
ENTRYPOINT ["dotnet", "DNE.Todo.API.dll"]

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
