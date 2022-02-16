
FROM mcr.microsoft.com/dotnet/sdk:5.0
COPY . /app
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Production
RUN ["dotnet", "restore"]
RUN ["dotnet", "build"]
RUN dotnet tool install --global dotnet-ef
EXPOSE 5000/tcp
RUN chmod +x ./Setup.sh
CMD  ./Setup.sh



 