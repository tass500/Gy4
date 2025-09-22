# Kódolási Követelmény Dokumentum: Modern Reszponzív Webes Alkalmazás Fejlesztése

## Áttekintés
Ez a dokumentum meghatározza a követelményeket egy modern, reszponzív webes alkalmazás fejlesztésére, amely mikroszerviz architektúrára épül. A frontend natív JavaScript, HTML és CSS használatával készül, a backend pedig C# .NET alapokon. A fejlesztés során kiemelt hangsúlyt kell fektetni a karbantarthatóságra, tesztelhetőségre és CI/CD folyamatokra. Az adatok tárolása MS SQL Server 2022-ben történik, az alkalmazás támogatja a többnyelvűséget (internationalization - i18n), és teljes akadálymentesítéssel (accessibility - a11y) rendelkezik. Minden kódgenerálás során teszteket kell létrehozni és futtatni, amíg sikeresek nem lesznek, és minden commit előtt az összes tesztet futtatni kell. Minden kódgenerálás mobile-first megközelítéssel történik, prioritizálva a mobil eszközökre optimalizálást.

## 1. Architektúra és Szerkezet
- **Több-bérlős (Multi-tenant) Tervezési Minta**: Az alkalmazás több bérlőt támogasson egyetlen alkalmazáspéldányon belül, ahol minden bérlő adatai logikailag el vannak különítve. A megvalósítás során a következő megközelítések egyikét vagy kombinációját alkalmazzuk:
  - **Adatbázis szintű elválasztás**: Minden bérlő számára külön adatbázis vagy sémakezelés (database/schema per tenant)
  - **Soronkénti bérlő azonosítás**: Minden táblában legyen egy `TenantId` oszlop a bérlők elkülönítéséhez (soft multi-tenancy)
  - **Hybrid megoldás**: Kritikus adatok külön adatbázisban, más adatok bérlőnkénti szűréssel
- **Mikroszerviz Alapú Felépítés**: Az alkalmazás különálló, egymástól független mikroszervizekből álljon, amelyek önállóan telepíthetőek és skálázhatóak. Minden mikroszerviz saját felelősségi körrel rendelkezzen (pl. frontend UI, backend API, adatbázis szolgáltatások, lokalizáció szolgáltatás, akadálymentesítés szolgáltatás).
- **Elválasztás Frontend és Backend Között**: 
  - **Frontend**: TypeScript és Angular keretrendszer használata komponens alapú fejlesztéshez
  - **Backend**: C# .NET szolgáltatások
  A szolgáltatások külön mikroszervizekben legyenek elhelyezve, RESTful API-kon keresztül kommunikálva egymással.
- **Moduláris Kód**: Minden komponens moduláris legyen, hogy könnyen újrafelhasználható és cserélhető legyen. Használjon dependency injection-t a .NET backendben és modulrendszert a JavaScriptben.
- **Adattárolás**: Az adatok tárolása elsődlegesen MS SQL Server 2022 adatbázisban történik, de az architektúra lehetővé teszi más adatbázisok (pl. PostgreSQL, MySQL) használatát is. Minden mikroszerviz saját adatbázis példányt használjon (database-per-service minta) a függetlenség biztosításához. Használjon Entity Framework Core (EF Core) a .NET backendben az adatbázis eléréséhez és migrációkhoz, repository mintával az adatelérési réteg kiszervezésével. A lokalizált és akadálymentes adatok (pl. alt text, ARIA címkék) külön táblákban vagy JSON oszlopokban tárolódjanak.

## 2. Karbantarthatóság
- **Tiszta Kód Elvek**: Kövesse a SOLID elveket (Single Responsibility, Open-Closed, Liskov Substitution, Interface Segregation, Dependency Inversion).
- **Konstansok és Enumok Használata**: 
  - Minden mágikus számot, szöveget vagy ismétlődő értéket helyezzen el megfelelő konstansként vagy enumként.
  - Használjon típusbiztos enumokat a string literálok helyett (pl. `TextAlignment.LEFT` a `'left'` helyett).
  - Csoportosítsa a kapcsolódó konstansokat logikus egységekbe (pl. `GRID_DEFAULTS`, `API_CONSTANTS`).
  - A konfigurációs értékeket (pl. URL-ek, időkorlátok, oldalméretek) mindig külső konfigurációból vagy konstansokból töltsük be.
  - A hibaüzeneteket és felhasználói üzeneteket is helyezze el központi konstansokban vagy i18n fájlokban.
  - Használjon `readonly` vagy `const` kulcsszavakat a konstansok deklarálásakor a véletlen módosítás megakadályozására.
  - Dokumentálja az összes konstans és enum használatát, különös tekintettel a mérnöki számokra és a várt értéktartományokra.
- **Kódolási Szabványok**:
  - **Frontend**: 
    - **TypeScript** szigorú típusossággal (strict mode)
    - **Angular** keretrendszer legfrissebb stabil verziója
    - **Angular CLI** projekt kezeléshez és build folyamathoz
    - **Angular Material** komponensek a konzisztens UI-hoz
    - **RxJS** reaktív programozáshoz és aszinkron műveletek kezeléséhez
    - **NgRx** állapotkezeléshez (nagyobb alkalmazások esetén)
    - **SCSS** stílusozáshoz, BEM módszertannal
    - **Angular i18n** beépített lokalizációs támogatás
    - Akadálymentesítés: ARIA címkék, billentyűzet navigáció, színkontraszt (legalább 4.5:1), alt text minden képhez
    - **ESLint** és **Prettier** kódformázáshoz és minőségbiztosításhoz
  - **Backend**: C# .NET Core/6+ , aszinkron programozási minták (async/await), LINQ lekérdezések optimalizálása. Entity Framework Core használata adatbázis műveletekhez, biztosítva a típusbiztonságot és a migrációk kezelését. .NET beépített lokalizáció (IStringLocalizer) és akadálymentes API-k (pl. JSON-LD strukturált adatok).
- **Dokumentáció**: Minden függvény, osztály és modul legyen dokumentálva (pl. XML comments C#-ban, JSDoc JavaScriptben). Tartson fenn egy README.md fájlt a projekt áttekintésével, beleértve az adatbázis sémát, migrációk, lokalizáció fájlok és akadálymentesítési útmutatót.
- **Verziókezelés**: 
  - Használjon Git-et feature branch stratégiával (pl. Git Flow). 
  - Minden commit legyen értelmes és követhető, beleértve az adatbázis migrációkat, lokalizáció frissítéseket és a11y javításokat.
  - **Ne commitoljunk csomagokat a verziókezelő rendszerbe** (pl. node_modules, bin, obj, packages mappák).
  - A .gitignore fájlban legyenek megfelelő bejegyzések a következőkhöz:
    - Node.js: `node_modules/`, `.npm`
    - .NET: `bin/`, `obj/`, `*.user`, `*.suo`, `.vs/`
    - IDE specifikus fájlok (pl. `.idea/`, `.vscode/` kivéve a szükséges munkakörnyezeti beállításokat)
    - Környezeti változókat tartalmazó fájlok (pl. `.env`, `appsettings.Development.json`)
  - Minden fejlesztő a saját gépekre telepítse a szükséges csomagokat a projekt gyökérkönyvtárában található parancsokkal:
    ```bash
    # Frontend csomagok telepítése
    cd frontend
    npm install
    
    # Backend csomagok visszaállítása
    cd ../backend
    dotnet restore
    ```
- **Refaktorálás**: Rendszeresen refaktorálja a kódot, hogy elkerülje a technikai adósságot, különösen az adatbázis réteget, lokalizáció logikát és akadálymentesítést.

## 3. Tesztelhetőség
- **Teszt Típusok**:
  - **Unit Tesztek**: Minden logikai egység (függvény, osztály) tesztelve legyen. Frontend: Jest vagy Mocha, beleértve az i18n és a11y funkciókat; Backend: xUnit vagy NUnit, beleértve az EF Core repository-kat, szolgáltatásokat, lokalizációt és a11y API-kat.
  - **Integrációs Tesztek**: Szolgáltatások közötti interakciók tesztelése, beleértve az adatbázis interakciókat, lokalizált tartalmakat és akadálymentes elemeket. Használjon in-memory adatbázist (pl. SQL Server in-memory) a tesztekhez.
  - **E2E Tesztek**: Teljes felhasználói folyamatok tesztelése (pl. Selenium, Cypress frontendhez), amelyek az adatbázis réteget, lokalizációt és akadálymentesítést is érintik.
  - **Teljesítmény Tesztek**: Load testing a skálázhatóság biztosításához, különösen az adatbázis lekérdezésekhez, lokalizált tartalom betöltéséhez és a11y elemekhez.
- **Teszt Fedettség**: Legalább 80% kódlefedettség unit tesztekkel, beleértve az adatbázis logikát, i18n és a11y funkciókat.
- **Automatizált Teszt Futtatás**: Minden commit/pull request előtt futtassa a teszteket, beleértve az adatbázis migrációk, lokalizáció és a11y tesztelését. Minden kódgenerálás során hozzon létre teszteket a generált kódhoz, és futtassa őket, amíg sikeresek nem lesznek.
- **Tesztelési Eljárás**: Minden új kódgeneráláskor (pl. függvény, osztály, komponens) automatikusan generáljon unit és integrációs teszteket. Futtassa őket helyi környezetben, és javítson minden hibát, amíg a tesztek nem sikeresek. Minden commit előtt futtassa az összes tesztet (unit, integrációs, E2E, teljesítmény), és csak akkor engedje a commit-ot, ha minden teszt sikeres.

## 4. CI/CD Folyamatok
- **Continuous Integration (CI)**:
  - Automatizált build minden commitra (pl. GitHub Actions, Azure DevOps).
  - Függőségek telepítése minden build során:
    ```yaml
    # Példa GitHub Actions workflow részlet
    - name: Install dependencies
      run: |
        cd frontend
        npm ci # Clean install a package-lock.json alapján
        cd ../backend
        dotnet restore
    ```
  - Kódminőség ellenőrzés: Linting (ESLint JS-hez, StyleCop C#-hoz), statikus analízis (SonarQube), beleértve az adatbázis sémát, lokalizáció fájlokat és a11y ellenőrzéseket.
  - Automatizált teszt futtatás minden build során, beleértve az adatbázis teszteket, i18n validációt és a11y scannereket (pl. axe-core). Futtassa az összes tesztet minden commit előtt, és blokkolja a build-et, ha bármelyik teszt sikertelen.
- **Continuous Deployment (CD)**:
  - Automatizált deploy staging és production környezetekbe sikeres build és teszt után.
  - Használjon containerizálást (Docker) a mikroszervizekhez, beleértve az adatbázis konténereket, lokalizáció fájlokat és a11y eszközöket.
  - Környezet-specifikus konfigurációk (pl. appsettings.json .NET-hez, environment variables JS-hez), beleértve a connection string-eket MS SQL Server 2022-höz, lokalizáció beállításokat és a11y konfigurációkat.
  - Rollback mechanizmus deploy hibák esetén, beleértve az adatbázis rollback-ot, lokalizáció frissítéseket és a11y javításokat.
  - Adatbázis migrációk, lokalizáció fájlok és a11y eszközök automatizálása: EF Core migrációk, i18n fájlok szinkronizálása és a11y scannerek futtatása a deploy során.
- **Monitoring és Logging**: Integrálja a logging-et (pl. Serilog .NET-hez, Winston JS-hez) és monitoring-ot (pl. Application Insights, Prometheus) a teljesítmény és hibák nyomon követéséhez, beleértve az adatbázis teljesítményt, lokalizált tartalom betöltését és a11y problémákat.

## 5. Reszponzivitás és Felhasználói Élmény
- **Reszponzív Design**: 
  - Használjon mobil-először (mobile-first) megközelítést
  - Angular Flex Layout modul használata a rugalmas elrendezésekhez
  - Reszponzív breakpoint-ok: XSmall (<600px), Small (600-959px), Medium (960-1279px), Large (1280-1919px), XLarge (≥1920px)
  - Angular CDK Layout modul használata a reszponzív viselkedések kezeléséhez
  - Képarányfüggő elrendezések (AspectRatio directive)
  - Print stíluslapok nyomtatáshoz
  - Progresszív fejlesztés: alapfunkciók minden eszközön, fejlettebb funkciók nagyobb képernyőkön
- **Akadálymentesítés (Accessibility - a11y)**: Kövesse a WCAG 2.1 AA szintet. Implementálja: ARIA címkék, billentyűzet navigáció (tab order), színkontraszt (4.5:1 minimum), alt text képekhez, focus indikátorok, képernyőolvasó kompatibilitás. Kerülje a mozgó elemeket, ha lehetséges, vagy adjon opciót a kikapcsolásra. Tesztelje eszközzel, mint a Lighthouse vagy axe.
- **Teljesítmény Optimalizálás**: Minimalizálja a bundle méretet (JS/CSS), használjon lazy loading-ot, optimalizálja képeket és API hívásokat. Adatbázis lekérdezések optimalizálása indexekkel és caching-gel. Lokalizált tartalom caching-je a gyorsabb betöltés érdekében.
- **Többnyelvűség**: Az alkalmazás támogatja a lokalizációt (i18n). Frontendben i18next vagy hasonló könyvtár használata, backendben .NET IStringLocalizer. Nyelvek: pl. magyar, angol alapértelmezettként. Szövegek dinamikus betöltése a felhasználó kulturális beállítása alapján. RTL (right-to-left) nyelvek támogatása, ha szükséges.

## 6. Biztonság
- **Beépített Biztonság**: Használjon HTTPS-t, validálja a bemeneteket, védekezzen XSS, CSRF és injection támadások ellen. Adatbázis szinten használjon parameterized queries-t az SQL injection ellen.
- **Autentikáció/Autorizáció**: JWT tokenek .NET backendben, biztonságos tárolás frontendben. Adatbázis szerepkörök és engedélyek kezelése.
- **Adatvédelem**: Kövesse a GDPR elveket. Adatok titkosítása az adatbázisban (pl. Always Encrypted MS SQL Server 2022-ben). Lokalizált és akadálymentes adatok védelme kulturális és akadálymentes szempontból.

## 7. Fejlesztési Eszközök és Környezet
- **IDE**: Visual Studio (backend), VS Code (frontend).
- **Package Manager**: 
  - **Frontend**: npm vagy yarn a csomagkezeléshez. A `package.json` fájlban rögzítsük a pontos verziószámokat (`dependencies` és `devDependencies`).
  - **Backend**: NuGet csomagkezelés. A `.csproj` fájlokban használjunk pontos verziószámokat a csomagokhoz.
  - **Minden fejlesztő telepítse a szükséges globális eszközöket**:
    ```bash
    # .NET SDK (a projekt verziójának megfelelően)
    dotnet --version
    
    # Node.js LTS verzió és npm
    node --version
    npm --version
    
    # Angular CLI globálisan
    npm install -g @angular/cli
    
    # TypeScript globálisan
    npm install -g typescript
    
    # ESLint és Prettier globálisan (opcionális, ha nincs beállítva IDE-be)
    npm install -g eslint prettier
    
    # Projekt függőségek telepítése (a projekt gyökerében)
    cd frontend
    npm install
    
    # Fejlesztői szerver indítása
    ng serve
    ```
  - A build folyamat a CI/CD rendszerben is telepítse a függőségeket minden fordítás előtt.
- **Adatbázis Eszközök**: SQL Server Management Studio (SSMS), Entity Framework Core Tools.
- **Lokalizáció Eszközök**: i18n könyvtárak (pl. i18next JS-hez, .NET Resource Files).
- **Akadálymentesítési Eszközök**: Lighthouse, axe-core, WAVE, billentyűzet navigációs tesztelők.
- **Containerizálás**: Docker minden mikroszervizhez, beleértve az MS SQL Server 2022 konténereket fejlesztési célra.
- **Verziókezelés**: Git.
- **CI/CD Eszközök**: GitHub Actions, Azure DevOps, integrálva az adatbázis migrációkkal, lokalizáció fájlokkal és a11y scannerekkel.

## 8. Szoftver Szolgáltatásként (SaaS) és Felhő Támogatás

### 8.1 SaaS Alapelvek
- **Adattitkosítási Szabályzat**: Minden bérlői adat titkosítva legyen tárolva (AES-256 vagy erősebb algoritmusok használatával)
- **Titkosítási Kulcsok Kezelése**: Centralizált kulcskezelő rendszer (pl. Azure Key Vault, AWS KMS) használata a titkosítási kulcsok biztonságos tárolásához és kezeléséhez
- **Bérlői Kulcskezelés**: Lehetőség bérlőnkénti egyedi titkosítási kulcsok használatára a szigorúbb adatvédelmi követelmények kielégítéséhez
- **Bérlői Önkiszolgáló Portál**: Webes felület a bérlők önálló regisztrációjához, konfigurálásához és számlázási adatok kezeléséhez
- **Szolgáltatási Szintek (Tiering)**: Különböző előfizetési csomagok (pl. Alap, Prémium, Enterprise) eltérő funkciókkal és korlátokkal
- **Használatalapú Számlázás**: Részletes felhasználói és erőforrás-használati nyilvántartás számlázási célokra
- **Saját Személyes Adatkezelés (GDPR)**: Eszközök a bérlők számára a felhasználói adatok kezeléséhez és exportálásához
- **SLA (Szolgáltatási Szint Szerződések)**: Garantált rendelkezésre állás, hibaelhárítási idők és kompenzációs rendszerek

### 8.2 Felhő Támogatás és Üzemeltetés

### 8.2.1 Felhő Platform Függetlenség
- **Platformfüggetlen Titkosítási Rendszer**: A titkosítási megoldások legyenek függetlenek az alatta lévő felhőplatformtól
- **Titkosított Tárolókonténer**: Minden tárolási szolgáltatás (blob, file, queue) legyen titkosítva a platform által biztosított mechanizmusokkal
- **Biztonságos Kommunikáció**: TLS 1.2 vagy újabb használata minden szolgáltatás közötti kommunikációhoz (mTLS a szolgáltatások között)
- Az alkalmazás legyen kész felhő platformokra történő üzembe helyezésre (Azure, AWS, Google Cloud)
- Használjon cloud-native megoldásokat, mint a Kubernetes, Docker és Service Mesh technológiák
- Implementáljon cloud configuratív beállításokat a különböző környezetekhez (fejlesztői, teszt, éles)
- Használjon Secret Management rendszereket a bizalmas adatok kezeléséhez (Azure Key Vault, AWS Secrets Manager)

### 8.2.2 Skálázhatóság és Megbízhatóság
- Horizontális skálázás támogatása minden szolgáltatásban
- Auto-scaling csoportok használata a felhő szolgáltatók eszközeivel
- Regionális redundancia és Geo-replikáció támogatása
- Disaster Recovery tervek és automatizált visszaállítási folyamatok

### 8.2.3 CI/CD és Infrastruktúra Mint Kód (IaC)
- Infrastruktúra kódolása Terraform vagy ARM sablonokkal
- CI/CD folyamatok beállítása GitHub Actions, Azure DevOps vagy hasonló eszközökkel
- Blue-Green vagy Canary deployment stratégiák implementálása
- Automatizált tesztelés minden telepítési fázisban

### 8.2.4 Figyelés és Naplózás
- Integrált megoldások a felhő szolgáltatók figyelő eszközeivel (Azure Monitor, AWS CloudWatch)
- Egyéni metrikák és riasztások beállítása
- Központosított naplózás megoldások használata (ELK stack, Azure Log Analytics)
- Teljesítményfigyelés és kapacitás tervezés

## 9. SaaS Adatkezelés és Adatbázis Követelmények
### 9.1 SaaS Adatszigorló Elkülönítés és Tárolás
- **Adatok Titkosítása Nyugalmi Állapotban**: Minden adat titkosítva legyen tárolva (TDE - Transparent Data Encryption) az adatbázis szintjén
- **Oszlopszintű Titkosítás**: Bizalmas adatok (pl. személyes azonosításra alkalmas adatok, fizetési információk) esetén oszlopszintű titkosítás alkalmazása
- **Always Encrypted**: Bizalmas adatok védelme az alkalmazás és az adatbázis közötti átvitel során is (Always Encrypted technológia)
- **Titkosított Biztonsági Mentések**: Az összes adatbiztonsági mentés titkosítva legyen tárolva külön titkosítási kulcsokkal
- **Adatszigorló Elkülönítés**: Minden bérlő adatai logikailag és fizikailag el legyenek különítve, megfelelve az iparági szabályozásoknak (GDPR, HIPAA stb.)
- **Adatbiztonság**: Titkosítás mind a nyugalmi, mind az átvitel alatt álló adatokhoz
- **Adatbiztonsági mentés és Visszaállítás**: Automatizált, bérlő-specifikus biztonsági mentési és visszaállítási folyamatok
- **Adatáttelepítés**: Eszközök a bérlők számára az adatok importálásához/exportálásához szabványos formátumokban
- **Adatmegőrzési szabályok**: Konfigurálható adatmegőrzési szabályok a bérlői adatokhoz

### 9.2 Több-bérlős Adatkezelés
- **Bérlő Azonosítás**: Minden HTTP kéréssel együtt kötelezően küldendő a bérlő azonosítója (pl. `X-Tenant-Id` fejléc, JWT claim, vagy aldomény)
- **Adatszűrés**: Automatikus szűrés minden adatbázis lekérdezésnél a bérlő alapján (pl. Entity Framework Core globális szűrőkkel)
- **Bérlő Kontextus**: A bérlő adatainak elérése a teljes alkalmazásban egységes módon, függetlenül a hívási kontextustól
- **Bérlő-specifikus konfiguráció**: Minden bérlő számára testreszabható beállítások és konfigurációk kezelése
- **Teljesítményoptimalizálás**: Gyorsítótárazás a bérlői adatokhoz, különösen a gyakran használt bérlői beállításokhoz

### 9.3 Adatbázis Specifikus Követelmények
- **Adatbázis Motor**: Elsődleges adatbázisként MS SQL Server 2022 használata, de az architektúra legyen rugalmas más adatbázisok (pl. PostgreSQL, MySQL) támogatásához is. A több-bérlős architektúra keretében külön adatbázis vagy sémakezelés használata bérlőnként. Használjon repository mintát és Dapper-t a nagyobb teljesítményű lekérdezésekhez.
- **Adatelérési réteg**: Használjon Entity Framework Core-t ORM-ként, repository mintával az adatelérés kiszervezéséhez. Implementáljon Unit of Work mintát a tranzakciók kezeléséhez. Minden adatbázis művelet automatikusan szűrje a bérlői adatokat a beállított bérlői kontextus alapján.
- **Adatbázis függetlenség**: A kód ne legyen szorosan kötve az adatbázis motorhoz. Használjon DTO-kat az adatok továbbításához a rétegek között. A több-bérlős architektúra megvalósítása ne függjön az adatbázis motor specifikus funkcióitól.
- **Migrációk**: Entity Framework Core Code-First migrációk használata sémaváltoztatásokhoz. Minden migráció legyen idempotens és verziókezelt. A migrációs rendszer támogassa a bérlő-specifikus sémaváltoztatásokat is.
- **Teljesítmény**: Használjon indexelést, particionálást és más optimalizációs technikákat. Használjon modernebb funkciókat, mint a JSON support, temporal tables és columnstore indexek. A több-bérlős környezetben különösen fontos a megfelelő indexelés a bérlői szűrők mellett.
- **Biztonság**: Connection string-ek biztonságos kezelése (pl. Azure Key Vault), szerepkör-alapú hozzáférés-vezérlés (RBAC), adattitkosítás nyugalmi és tranzakció közbeni állapotban is. A bérlők közötti adatszigorló elkülönítésének biztosítása. Minden adatbázis művelet ellenőrizze a bérlői engedélyeket.
- **Backup és Recovery**: Automatizált backup-ok és recovery tervek a CI/CD pipeline-ban. Több rétegű biztonsági mentési stratégia (teljes, differenciális, tranzakciónapló). A bérlői adatok különálló biztonsági mentése és visszaállítása lehetősége. Bérlői adatok exportálása és importálása standard formátumban.

## 10. SaaS Szolgáltatás Menedzsment

### 10.1 Bérlői Életciklus Kezelés
- **Regisztráció és Aktiválás**: Automatizált folyamatok az új bérlők felvételéhez és aktiválásához
- **Fiókkezelés**: Önkiszolgáló eszközök a bérlők számára a fiókbeállítások kezeléséhez
- **Előfizetéskezelés**: Szolgáltatáscsomagok frissítése, lefokozása és lemondása
- **Felfüggesztés és Törlés**: Biztonságos folyamatok inaktív fiókok kezeléséhez és adatok végleges törléséhez
- **Visszaállítási lehetőségek**: Törölt bérlők időszakos visszaállításának lehetősége

### 10.2 Számlázás és Használati Díjak Nyomon Követése
- **Fizetési Adatok Titkosítása**: Minden fizetési kártya adat (PCI DSS szabványnak megfelelően) titkosítva legyen tárolva
- **Titkosított Tárterület a Számlákhoz**: A generált számlák és pénzügyi dokumentumok titkosítva legyenek tárolva
- **Biztonságos Fizetési Folyamat**: A fizetési folyamat során az érzékeny adatok soha ne kerüljenek a rendszerünk szervereire (pl. Stripe, Braintree használata)
- **Naplózási Adatok Titkosítása**: A biztonsági naplók és audit trail-ek titkosítva legyenek tárolva
- **Használati metrikák**: Részletes nyomon követés az erőforrás-használatról (tárhely, API hívások, felhasználók száma stb.)
- **Számlázási ciklusok**: Támogatás havi, negyedéves és éves számlázási ciklusokhoz
- **Többfajta fizetési mód**: Bankkártya, banki átutalás és egyéb fizetési módok támogatása
- **Számla előnézet és letöltés**: Önkiszolgáló felület a számlák megtekintéséhez és letöltéséhez
- **Pénzügyi jelentések**: Részletes jelentések a bérleti díjakról és a használati díjakról

## 11. Biztonsági Követelmények és Titkosítási Szabályok

### 11.1 Adatvédelem és Titkosítás
- **Adatbesorolási Szabályzat**: Minden adatot besorolni kell érzékenységi szint szerint, és ennek megfelelően kell kezelni a titkosítási követelményeket
- **Titkosítási Szabványok**: A titkosításhoz csak iparági szabványnak megfelelő algoritmusok használata (AES-256, RSA-2048, SHA-256)
- **Kulcs Életciklus Kezelés**: A titkosítási kulcsok életciklusának kezelése (generálás, használat, forgatás, archiválás, megsemmisítés)
- **Titkosított Kommunikáció**: Minden API hívás és szolgáltatások közötti kommunikáció TLS 1.2 vagy újabb titkosítással
- **Titkosított Munkamenetek**: Felhasználói munkamenetek védelme HTTPS és biztonságos sütik használatával
- **Forráskód Titkosítás**: A forráskódban tárolt bizalmas adatok (pl. jelszavak, kulcsok) titkosítása és biztonságos kezelése
- **Titkosított Üzenetsor-kezelés**: Az aszinkron üzenetkezelés során az üzenetek tartalmának titkosítása (pl. Azure Service Bus, RabbitMQ TLS)

### 11.2 Biztonságos Fejlesztési Eljárások
- **Biztonságos Kódolási Gyakorlatok**: OWASP Top 10 alapján biztonságos kód írása
- **Titkosítási Könyvtárak**: Csak jól auditált, széles körben használt titkosítási könyvtárak használata
- **Biztonsági Tesztelés**: Rendszeres biztonsági auditok és penetrációs tesztek végrehajtása
- **Sejthető Adatok Titkosítása**: Minden olyan adat titkosítása, amelyből más adatokat lehet következtetni (pl. email címek, felhasználónevek)
- **Titkosított Naplózás**: A naplózott érzékeny adatok titkosítása

## 12. Többnyelvűség (Internationalization - i18n) Specifikus Követelmények
- **Frontend Lokalizáció**: Használjon i18next vagy hasonló könyvtárat a JavaScriptben. Lokalizáció fájlok (pl. JSON) minden támogatott nyelvhez (pl. hu.json, en.json). Dinamikus nyelv váltás a felhasználó beállítása alapján.
- **Backend Lokalizáció**: .NET beépített IStringLocalizer használata. Resource fájlok (resx) minden nyelvhez. Kulturális beállítások kezelése (pl. CultureInfo).
- **Adatbázis Támogatás**: Lokalizált szövegek tárolása külön táblákban vagy JSON oszlopokban, hogy kulturális szempontból kezelhetők legyenek.
- **Tesztelés**: Lokalizáció tesztek minden nyelvhez, beleértve a fordítások helyességét és formázást.
- **CI/CD Integráció**: Lokalizáció fájlok automatikus validálása és deploy-ja.

## 10. Akadálymentesítés (Accessibility - a11y) Specifikus Követelmények
- **WCAG 2.1 Megfelelés**: AA szint követése minden komponensben. Fókusz: érzékelhető (perceivable), működtethető (operable), érthető (understandable), robusztus (robust).
- **Frontend Implementáció**: ARIA címkék (pl. role, aria-label), billentyűzet navigáció (tab, enter, escape), színkontraszt (4.5:1), alt text, focus indikátorok, mozgó tartalom figyelmeztetések.
- **Backend Támogatás**: API-k biztosítsák a strukturált adatokat (pl. JSON-LD), hogy képernyőolvasók használhassák.
- **Tesztelés**: Automatizált a11y scannerek (pl. axe-core, Lighthouse Accessibility audit) minden build során. Manuális tesztelés különböző eszközökkel (pl. képernyőolvasók).
- **CI/CD Integráció**: A11y ellenőrzések automatizálása, sikertelen build, ha nem felel meg a WCAG 2.1-nek.

## 10.1 Több-bérlős Tesztelés
- **Egységtesztek**: Minden bérlő-specifikus logikához írjanak egységteszteket, amelyek ellenőrzik a bérlői elkülönítést
- **Integrációs tesztek**: Teszteljék a bérlők közötti adatszigorló elkülönítését
- **Teljesítménytesztek**: Ellenőrizzék az alkalmazás viselkedését nagy számú bérlő esetén
- **Bérlő-kezelő felület**: Teszteljék a bérlők létrehozását, módosítását és törlését
- **Adatáttelepítés**: Automatizált tesztek a bérlői adatok migrációjához

## 11. Kódgenerálás és Tesztelési Folyamat
- **Automatizált Teszt Generálás**: Minden kódgenerálás során (pl. új függvény, osztály, mikroszerviz) automatikusan generáljon unit és integrációs teszteket a generált kódhoz. Használjon eszközöket, mint a Jest/Mocha (JS) vagy xUnit (C#) a tesztgeneráláshoz.
- **Teszt Futtatás és Iteráció**: A generált teszteket azonnal futtassa helyi környezetben. Ha a tesztek sikertelenek, javítson a kódon vagy a teszteken, és futtassa újra, amíg minden teszt nem sikerül. Ez biztosítja a kód minőségét és tesztelhetőségét.
- **Commit Előtti Teszt Futtatás**: Minden commit előtt futtassa az összes tesztet (unit, integrációs, E2E, teljesítmény). Csak akkor engedje a commit-ot, ha minden teszt sikeres. Ez részét képezi a CI/CD pipeline-nak.
- **Eszközök**: Használjon pre-commit hook-okat (pl. Husky JS-hez) a lokális teszt futtatáshoz, és GitHub Actions-t a távoli teszteléshez.
- **Mobile-First Megközelítés**: Minden kódgenerálás során prioritizáljon a mobil eszközökre optimalizálást. Kezdje a CSS és layout tervezést mobil nézettel, majd skálázza fel nagyobb képernyőkre. Biztosítsa a reszponzivitást media queries és flexible grid rendszerekkel.

## 12. Angular Specifikus Követelmények
- **Komponens Architektúra**: 
  - Minden komponens legyen egyetlen felelősségi elv alapján kialakítva
  - Használjon OnPush változó észlelési stratégiát a teljesítmény érdekében
  - Tisztítsa fel az előfizetéseket (unsubscribe) a komponens megsemmisülésekor
  - Használjon async pipe-ot a template-ekben ahol lehetséges

- **Modulok és Lazy Loading**:
  - Funkcionális modulok használata a kód szervezéséhez
  - Lazy loading a főbb útvonalakhoz a kezdeti betöltési idő csökkentésére
  - Shared modul közös komponensek, direktívák és csővek számára
  - Core modul egyszeri szolgáltatásoknak

- **Teljesítményoptimalizálás**:
  - AOT (Ahead-of-Time) fordítás használata éles környezetben
  - Production build a --prod kapcsolóval
  - Bundle analízis a build méret optimalizálásához
  - Lazy loading a moduloknál
  - TrackBy használata *ngFor ciklusokban

## 13. TypeScript Ajánlások
- **Kötelező típusok**: Minden változó, függvény paraméter és visszatérési érték rendelkezzen típussal
- **Interface-k használata**: Minden adatmodellhez készüljön TypeScript interface
- **Strict mode**: Minden TypeScript projekt legyen strict módban
- **ESLint és Prettier**: A kódformázás és minőségbiztosítás érdekében
- **Barrel fájlok**: Modulok egyszerűbb importálhatóságához
- **Design Pattern-ek alkalmazása**: Használjon megfelelő tervezési mintákat a kód újrafelhasználhatóságának és karbantarthatóságának növelésére

## 14. Frontend Design Pattern-ek (TypeScript/Angular)

### 14.1 Létrehozási Módoszatok (Creational Patterns)
- **Singleton**: 
  - Használati hely: Konfigurációk, naplózás, gyorsítótárazás
  - Angular-ban: `@Injectable({ providedIn: 'root' })` dekorátorral
  - Példa: `LoggerService`, `ConfigurationService`

- **Gyártó Módszer (Factory Method)**:
  - Használati hely: Dinamikus komponenslétrehozás, adatforrásfüggetlen szolgáltatások
  - Példa: `DialogFactory` a különböző típusú dialógusok létrehozásához

- **Absztrakt Gyár (Abstract Factory)**:
  - Használati hely: Kapcsolódó objektumcsaládok létrehozása
  - Példa: Témakezelő rendszerek (light/dark theme komponensek)

### 14.2 Szerkezeti Módoszatok (Structural Patterns)
- **Kompozit (Composite)**:
  - Használati hely: Fa-szerkezetű komponensek (pl. menük, navigáció)
  - Angular-ban: Rekurzív komponensek `@Input`-okkal

- **Díszítő (Decorator)**:
  - Használati hely: Funkcionalitás bővítése futási időben
  - Angular-ban: `@Directive`-ok, metódusdekorátorok
  - Példa: `@LogExecutionTime()`, `@Cacheable()`

- **Adapter (Adapter/Wrapper)**:
  - Használati hely: Örökölt rendszerek integrálása, harmadik féltől származó könyvtárak becsomagolása
  - Példa: `ApiAdapter` a külső API hívásokhoz

### 14.3 Viselkedési Módoszatok (Behavioral Patterns)
- **Stratégia (Strategy)**:
  - Használati hely: Algoritmusok cserélhetősége futási időben
  - Példa: Rendezés, szűrés stratégiák
  ```typescript
  interface SortStrategy<T> {
    sort(items: T[]): T[];
  }
  
  class AlphabeticalSort implements SortStrategy<User> {
    sort(users: User[]): User[] {
      return [...users].sort((a, b) => a.name.localeCompare(b.name));
    }
  }
  ```

- **Megfigyelő (Observer)**:
  - Használati hely: Eseménykezelés, állapotváltozások követése
  - RxJS `Subject` és `Observable` használatával
  - Példa: State management, valós idejű adatfrissítések

- **Parancs (Command)**:
  - Használati hely: Műveletek paraméterezése és ütemezése
  - Példa: Visszavonható műveletek (undo/redo)

### 14.4 Angular-specifikus Pattern-ek
- **Komponens és Sablon (Component Template)**:
  - Használati hely: UI komponensek létrehozása
  - Angular `@Component` dekorátorral

- **Dependency Injection**:
  - Használati hely: Szolgáltatások injektálása
  - Angular DI rendszerének használata `@Injectable()` dekorátorral

- **Küldő (Mediator)**:
  - Használati hely: Komponensek közötti kommunikáció
  - Példa: `DataService` mint közvetítő komponensek között

## 15. Backend Design Pattern-ek (C#/.NET)

### 15.1 Alapvető Tervezési Minták
- **Repository Pattern**:
  - Használati hely: Adatelérési réteg absztrakciója
  - Előny: Könnyebb tesztelhetőség, egyszerűbb karbantarthatóság
  - Példa: `IUserRepository`, `ProductRepository`

- **Unit of Work**:
  - Használati hely: Több adatbázis művelet atomi végrehajtása
  - Integráció: Entity Framework Core `DbContext`-tel
  - Példa: Tranzakciókezelés több repository között

- **CQRS (Command Query Responsibility Segregation)**:
  - Használati hely: Külön választani az író és olvasó műveleteket
  - Előny: Skálázhatóság, jobb teljesítmény
  - Implementáció: `ICommand`/`IQuery` interfészekkel

### 15.2 Mikroszerviz Architektúra Minták
- **API Gateway**:
  - Használati hely: Bejövő kérések kezelése, útválasztás
  - Megoldások: Ocelot, YARP, Azure API Management
  - Funkciók: Hitelesítés, gyorsítótárazás, terheléselosztás

- **Service Discovery**:
  - Használati hely: Dinamikus szolgáltatásfelderítés
  - Megoldások: Consul, Eureka, Kubernetes Service Discovery

- **Circuit Breaker**:
  - Használati hely: Hibatűrő rendszerek kialakítása
  - Implementáció: Polly könyvtár
  ```csharp
  var circuitBreaker = Policy
      .Handle<HttpRequestException>()
      .CircuitBreakerAsync(
          exceptionsBeforeBreaking: 3,
          durationOfBreak: TimeSpan.FromSeconds(30)
      );
  ```

### 15.3 Adatkezelési Minták
- **Specification Pattern**:
  - Használati hely: Üzleti szabályok újrafelhasználhatósága
  - Előny: Tisztább adatelérési réteg
  - Példa: `ISpecification<T>` implementációk szűréshez

- **Outbox Pattern**:
  - Használati hely: Megbízható üzenetküldés
  - Megoldás: Tranzakción belüli üzenetküldés
  - Implementáció: `IDomainEvent` és `IOutbox` használatával

### 15.4 Teljesítményoptimalizálási Minták
- **Caching Strategies**:
  - Használati hely: Gyakran változó adatok gyorsítótárazása
  - Megoldások: Redis, MemoryCache, Distributed Cache
  - Minta: Cache-Aside, Write-Through, Read-Through

- **Bulkhead Pattern**:
  - Használati hely: Hibatűrés növelése
  - Implementáció: Erőforrások elkülönítése
  - Példa: Külön HttpClient példányok különböző szolgáltatásokhoz

### 15.5 Big Data és Stream Feldolgozás

#### 15.5.1 Apache Kafka Integráció
- **Használati hely**: Eseményvezérelt architektúrák, üzenetküldés, adatstream-ek
- **Alapvető komponensek**:
  - **Producerek**: Adatforrások, amelyek üzeneteket küldenek a topic-okba
  - **Fogyasztók**: A topic-okból fogyasztják az üzeneteket
  - **Topic-ok**: Logikai csatornák az üzenetek kategorizálásához
  - **Brokerek**: A Kafka kiszolgálók, amelyek kezelik a topic-okat

- **Minta konfiguráció (appsettings.json)**
  ```json
  {
    "Kafka": {
      "BootstrapServers": "kafka1:9092,kafka2:9092,kafka3:9092",
      "GroupId": "logistics-service-group",
      "Topics": {
        "OrderEvents": "order-events",
        "InventoryUpdates": "inventory-updates"
      },
      "SaslUsername": "${KAFKA_USERNAME}",
      "SaslPassword": "${KAFKA_PASSWORD}",
      "SecurityProtocol": "SaslSsl",
      "SaslMechanism": "Plain"
    }
  }
  ```

- **Producer példa (C#)**
  ```csharp
  public class KafkaProducerService
  {
      private readonly IProducer<Null, string> _producer;
      private readonly string _topic;

      public KafkaProducerService(IConfiguration config)
      {
          var producerConfig = new ProducerConfig
          {
              BootstrapServers = config["Kafka:BootstrapServers"],
              SecurityProtocol = SecurityProtocol.SaslSsl,
              SaslMechanism = SaslMechanism.Plain,
              SaslUsername = config["Kafka:SaslUsername"],
              SaslPassword = config["Kafka:SaslPassword"]
          };
          _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
          _topic = config["Kafka:Topics:OrderEvents"];
      }

      public async Task ProduceAsync(string message)
      {
          await _producer.ProduceAsync(_topic, new Message<Null, string> 
          { 
              Value = message 
          });
      }
  }
  ```

#### 15.5.2 Apache Flink Integráció
- **Használati hely**: Valós idejű adatfeldolgozás és elemzés
- **Főbb képességek**:
  - Event time processing
  - Exactly-once feldolgozás
  - Állapotkezelés
  - Windows műveletek

- **Flink Job konfiguráció (flink-conf.yaml)**
  ```yaml
  jobmanager.rpc.address: jobmanager
  jobmanager.rpc.port: 6123
  jobmanager.memory.process.size: 1600m
  taskmanager.memory.process.size: 1728m
  taskmanager.numberOfTaskSlots: 1
  parallelism.default: 1
  state.backend: filesystem
  state.checkpoints.dir: file:///opt/flink/checkpoints
  state.savepoints.dir: file:///opt/flink/savepoints
  execution.checkpointing.interval: 10s
  execution.checkpointing.mode: EXACTLY_ONCE
  ```

- **Példa Flink feldolgozó (Java)**
  ```java
  public class OrderProcessingJob {
      public static void main(String[] args) throws Exception {
          final StreamExecutionEnvironment env = 
              StreamExecutionEnvironment.getExecutionEnvironment();
          
          // Checkpoint konfiguráció
          env.enableCheckpointing(10000);
          env.getCheckpointConfig().setCheckpointingMode(CheckpointingMode.EXACTLY_ONCE);
          
          // Kafka forrás konfigurálása
          Properties kafkaProps = new Properties();
          kafkaProps.setProperty("bootstrap.servers", "kafka:9092");
          kafkaProps.setProperty("group.id", "flink-order-processor");
          
          // Kafka forrás létrehozása
          FlinkKafkaConsumer<String> consumer = new FlinkKafkaConsumer<>(
              "order-events",
              new SimpleStringSchema(),
              kafkaProps
          );
          
          // Adatfolyam feldolgozása
          DataStream<String> stream = env
              .addSource(consumer)
              .map(new OrderProcessor())
              .keyBy(value -> value.getCategory())
              .window(TumblingEventTimeWindows.of(Time.minutes(5)))
              .process(new OrderAggregator());
          
          // Eredmény kiírása
          stream.print();
          
          // Feladat indítása
          env.execute("Order Processing Job");
      }
  }
  ```

#### 15.5.3 Docker és Docker Compose Konfiguráció
- **Dockerfile (minta .NET Core szolgáltatáshoz)**
  ```dockerfile
  # Build szakasz
  FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
  WORKDIR /src
  COPY ["Logistics.API/Logistics.API.csproj", "Logistics.API/"]
  RUN dotnet restore "Logistics.API/Logistics.API.csproj"
  COPY . .
  WORKDIR "/src/Logistics.API"
  RUN dotnet build "Logistics.API.csproj" -c Release -o /app/build
  
  # Publish szakasz
  FROM build AS publish
  RUN dotnet publish "Logistics.API.csproj" -c Release -o /app/publish
  
  # Futtatási szakasz
  FROM mcr.microsoft.com/dotnet/aspnet:6.0
  WORKDIR /app
  COPY --from=publish /app/publish .
  EXPOSE 80
  EXPOSE 443
  ENTRYPOINT ["dotnet", "Logistics.API.dll"]
  ```

- **docker-compose.yml (teljes környezet)**
  ```yaml
  version: '3.8'
  
  services:
    # API szolgáltatás
    logistics-api:
      build:
        context: .
        dockerfile: Dockerfile
      ports:
        - "5000:80"
      environment:
        - ASPNETCORE_ENVIRONMENT=Development
        - ConnectionStrings:DefaultConnection=Server=db;Database=LogisticsDb;User=sa;Password=YourStrong@Passw0rd
        - Kafka:BootstrapServers=kafka:9092
      depends_on:
        - db
        - kafka
    
    # Adatbázis
    db:
      image: mcr.microsoft.com/mssql/server:2022-latest
      environment:
        - ACCEPT_EULA=Y
        - SA_PASSWORD=YourStrong@Passw0rd
        - MSSQL_PID=Developer
      ports:
        - "1433:1433"
      volumes:
        - sql_data:/var/opt/mssql
    
    # Kafka és Zookeeper
    zookeeper:
      image: confluentinc/cp-zookeeper:7.0.0
      environment:
        ZOOKEEPER_CLIENT_PORT: 2181
        ZOOKEEPER_TICK_TIME: 2000
    
    kafka:
      image: confluentinc/cp-kafka:7.0.0
      depends_on:
        - zookeeper
      ports:
        - "9092:9092"
      environment:
        KAFKA_BROKER_ID: 1
        KAFKA_ZOOKEEPER_CONNECT: zookeeper:2181
        KAFKA_ADVERTISED_LISTENERS: PLAINTEXT://kafka:29092,PLAINTEXT_HOST://localhost:9092
        KAFKA_LISTENER_SECURITY_PROTOCOL_MAP: PLAINTEXT:PLAINTEXT,PLAINTEXT_HOST:PLAINTEXT
        KAFKA_INTER_BROKER_LISTENER_NAME: PLAINTEXT
        KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR: 1
    
    # Flink JobManager
    jobmanager:
      image: flink:1.14.0-scala_2.12-java11
      ports:
        - "8081:8081"
      command: jobmanager
      environment:
        - JOB_MANAGER_RPC_ADDRESS=jobmanager
      volumes:
        - ./flink-conf:/opt/flink/conf
    
    # Flink TaskManager
    taskmanager:
      image: flink:1.14.0-scala_2.12-java11
      depends_on:
        - jobmanager
      command: taskmanager
      environment:
        - JOB_MANAGER_RPC_ADDRESS=jobmanager
      volumes:
        - ./flink-conf:/opt/flink/conf
  
  volumes:
    sql_data:
  ```

### 15.6 Biztonsági Minták
- **Validity Check Pattern**:
  - Használati hely: Bemeneti adatok validálása
  - Implementáció: FluentValidation könyvtár
  ```csharp
  public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
  {
      public CreateUserCommandValidator()
      {
          RuleFor(x => x.Email).EmailAddress().NotEmpty();
          RuleFor(x => x.Password).MinimumLength(8);
      }
  }
  ```

- **Retry Pattern**:
  - Használati hely: Ideiglenes hibák kezelése
  - Megoldás: Polly könyvtárral
  - Konfigurálható újrapróbálkozási stratégiák

## 16. Kubernetes Kompatibilitás és Üzembehelyezés

### 16.1 Alapkövetelmények
- **Konténerizáció**: Minden szolgáltatás Docker konténerként legyen csomagolva
- **Státusztalanság**: A szolgáltatások legyenek állapotmentesek, vagy használjanak külső állapotkezelést (pl. Redis, Database)
- **Konfiguráció kezelés**: Környezeti változók és ConfigMaps használata
- **Titkos kulcsok kezelése**: Secrets használata bizalmas adatokhoz
- **Health Check-ek**: Liveness és Readiness probék implementálása

### 16.2 Kubernetes Erőforrások

#### 16.2.1 Deployment Példa (deployment.yaml)
```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: logistics-api
  labels:
    app: logistics
    tier: backend
spec:
  replicas: 3
  selector:
    matchLabels:
      app: logistics
      tier: backend
  template:
    metadata:
      labels:
        app: logistics
        tier: backend
    spec:
      containers:
      - name: logistics-api
        image: your-registry/logistics-api:${TAG:-latest}
        ports:
        - containerPort: 80
        envFrom:
        - configMapRef:
            name: logistics-config
        - secretRef:
            name: logistics-secrets
        livenessProbe:
          httpGet:
            path: /health/live
            port: 80
          initialDelaySeconds: 30
          periodSeconds: 10
        readinessProbe:
          httpGet:
            path: /health/ready
            port: 80
          initialDelaySeconds: 5
          periodSeconds: 5
        resources:
          requests:
            cpu: "100m"
            memory: "256Mi"
          limits:
            cpu: "500m"
            memory: "1Gi"
```

#### 16.2.2 Service Példa (service.yaml)
```yaml
apiVersion: v1
kind: Service
metadata:
  name: logistics-service
  labels:
    app: logistics
    tier: backend
spec:
  type: ClusterIP
  ports:
  - port: 80
    targetPort: 80
    protocol: TCP
    name: http
  selector:
    app: logistics
    tier: backend
```

#### 16.2.3 Ingress Példa (ingress.yaml)
```yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: logistics-ingress
  annotations:
    nginx.ingress.kubernetes.io/rewrite-target: /$1
    cert-manager.io/cluster-issuer: "letsencrypt-prod"
spec:
  tls:
  - hosts:
    - api.yourdomain.com
    secretName: logistics-tls
  rules:
  - host: api.yourdomain.com
    http:
      paths:
      - path: /api/v1/?(.*)
        pathType: Prefix
        backend:
          service:
            name: logistics-service
            port:
              number: 80
```

### 16.3 Helm Chart Struktúra
```
logistics-chart/
├── Chart.yaml
├── values.yaml
├── charts/
├── templates/
│   ├── deployment.yaml
│   ├── service.yaml
│   ├── ingress.yaml
│   ├── configmap.yaml
│   ├── secrets.yaml
│   ├── hpa.yaml
│   └── pdb.yaml
└── README.md
```

### 16.4 Auto-scaling Konfiguráció
```yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: logistics-api-hpa
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: logistics-api
  minReplicas: 2
  maxReplicas: 10
  metrics:
  - type: Resource
    resource:
      name: cpu
      target:
        type: Utilization
        averageUtilization: 70
  - type: Resource
    resource:
      name: memory
      target:
        type: Utilization
        averageUtilization: 80
```

## 17. Továbbfejlesztett Architektúra és Skálázhatóság

### 16.1 Skálázási Stratégiák
- **Vízszintes skálázás**: Automatikus skálázás Docker Swarm vagy Kubernetes alapon
- **Függőleges particionálás**: Adatbázis shardolás mikroszolgáltatások között
- **Replikáció**: Adatbázis és szolgáltatás replikáció a rendelkezésre állás növelésére

### 16.2 Monitorozás és Logolás
- **Prometheus + Grafana**: Metrikák gyűjtése és vizualizáció
- **ELK Stack (Elasticsearch, Logstash, Kibana)**: Központosított naplózás
- **Distributed Tracing**: Jaeger vagy Zipkin használata a kérések nyomon követésére

### 16.3 CI/CD Folyamat
- **GitHub Actions** vagy **Azure DevOps** folyamatok
- Automatikus tesztelés és minőségellenőrzés
- Konténer alapú telepítés Kubernetes-re
- Canary és Blue/Green telepítési stratégiák

### 16.4 Adatbázis Kihívások és Megoldások
- **CQRS**: Külön olvasási és írási modell
- **Event Sourcing**: Minden állapotváltozás eseményként történik
- **Read Models**: Optimalizált lekérdezési modellek a gyors olvasáshoz
- **Több adatbázis használata**: A jobb szolgáltatás elkülönítés és skálázhatóság érdekében

## 18. Tervezési Elvek (SOLID) és Pattern-ek
- **Egyszeri Felelősség (SRP)**: Minden osztány/komponens egyetlen felelősséggel rendelkezzen
- **Nyílt-Zárt Elv (OCP)**: Bővíthetőség a módosítás nélkül (pl. Strategy pattern)
- **Liskov Helyettesítési Elv (LSP):** Az ős osztályt lecserélhetjük a leszármazottjára
- **Interface Elválasztási Elv (ISP)**: Kicsi, célirányos interfészek
- **Függőség Inverzió Elve (DIP)**: Magas szintű modulok ne függjenek az alacsony szintűeket

## 19. Ellenőrzőlista Kódgenerálás Előtt
Mielőtt bármilyen kódot generálnék, ellenőrizzem:
- A generált kód megfelel-e a SOLID elveknek és használ-e megfelelő design pattern-eket (frontend és backend egyaránt)?
- Van-e megfelelő tesztelhetőség (unit/integrációs tesztek, beleértve az adatbázist, i18n-t és a11y-t), és sikeresen futtatva lettek-e?
- Karbantartható-e (dokumentált, moduláris)?
- Integrálható-e a CI/CD pipeline-ba, beleértve az adatbázis migrációkat, lokalizációt és a11y-t?
- Reszponzív és biztonságos-e?
- Mikroszerviz architektúrához illeszkedik-e, Kubernetes-kompatibilis-e, MS SQL Server 2022-vel kompatibilis, támogatja a többnyelvűséget és teljes akadálymentesítéssel rendelkezik?
- Megfelelően van-e konfigurálva a Helm chart a telepítéshez?
- Megvannak-e a szükséges Kubernetes erőforrás-definíciók (Deployment, Service, Ingress stb.)?
- Megfelelően vannak-e beállítva a resource request-ek és limit-ek a konténerekhez?
- Megvannak-e a szükséges RBAC szabályok a szolgáltatások számára?
- Be van-e állítva a megfelelő hálózati házirend (NetworkPolicy)?
- Sikeresek-e az összes teszt minden kódgenerálás és commit előtt?
- Mobile-first megközelítéssel készült-e, prioritizálva a mobil optimalizálást?

Ez a dokumentum alapvető irányelv minden fejlesztési tevékenységhez. Bármilyen eltérés indokolt legyen és dokumentált.
