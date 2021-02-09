FROM alpine:3.12 as watchdog

RUN apk --no-cache add curl \
    && echo "Pulling watchdog binary from Github." \
    && curl -sSL https://github.com/openfaas-incubator/of-watchdog/releases/download/0.8.1/of-watchdog > /usr/bin/fwatchdog \
    && chmod +x /usr/bin/fwatchdog \
    && cp /usr/bin/fwatchdog /home/app \
    && apk del curl --no-cache

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as builder

# Supress collection of data.
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# Optimize for Docker builder caching by adding projects first.

COPY ./Nuget.Config /root/.nuget/NuGet/NuGet.Config

RUN mkdir -p /home/app/src/function/Function
WORKDIR /home/app/src/function/Function
COPY ./function/Function/Function.csproj  .

RUN mkdir -p /home/app/src/function/Function.Tests
WORKDIR /home/app/src/function/Function.Tests
COPY ./function/Function.Tests/Function.Tests.csproj  .
RUN dotnet restore ./Function.Tests.csproj

WORKDIR /home/app/src/
COPY ./root.csproj  .
RUN dotnet restore ./root.csproj

COPY .  .

RUN dotnet test ./function/Function.Tests/Function.Tests.csproj

RUN dotnet publish -c release -o published

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine

COPY --from=watchdog /usr/bin/fwatchdog  /usr/bin/

# Create a non-root user
RUN addgroup --system app \
    && adduser --system --ingroup app app

WORKDIR /home/app/
COPY --from=builder /home/app/src/published .
RUN chown app:app -R /home/app

USER app

ENV fprocess="dotnet ./root.dll"
ENV cgi_headers="true"
ENV mode="http"
ENV upstream_url="http://localhost:5000"
EXPOSE 8080

HEALTHCHECK --interval=3s CMD [ -e /tmp/.lock ] || exit 1

CMD ["fwatchdog"]
