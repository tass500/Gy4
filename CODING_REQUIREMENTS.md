# Kódolási Követelmény Dokumentum: Modern Reszponzív Webes Alkalmazás Fejlesztése

## Áttekintés
Ez a dokumentum meghatározza a követelményeket egy modern, reszponzív webes alkalmazás fejlesztésére, amely mikroszerviz architektúrára épül. A frontend natív JavaScript, HTML és CSS használatával készül, a backend pedig C# .NET alapokon. A fejlesztés során kiemelt hangsúlyt kell fektetni a karbantarthatóságra, tesztelhetőségre és CI/CD folyamatokra. Az adatok tárolása MS SQL Server 2022-ben történik, az alkalmazás támogatja a többnyelvűséget (internationalization - i18n), és teljes akadálymentesítéssel (accessibility - a11y) rendelkezik.

## 1. Architektúra és Szerkezet
- **Mikroszerviz Alapú Felépítés**: Az alkalmazás különálló, egymástól független mikroszervizekből álljon, amelyek önállóan telepíthetőek és skálázhatóak. Minden mikroszerviz saját felelősségi körrel rendelkezzen (pl. frontend UI, backend API, adatbázis szolgáltatások, lokalizáció szolgáltatás, akadálymentesítés szolgáltatás).
- **Elválasztás Frontend és Backend Között**: Frontend (JavaScript/HTML/CSS) és Backend (C# .NET) szolgáltatások külön mikroszervizekben legyenek elhelyezve. Használjon API-kapcsolatokat (pl. RESTful API-kat) a kommunikációhoz.
- **Moduláris Kód**: Minden komponens moduláris legyen, hogy könnyen újrafelhasználható és cserélhető legyen. Használjon dependency injection-t a .NET backendben és modulrendszert a JavaScriptben.
- **Adattárolás**: Az adatok tárolása MS SQL Server 2022-ben történik. Minden mikroszerviz saját adatbázis példányt használjon (database-per-service minta) a függetlenség biztosításához, vagy centralizált adatbázist, ha szükséges. Használjon Entity Framework Core (EF Core) a .NET backendben az adatbázis eléréséhez és migrációkhoz. A lokalizált és akadálymentes adatok (pl. alt text, ARIA címkék) külön táblákban vagy JSON oszlopokban tárolódjanak.

## 2. Karbantarthatóság
- **Tiszta Kód Elvek**: Kövesse a SOLID elveket (Single Responsibility, Open-Closed, Liskov Substitution, Interface Segregation, Dependency Inversion).
- **Kódolási Szabványok**:
  - **Frontend**: ES6+ JavaScript, CSS Grid/Flexbox reszponzív elrendezéshez, BEM (Block Element Modifier) névadási konvenció CSS osztályokhoz. Használjon i18n könyvtárakat (pl. i18next) a lokalizációhoz, külön JSON fájlokat minden nyelvhez. Akadálymentesítés: ARIA címkék, billentyűzet navigáció, színkontraszt (legalább 4.5:1), alt text minden képhez.
  - **Backend**: C# .NET Core/6+ , aszinkron programozási minták (async/await), LINQ lekérdezések optimalizálása. Entity Framework Core használata adatbázis műveletekhez, biztosítva a típusbiztonságot és a migrációk kezelését. .NET beépített lokalizáció (IStringLocalizer) és akadálymentes API-k (pl. JSON-LD strukturált adatok).
- **Dokumentáció**: Minden függvény, osztály és modul legyen dokumentálva (pl. XML comments C#-ban, JSDoc JavaScriptben). Tartson fenn egy README.md fájlt a projekt áttekintésével, beleértve az adatbázis sémát, migrációk, lokalizáció fájlok és akadálymentesítési útmutatót.
- **Verziókezelés**: Használjon Git-et feature branch stratégiával (pl. Git Flow). Minden commit legyen értelmes és követhető, beleértve az adatbázis migrációkat, lokalizáció frissítéseket és a11y javításokat.
- **Refaktorálás**: Rendszeresen refaktorálja a kódot, hogy elkerülje a technikai adósságot, különösen az adatbázis réteget, lokalizáció logikát és akadálymentesítést.

## 3. Tesztelhetőség
- **Teszt Típusok**:
  - **Unit Tesztek**: Minden logikai egység (függvény, osztály) tesztelve legyen. Frontend: Jest vagy Mocha, beleértve az i18n és a11y funkciókat; Backend: xUnit vagy NUnit, beleértve az EF Core repository-kat, szolgáltatásokat, lokalizációt és a11y API-kat.
  - **Integrációs Tesztek**: Szolgáltatások közötti interakciók tesztelése, beleértve az adatbázis interakciókat, lokalizált tartalmakat és akadálymentes elemeket. Használjon in-memory adatbázist (pl. SQL Server in-memory) a tesztekhez.
  - **E2E Tesztek**: Teljes felhasználói folyamatok tesztelése (pl. Selenium, Cypress frontendhez), amelyek az adatbázis réteget, lokalizációt és akadálymentesítést is érintik.
  - **Teljesítmény Tesztek**: Load testing a skálázhatóság biztosításához, különösen az adatbázis lekérdezésekhez, lokalizált tartalom betöltéséhez és a11y elemekhez.
- **Teszt Fedettség**: Legalább 80% kódlefedettség unit tesztekkel, beleértve az adatbázis logikát, i18n és a11y funkciókat.
- **Automatizált Teszt Futtatás**: Minden commit/pull request előtt futtassa a teszteket, beleértve az adatbázis migrációk, lokalizáció és a11y tesztelését.

## 4. CI/CD Folyamatok
- **Continuous Integration (CI)**:
  - Automatizált build minden commitra (pl. GitHub Actions, Azure DevOps).
  - Kódminőség ellenőrzés: Linting (ESLint JS-hez, StyleCop C#-hoz), statikus analízis (SonarQube), beleértve az adatbázis sémát, lokalizáció fájlokat és a11y ellenőrzéseket.
  - Automatizált teszt futtatás minden build során, beleértve az adatbázis teszteket, i18n validációt és a11y scannereket (pl. axe-core).
- **Continuous Deployment (CD)**:
  - Automatizált deploy staging és production környezetekbe sikeres build és teszt után.
  - Használjon containerizálást (Docker) a mikroszervizekhez, beleértve az adatbázis konténereket, lokalizáció fájlokat és a11y eszközöket.
  - Környezet-specifikus konfigurációk (pl. appsettings.json .NET-hez, environment variables JS-hez), beleértve a connection string-eket MS SQL Server 2022-höz, lokalizáció beállításokat és a11y konfigurációkat.
  - Rollback mechanizmus deploy hibák esetén, beleértve az adatbázis rollback-ot, lokalizáció frissítéseket és a11y javításokat.
  - Adatbázis migrációk, lokalizáció fájlok és a11y eszközök automatizálása: EF Core migrációk, i18n fájlok szinkronizálása és a11y scannerek futtatása a deploy során.
- **Monitoring és Logging**: Integrálja a logging-et (pl. Serilog .NET-hez, Winston JS-hez) és monitoring-ot (pl. Application Insights, Prometheus) a teljesítmény és hibák nyomon követéséhez, beleértve az adatbázis teljesítményt, lokalizált tartalom betöltését és a11y problémákat.

## 5. Reszponzivitás és Felhasználói Élmény
- **Reszponzív Design**: Használjon mobil-először (mobile-first) megközelítést CSS-ben. Biztosítsa a kompatibilitást különböző képernyőméretekkel (pl. Bootstrap vagy Tailwind CSS alapok).
- **Akadálymentesítés (Accessibility - a11y)**: Kövesse a WCAG 2.1 AA szintet. Implementálja: ARIA címkék, billentyűzet navigáció (tab order), színkontraszt (4.5:1 minimum), alt text képekhez, focus indikátorok, képernyőolvasó kompatibilitás. Kerülje a mozgó elemeket, ha lehetséges, vagy adjon opciót a kikapcsolásra. Tesztelje eszközzel, mint a Lighthouse vagy axe.
- **Teljesítmény Optimalizálás**: Minimalizálja a bundle méretet (JS/CSS), használjon lazy loading-ot, optimalizálja képeket és API hívásokat. Adatbázis lekérdezések optimalizálása indexekkel és caching-gel. Lokalizált tartalom caching-je a gyorsabb betöltés érdekében.
- **Többnyelvűség**: Az alkalmazás támogatja a lokalizációt (i18n). Frontendben i18next vagy hasonló könyvtár használata, backendben .NET IStringLocalizer. Nyelvek: pl. magyar, angol alapértelmezettként. Szövegek dinamikus betöltése a felhasználó kulturális beállítása alapján. RTL (right-to-left) nyelvek támogatása, ha szükséges.

## 6. Biztonság
- **Beépített Biztonság**: Használjon HTTPS-t, validálja a bemeneteket, védekezzen XSS, CSRF és injection támadások ellen. Adatbázis szinten használjon parameterized queries-t az SQL injection ellen.
- **Autentikáció/Autorizáció**: JWT tokenek .NET backendben, biztonságos tárolás frontendben. Adatbázis szerepkörök és engedélyek kezelése.
- **Adatvédelem**: Kövesse a GDPR elveket. Adatok titkosítása az adatbázisban (pl. Always Encrypted MS SQL Server 2022-ben). Lokalizált és akadálymentes adatok védelme kulturális és akadálymentes szempontból.

## 7. Fejlesztési Eszközök és Környezet
- **IDE**: Visual Studio (backend), VS Code (frontend).
- **Package Manager**: npm (JS), NuGet (.NET).
- **Adatbázis Eszközök**: SQL Server Management Studio (SSMS), Entity Framework Core Tools.
- **Lokalizáció Eszközök**: i18n könyvtárak (pl. i18next JS-hez, .NET Resource Files).
- **Akadálymentesítési Eszközök**: Lighthouse, axe-core, WAVE, billentyűzet navigációs tesztelők.
- **Containerizálás**: Docker minden mikroszervizhez, beleértve az MS SQL Server 2022 konténereket fejlesztési célra.
- **Verziókezelés**: Git.
- **CI/CD Eszközök**: GitHub Actions, Azure DevOps, integrálva az adatbázis migrációkkal, lokalizáció fájlokkal és a11y scannerekkel.

## 8. Adattárolás és Adatbázis Specifikus Követelmények
- **Adatbázis Motor**: MS SQL Server 2022 minden adattároláshoz. Használjon modernebb funkciókat, mint a JSON support, temporal tables és columnstore indexek.
- **Migrációk**: Entity Framework Core használatával kezelje az adatbázis sémaváltozásokat. Minden migráció legyen verziókezelt és tesztelt.
- **Biztonság és Optimalizálás**: Connection string-ek biztonságos kezelése (pl. Azure Key Vault), lekérdezések optimalizálása, indexelés.
- **Backup és Recovery**: Automatizált backup-ok és recovery tervek a CI/CD pipeline-ban.

## 9. Többnyelvűség (Internationalization - i18n) Specifikus Követelmények
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

## 11. Ellenőrzőlista Kódgenerálás Előtt
Mielőtt bármilyen kódot generálnék, ellenőrizzem:
- A generált kód megfelel-e a SOLID elveknek?
- Van-e megfelelő tesztelhetőség (unit/integrációs tesztek, beleértve az adatbázist, i18n-t és a11y-t)?
- Karbantartható-e (dokumentált, moduláris)?
- Integrálható-e a CI/CD pipeline-ba, beleértve az adatbázis migrációkat, lokalizációt és a11y-t?
- Reszponzív és biztonságos-e?
- Mikroszerviz architektúrához illeszkedik-e, MS SQL Server 2022-vel kompatibilis, támogatja a többnyelvűséget és teljes akadálymentesítéssel rendelkezik?

Ez a dokumentum alapvető irányelv minden fejlesztési tevékenységhez. Bármilyen eltérés indokolt legyen és dokumentált.
