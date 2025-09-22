# Project Requirement Document - Tenant Service

## 1. Áttekintés
A Tenant Service felelős a több-bérlős architektúra kezeléséért, beleértve a bérlők létrehozását, kezelését és konfigurálását. A szolgáltatás biztosítja a bérlői adatok elkülönítését és kezelését.

## 2. Funkcionális követelmények

### 2.1 Főbb funkciók
- [ ] Bérlők létrehozása, olvasása, frissítése, törlése (CRUD)
- [ ] Bérlői konfiguráció kezelése
- [ ] Bérlői állapot kezelése (aktív, inaktív, felfüggesztett)
- [ ] Bérlői kvóták és korlátok kezelése
- [ ] Bérlői adatok exportálása és importálása

### 2.2 Felhasználói történetek
- **Mint rendszergazda szeretnék új bérlőt regisztrálni, hogy hozzáférjenek a rendszerhez**
  - Kötelező adatok: bérlő neve, admin email, csomag típusa
  - Automatikus üdvözlő email küldése

## 3. Nem funkcionális követelmények

### 3.1 Teljesítmény
- Válaszidő: < 100ms
- Egyidejű bérlői műveletek kezelése: 100+
- Adatbázis teljesítmény optimalizálás több bérlő esetén

### 3.2 Biztonság
- Hitelesítés: JWT token
- Engedélyezés: Role-based access control (RBAC)
- Adatbiztonság: Bérlői adatok szigorú elkülönítése

## 4. API specifikáció

### 4.1 Végpontok
- `GET /api/tenants` - Összes bérlő listázása
- `GET /api/tenants/{id}` - Bérlő adatainak lekérdezése
- `POST /api/tenants` - Új bérlő létrehozása
- `PUT /api/tenants/{id}` - Bérlő adatainak frissítése
- `DELETE /api/tenants/{id}` - Bérlő törlése (soft delete)
- `GET /api/tenants/{id}/status` - Bérlő állapotának lekérdezése

## 5. Adatmodell

### 5.1 Főbb entitások
- **Tenant**
  - Id: Guid (Primary Key)
  - Name: string (Egyedi név)
  - Domain: string (Egyedi domain)
  - Status: enum (Active, Suspended, Inactive)
  - CreatedAt: DateTime
  - UpdatedAt: DateTime
  - Configuration: JSON (Bérlői konfiguráció)

## 6. Integrációk
- **Auth Service**: Bérlői hitelesítés és engedélyezés
- **Billing Service**: Előfizetések és számlázás kezelése
- **Notification Service**: Értesítések küldése bérlői műveletekről

## 7. Figyelés és naplózás
- Bérlői műveletek naplózása
- Kritérikus metrikák nyomon követése
- Riasztások beállítása fontos eseményekre

## 8. Telepítés és üzemeltetés
### 8.1 Követelmények
- .NET 8.0+
- MS SQL Server 2022+
- Redis gyorsítótárazáshoz

## 9. Ismert korlátok
- Maximális bérlőszám: 10,000 (konfigurálható)
- Bérlői adatok exportálása kötegelt feldolgozásban történik

## 10. Jövőbeli fejlesztések
- Több adatbázis támogatása bérlőnként
- Önkiszolgáló bérlői portál
- Speciális bérlői analitikák
