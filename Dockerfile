# ---------- build ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj(s) and restore (speeds rebuilds)
COPY *.sln .
COPY EmployeePayslipApp/EmployeePayslipApp.csproj EmployeePayslipApp/
# copy other projects/nuget info if needed
RUN dotnet restore

# copy everything and publish
COPY . .
RUN dotnet publish EmployeePayslipApp/EmployeePayslipApp.csproj -c Release -o /app

# ---------- runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "EmployeePayslipApp.dll"]
