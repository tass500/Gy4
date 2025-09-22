# Logisztikai Rendszer

Ez egy modern, mikroszolgáltatás-alapú logisztikai rendszer, amely a következő főbb komponensekből áll. A rendszer fejlesztése során kiemelt hangsúlyt fektetünk a karbantarthatóságra, tesztelhetőségre és CI/CD folyamatokra.

## Főbb technológiák

- **Frontend**: 
  - TypeScript, Angular 19+
  - NgRx állapotkezelés
  - i18next lokalizáció
  - OWASP biztonsági irányelvek
  
- **Backend**: 
  - .NET 8.0+ (C# 10+)
  - Entity Framework Core 8.0+
  - Dapper nagy teljesítményű lekérdezésekhez
  - MediatR CQRS implementációhoz
  - FluentValidation
  
- **Adatbázis**: 
  - MS SQL Server 2022 (elsődleges)
  - Támogatott alternatívák: PostgreSQL, MySQL
  - TDE (Transparent Data Encryption)
  - Always Encrypted bizalmas adatokhoz
  
- **Üzenetküldés és Stream feldolgozás**:
  - Apache Kafka üzenetsorokhoz
  - Apache Flink valós idejű feldolgozáshoz
  - TLS 1.2+ titkosítás a kommunikációhoz
  
- **Konténerizáció és Orchestráció**:
  - Docker konténerek
  - Kubernetes fürtök
  - Helm chartok telepítéshez
  
- **Monitorozás és Naplózás**:
  - Prometheus + Grafana metrikákhoz
  - ELK Stack (Elasticsearch, Logstash, Kibana) naplókhoz
  - Azure Application Insights / AWS CloudWatch integráció

## Architekturális elvek

- **Mikroszolgáltatás architektúra** különálló, skálázható szolgáltatásokkal
- **Több-bérlős (Multi-tenant) tervezés** adatszigorló elkülönítéssel
- **Event-driven design** aszinkron kommunikációval
- **Domain-Driven Design (DDD)** a komplex üzleti logika modellezéséhez
- **CQRS és Event Sourcing** kiválasztott modulokban
- **SaaS készenlét** bérlői önkiszolgáló felülettel
- **Titkosítás** minden érzékeny adathoz
- **Többnyelvűség (i18n) és akadálymentesítés (a11y)** teljes körű támogatása
- **CI/CD folyamatok** Azure DevOps-alapú teljes körű automatizálással

## Projektstruktúra

```
src/
├── api-gateway/         # API Gateway (Ocelot)
├── services/
│   ├── tenant-service/  # Bérlői szolgáltatás
│   │   ├── src/         # Forráskód
│   │   └── tests/       # Tesztek
│   ├── logistics-service/ # Logisztikai alapszolgáltatás
│   ├── warehouse-service/ # Raktárkezelő szolgáltatás
│   ├── shipping-service/  # Szállítási szolgáltatás
│   └── billing-service/   # Számlázási szolgáltatás
├── frontend/           # Angular alapú felhasználói felület
├── infra/              # Infrastruktúra kód (Docker, Kubernetes)
└── scripts/            # Segédszkriptek
```

## Biztonság

- **Hitelesítés**:
  - JWT token alapú hitelesítés
  - OAuth 2.0 és OpenID Connect támogatás
  - Többfaktoros hitelesítés (MFA)
  
- **Adatvédelem**:
  - Azure Key Vault / AWS KMS a titkok kezeléséhez
  - Minden érzékeny adat titkosítva tárolva
  - Személyes adatok védelme (GDPR)
  
- **Biztonsági intézkedések**:
  - OWASP Top 10 elleni védelem
  - Rendszeres biztonsági auditok
  - Automatizált sebezhetőségi vizsgálatok

## CI/CD Folyamat

- **Folyamatos Integráció (CI)**:
  - Automatikus build és egységtesztek minden kódbázis változtatásnál
  - Kódminőség ellenőrzés (SonarQube)
  - Biztonsági szkennelések (OWASP ZAP, WhiteSource)
  
- **Folyamatos Szállítás/Telepítés (CD)**:
  - Automatikus deployment különböző környezetekbe (dev, test, staging, production)
  - Blue-Green vagy Canary deployment stratégiák
  - Automatikus visszaállítás hibás verziók esetén
  
- **Monitorozás és Jelentés**:
  - Build és release pipeline-ok állapotának nyomon követése
  - Tesztlefedettség és minőségmérőszámok
  - Üzemeltetési metrikák és riasztások

## Fejlesztői környezet beállítása

### Előfeltételek

- **Fejlesztői eszközök**:
  - .NET 8.0+ SDK
  - Node.js 18+ és npm 9+
  - Angular CLI 19+
  - Git
  - Docker Desktop (opcionális, konténeres fejlesztéshez)
  - Azure CLI (opcionális, Azure-erőforrások kezeléséhez)
  - Kubernetes CLI (kubectl) (opcionális, Kubernetes-hez)

- **Ajánlott fejlesztői környezetek**:
  - Visual Studio 2022 (Windows)
  - Visual Studio Code (keresztplatformos)
  - JetBrains Rider (keresztplatformos)
  - Azure Data Studio (adatbázis-kezeléshez)

### Telepítés

1. Klónozd le a repository-t:
   ```bash
   git clone <repository-url>
   cd Gy4
   ```

2. Frontend függőségek telepítése:
   ```bash
   cd src/frontend
   npm install
   ```

3. Backend függőségek visszaállítása (amint a hálózati kapcsolat rendben van):
   ```bash
   cd ../..
   dotnet restore
   ```

### Futtatás

1. Indítsd el a backend szolgáltatásokat:
   ```bash
   # Minden szolgáltatás indítása (fejlesztési módban)
   dotnet run --project src/api-gateway
   ```

2. Indítsd el a frontend fejlesztői szerverét:
   ```bash
   cd src/frontend
   ng serve
   ```

3. Nyisd meg a böngészőben: http://localhost:4200

## Fejlesztés

### Új szolgáltatás hozzáadása

1. Hozz létre egy új könyvtárat a `src/services` mappában
2. Hozz létre egy új .NET WebAPI projektet a könyvtárban:
   ```bash
   dotnet new webapi -n Logistics.ModuleName.Service
   ```
3. Állítsd be a projektet:
   - Adjon hozzá referenciát a `Logistics.Core` projekthez
   - Konfigurálja a függőséginjektálást
   - Adja hozzá a szükséges NuGet csomagokat
   - Állítsa be a naplózást és metrikákat

4. Konfiguráld a szolgáltatást az API Gateway-ben:
   - Adj hozzá egy új útvonalt a `ocelot.json` fájlban
   - Állítsd be a szolgáltatás címét és portját
   - Konfiguráld a hitelesítést és engedélyezést

### Kódolási szabályok és irányelvek

- **Dokumentáció**:
  - Minden nyilvános osztálynak, interfésznek és metódusnak legyen XML dokumentációja
  - Használj értelmes, leíró neveket angol nyelven
  - Minden kommentet angol nyelven írj

- **Kódminőség**:
  - Kövesd a Clean Code elveit
  - Használj konzisztens formázást (editorconfig segítségével)
  - Tartsd be a SOLID elveket
  - Írj teszteket minden új funkcióhoz (min. 80% kódlefedettség)

- **Verzióközlés**:
  - Használj szemantikus verziószámozást (SemVer)
  - Minden változtatáshoz írj egyértelmű commit üzenetet
  - Használj feature brancheket a fejlesztéshez
  - Minden kódmódosításhoz készüljön pull request

- **Biztonság**:
  - Mindig validáld a bemeneti adatokat
  - Használj paraméterezett lekérdezéseket az adatbázis műveletekhez
  - Ne logolj érzékeny adatokat
  - Kövesd az OWASP biztonsági irányelveit

## Következő lépések

- [ ] API Gateway konfigurálása
- [ ] Bérlői szolgáltatás implementálása
- [ ] Felhasználói hitelesítés és engedélyezés
- [ ] Raktárkezelő modul fejlesztése
- [ ] Szállítási modul fejlesztése
- [ ] Számlázási modul fejlesztése
- [ ] Integrációs tesztek írása
- [ ] CI/CD folyamat beállítása
