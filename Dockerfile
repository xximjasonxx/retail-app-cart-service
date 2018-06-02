FROM microsoft/aspnetcore-build:2.0 as build-env
WORKDIR /code

COPY *.csproj ./
RUN dotnet restore

COPY . ./
ENV ASPNETCORE_ENVIRONMENT=Development
RUN dotnet publish -c Debug -o ./artifact

FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /code/artifact .

ENTRYPOINT [ "dotnet", "CartApi.dll" ]