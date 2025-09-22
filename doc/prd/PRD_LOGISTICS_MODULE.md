# Project Requirement Document (PRD) - Logisztikai Modul

## Áttekintés
Ez a dokumentum meghatározza a követelményeket egy modern, reszponzív logisztikai modul fejlesztésére, amely mikroszerviz architektúrára épül. A modul célja a raktári funkciók, csomagküldés, számlázás, valamint be- és kivételezés kezelése. A fejlesztés során kiemelt hangsúlyt kell fektetni a karbantarthatóságra, tesztelhetőségre és CI/CD folyamatokra, összhangban a CODING_REQUIREMENTS.md dokumentumban leírtakkal. Az adatok tárolása MS SQL Server 2022-ben történik, az alkalmazás támogatja a többnyelvűséget (i18n), és teljes akadálymentesítéssel (a11y) rendelkezik.

A projekt kezdeti fázisa egy egyszerű üdvözlő oldal létrehozása, amely bemutatja a modult. A további funkcionalitásokat (raktári funkciók, csomagküldés, számlázás, be-/kivételezés) a későbbiekben fogjuk megadni és implementálni.

## 1. Projekt Célok és Hatáskör
- **Cél**: Egy teljes logisztikai modul fejlesztése, amely segíti a vállalatok raktári műveleteit, csomagküldését, számlázását és anyagmozgatását.
- **Hatáskör**:
  - Üdvözlő oldal: Egyszerű, reszponzív oldal, amely üdvözli a felhasználót és röviden bemutatja a modult.
  - Jövőbeli Funkcionalitások (későbbiekben bővítendők):
    - Raktári funkciók: Készletkezelés, termékek nyomonkövetése.
    - Csomagküldés: Csomagok létrehozása, nyomonkövetése, szállítási címkék generálása.
    - Számlázás: Automatizált számlák generálása és kezelése.
    - Be-/Kivételezés: Termékek be- és kivételezésének rögzítése, raktárkészlet frissítése.
- **Felhasználók**: Logisztikai dolgozók, raktárvezetők, adminisztrátorok.

## 2. Funkcionalitás (Kezdeti Fázis)
- **Üdvözlő Oldal**:
  - Egyszerű, reszponzív design mobil-először megközelítéssel.
  - Üdvözlő üzenet a modulról.
  - Interaktív gomb, amely mikroszerviz hívást indít és megjeleníti az üdvözlő üzenetet.
  - A gombnak láthatónak kell lennie és könnyen elérhetőnek a felhasználók számára.
  - A mikroszerviz hívás aszinkron módon történik, és a válaszban kapott üzenet jelenik meg a felhasználónak.
  - Hiba esetén felhasználóbarát hibaüzenet megjelenítése.
  - Navigációs menü (készülék a későbbi oldalakra, de kezdetben csak dekoratív).
  - Alapvető i18n támogatás (pl. magyar és angol nyelv).
  - WCAG 2.1 AA szintű akadálymentesítés (ARIA címkék, billentyűzet navigáció, színkontraszt, fókuszkezelés).

## 3. Mikroszerviz Specifikáció (Üdvözlő Funkció)
- **Végpont**: `/api/greeting`
- **Metódus**: GET
- **Válasz formátuma**: JSON
  ```json
  {
    "message": "Üdvözöljük a Logisztikai Modulban!"
  }
  ```
- **Hibakezelés**: 
  - 200 OK: Sikeres kérés, visszaadja az üdvözlő üzenetet
  - 500 Internal Server Error: Szerverhiba esetén
  - 503 Service Unavailable: Ha a szolgáltatás nem elérhető
- **Időtúllépés**: 5 másodperc
- **Újrapróbálkozás**: 2 alkalommal történik automatikus újrapróbálkozás hiba esetén

## 4. Nem Funkcionalitás Követelmények
- **Teljesítmény**: Gyors betöltés, optimalizált API hívások.
- **Biztonság**: HTTPS, input validáció, SQL injection védelem.
- **Skálázhatóság**: Mikroszerviz architektúra, hogy könnyen bővíthető legyen a későbbi funkcionalitásokkal.
- **Kompatibilitás**: Modern böngészők, mobil eszközök.

## 5. Architektúra és Technológia
- **Mikroszerviz Alapú Felépítés**: Külön mikroszervizek a frontend (JS/HTML/CSS), backend (C# .NET), adatbázis (MS SQL Server 2022) és egyéb szolgáltatások (lokalizáció, akadálymentesítés) számára.
- **Frontend**: ES6+ JavaScript, CSS Grid/Flexbox, i18next i18n könyvtár.
- **Backend**: C# .NET Core/6+, Entity Framework Core (EF Core) az adatbázis eléréséhez.
- **Adattárolás**: MS SQL Server 2022, database-per-service minta.
- **Többnyelvűség (i18n)**: JSON fájlok (hu.json, en.json) frontendben, .NET IStringLocalizer backendben.
- **Akadálymentesítés (a11y)**: ARIA címkék, billentyűzet navigáció, Lighthouse/axe-core tesztelés.

## 6. Fejlesztési Szakaszok
- **1. Fázis**: Üdvözlő oldal fejlesztése és tesztelése.
- **2. Fázis**: Raktári funkciók implementálása (későbbiekben részletezendő).
- **3. Fázis**: Csomagküldés funkciók.
- **4. Fázis**: Számlázás és be-/kivételezés.

## 7. Tesztelhetőség
- **Unit Tesztek**: Minden komponens tesztelve (Jest/Mocha JS-hez, xUnit .NET-hez).
- **Integrációs Tesztek**: Adatbázis interakciók tesztelése in-memory SQL Serverrel.
- **E2E Tesztek**: Teljes folyamatok (Selenium/Cypress).
- **Teljesítmény Tesztek**: Load testing.
- **Automatizált Teszt Futtatás**: Minden commit/pull request előtt, sikertelen build blokkolás.

## 8. CI/CD Folyamatok
- **CI**: Automatizált build (GitHub Actions), linting, statikus analízis, teszt futtatás.
- **CD**: Deploy staging/production-ba, rollback mechanizmus.
- **Monitoring**: Logging (Serilog .NET, Winston JS), Application Insights.

## 9. Kockázatok és Korlátok
- **Kockázatok**: Adatbázis teljesítmény, i18n fordítások késedelme.
- **Korlátok**: Kezdetben csak üdvözlő oldal, teljes funkcionalitás későbbi fázisokban.

## 10. Ellenőrzőlista Implementálás Előtt
- Megfelel-e a SOLID elveknek?
- Tesztelhetőség biztosítva?
- Karbantartható és dokumentált?
- CI/CD integrálható?
- Reszponzív és biztonságos?
- MS SQL Server 2022, i18n, a11y támogatott?

Ez a dokumentum alapvető irányelv. Bármilyen eltérés indokolt legyen és dokumentált.
