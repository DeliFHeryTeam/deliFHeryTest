# Allgemein: 
Vorerst ergibt sich folgende Projektstruktur: 

```bash
├── README.md
├── backend/
└── frontend/
        /* Private */
```
Das Backend wird im Team entwickelt und das Frontend entwickelt jeder für
sich. Daher wird im `.gitignore` der Inhalt des directory `frontend` 
ausgeschlossen. 

```.gitignore
# Frontend 
frontend/**
!frontend/.gitkeep
```
# Projekterstellung
Um das Backend zu erstellen wurde wie folgt vorgegangen: 
+ Neue solution erstellen: `dotnet new sln -n DeliFHery`

| Schicht | Projektname | CLI-Befehl| Hinzufügen zur Solution |
| -------|   ----       |-----------|-------------------------|
| Präsentation (API)    | DeliFHery.Api  | dotnet new webapi -n DeliFHery.Api --framework net9.0    | dotnet sln DeliFHery.sln add DeliFHery.Api/DeliFHery.Api.csproj   |
| Geschäftslogik (Core) | DeliFHery.Core | dotnet new classlib -n DeliFHery.Core --framework net9.0 | dotnet sln DeliFHery.sln add DeliFHery.Core/DeliFHery.Core.csproj |
| Datenzugriff (DAL)    | DeliFHery.Data | dotnet new classlib -n DeliFHery.Data --framework net9.0 | dotnet sln DeliFHery.sln add DeliFHery.Data/DeliFHery.Data.csproj |# Docker 


Testprojekt hinzufügen: 
```bash
dotnet new xunit -n DeliFHery.Data.Tests --framework net9.0
dotnet sln DeliFHery.sln add DeliFHery.Data.Tests/DeliFHery.Data.Tests.csproj```

Nun sieht die Struktur wie folgt aus: 
```bash 
.
├── backend
│   ├── DeliFHery.Api/
│   ├── DeliFHery.Core/
│   └── DeliFHery.Data/
│   └── DeliFHery.Data.Tests/
└── frontend /* privat, optional */
    └── deliFHeryApp/
```
Projektabhängigkeiten festlegen: 

```bash
# API -> Core
dotnet add DeliFHery.Api/DeliFHery.Api.csproj reference DeliFHery.Core/DeliFHery.Core.csproj
# Data -> Core
dotnet add DeliFHery.Data/DeliFHery.Data.csproj reference DeliFHery.Core/DeliFHery.Core.csproj
# Data.Tests -> Data
dotnet add DeliFHery.Data.Tests/DeliFHery.Data.Tests.csproj reference DeliFHery.Data/DeliFHery.Data.csproj
```


Es sind für das Projekt folgende Container nötig: 
+ Datenbank : `mysql:8.0`
+ Backend : `mcr.microsoft.com/dotnet/runtime:9.0`
            `mcr.microsoft.com/dotnet/sdk:9.0`
+ Frontend: `node:20-alpine`

+ TODO: Zahlungsservice

# Docker: 
Files anlegen: 
```bash 
.
├── README.md
├── backend/
│   ├── DeliFHery.Api/
│   ├── DeliFHery.Core/
│   ├── DeliFHery.Data/
│   ├── DeliFHery.Data.Tests/
│   ├── DeliFHery.sln
│   └── Dockerfile      <--- hier
├── docker-compose.yaml <--- hier
└── frontend/ # optional
    ├── Dockerfile      <--- hier
    └── deliFHeryApp
```


### Docker Compose: 
***DB***
```yaml
  db:
    image: mysql:8.0
    container_name: delifhery_db
    environment:
      MYSQL_ROOT_PASSWORD: ${DB_ROOT_PW}
      MYSQL_DATABASE: delifhery
      MYSQL_USER: ${DB_USER}
      MYSQL_PASSWORD: ${DB_USER_PW}
    ports:
      - "3306:3306"
    volumes:
      - ./db:/var/lib/mysql # Store data locally
    networks:
      - delifhery-net
    healthcheck:
      test: ["CMD", "mysqladmin" ,"ping", "-h", "localhost"]
      timeout: 10s
      retries: 5
```
***Backend***
```yaml
  backend:
    build: 
      context: ./backend
      dockerfile: Dockerfile
    container_name: delifhery_api
    restart: always
    environment:
      ASPNETCORE_URLS: "http://+:8080"
      ConnectionStrings__DefaultConnection: >
        Server=db;
        Database=delifhery;
        User=${DB_USER};
        Password=${DB_USER_PW};
    depends_on:
      db:
        condition: service_healthy # Health check from db
    ports:
      - "8080:8080"
    networks:
      - delifhery-net

```
***Frontend***
```yaml
  frontend:
    build:
      context: ./frontend/deliFHeryApp
      dockerfile: Dockerfile
    container_name: delifhery_frontend
    ports:
      - "4200:80"
    environment:
      - API_BASE_URL=http://backend:8080
    networks:
      - delifhery-net
```
***Netzwerk***
```yaml
networks:
  delifhery-net:
    driver: bridge

```

### Dockerfiles: 

***Backend***
```Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore DeliFHery.sln
RUN dotnet test DeliFHery.Data.Tests/DeliFHery.Data.Tests.csproj --no-restore --verbosity normal
RUN dotnet publish DeliFHery.Api/DeliFHery.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "DeliFHery.Api.dll"]

```

***Frontend(optional)***
```bash
FROM node:20-alpine AS builder
WORKDIR /app

COPY package.json ./
RUN npm install

COPY . .

RUN npm run build

FROM nginx:alpine
WORKDIR /usr/share/nginx/html

COPY --from=builder /app/dist/deliFHeryApp /usr/share/nginx/html

COPY nginx-custom.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
```

Wichtig in frontend/deliFHeryApp: 
Erstelle `frontend/deliFHeryApp/nginx-custom.conf` und darin: 
```conf
server {
    listen 80;
    server_name localhost;

    root /usr/share/nginx/html;
    index index.html;

    # Wichtig: Leitet alle nicht gefundenen Anfragen an index.html um (für Angular Router)
    location / {
        try_files $uri $uri/ /index.html;
    }
}
```
um alle nicht gefundenen Anfragen an index.html umzuleiten (für Angular Router)

# Private Runner: 
Repo initialisieren und remote hinzufügen: 
```bash
git init
git add .
git commit -m "finish config "
git remote add origin git@github.com:{u_name}/{reponame}
git push --set-upstream origin main
```

Erweitern von `docker-compose.yml`
```yml
  runner:
    image: myoung34/github-runner:latest # Ein beliebtes Community-Image
    container_name: delifhery_runner
    restart: unless-stopped
    # Wichtig: Der Runner muss die Docker-Engine des Hosts verwenden, 
    # um Docker-Befehle (z.B. das Bauen des Backend-Containers) auszuführen.
    volumes:
      - /var/run/docker.sock:/var/run/docker.sock 
    environment:
      # Registrierungsinformationen, gelesen aus der .env Datei
      REPO_URL: https://github.com/${GH_OWNER}/${GH_REPO}
      ACCESS_TOKEN: ${GH_PAT}
      RUNNER_NAME: ${GH_RUNNER_NAME}
      RUNNER_WORKDIR: /home/docker/github-actions-runner # Arbeitsverzeichnis im Container
      LABELS: self-hosted,linux,x64,docker-compose # Wichtige Labels für das Workflow-Targeting
    networks:
      - delifhery-net
  db:
```
Wichtig alle Variablen aus der `.env`-Datei, die die Datenbank betreffen müssen auf Github als Secret hinzugefügt werden.  

### Worflow: 

```yml
name: CI/CD Pipeline

on:
  push:
    branches:
      - main
  pull_request:
    branches:
      - main

jobs:
  build-and-test:
    name: Build, Test, and Deploy Backend
    runs-on: [self-hosted, docker-compose] 

    steps:
      # --------------------------------------------------
      # --- 1. CODE-CHECKOUT ---
      # --------------------------------------------------
      - name: Checkout Repository
        uses: actions/checkout@v4
        
      # --------------------------------------------------
      # --- 2. BUILD UND TEST (Kern der CI-Pipeline) ---
      # --------------------------------------------------
      - name: Build and Test Backend (via Dockerfile)
        id: build_api
        run: |
          docker build \
            ./backend \
            -f ./backend/Dockerfile \
            --tag delifhery-api:latest \
            --no-cache 
            
      # --------------------------------------------------
      # --- 3. DEPLOYMENT (Auf dem lokalen Docker Host) ---
      # --------------------------------------------------
      - name: Deploy (Restart) Backend Container
        run: docker compose up -d --no-deps --force-recreate backend

        env:

          DB_USER: ${{ secrets.DB_USER }}
          DB_USER_PW: ${{ secrets.DB_USER_PW }}
          DB_ROOT_PW: ${{ secrets.DB_ROOT_PW }}
```

