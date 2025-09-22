# Project Requirement Document - Shipping Service

## 1. Áttekintés
A Shipping Service felelős a szállítási folyamatok kezeléséért, beleértve a szállítmányok létrehozását, nyomon követését és a szállítmányozókkal való integrációt.

## 2. Funkcionális követelmények

### 2.1 Főbb funkciók
- [ ] Szállítmányok létrehozása és kezelése
- [ ] Szállítási címek validálása
- [ ] Szállítmányozók integrációja (GLS, DHL, stb.)
- [ ] Követési információk lekérdezése
- [ ] Szállítási költségek kalkulálása
- [ ] Címkék és szállítási dokumentumok generálása

### 2.2 Felhasználói történetek
- **Mint rendeléskezelő szeretnék szállítmányt létrehozni, hogy a vevő megkaphassa a rendelését**
  - Rendelés kiválasztása
  - Szállítási mód kiválasztása
  - Szállítási adatok megadása
  - Címke nyomtatása

## 3. Nem funkcionális követelmények

### 3.1 Teljesítmény
- Válaszidő: < 200ms
- 1,000+ szállítmány/nap kezelése
- Valós idejű követési információk frissítése

### 3.2 Biztonság
- Szállítási adatok titkosítása
- API kulcsok biztonságos kezelése
- Minden szállítási művelet naplózása

## 4. API specifikáció

### 4.1 Végpontok
- `POST /api/shipments` - Új szállítmány létrehozása
- `GET /api/shipments/{id}` - Szállítmány adatainak lekérdezése
- `GET /api/shipments/{id}/tracking` - Követési információk lekérdezése
- `POST /api/shipments/calculate` - Szállítási költség kalkulálása
- `GET /api/shipments/{id}/label` - Szállítási címke generálása

## 5. Adatmodell

### 5.1 Főbb entitások
- **Shipment**
  - Id: Guid
  - OrderId: Guid
  - Carrier: string (pl. "GLS", "DHL")
  - TrackingNumber: string
  - Status: enum (Created, InTransit, Delivered, etc.)
  - CreatedAt: DateTime
  - UpdatedAt: DateTime

- **ShipmentItem**
  - Id: Guid
  - ShipmentId: Guid
  - ProductId: Guid
  - Quantity: int
  - Weight: decimal
  - Dimensions: string

## 6. Integrációk
- **Order Service**: Rendelések fogadása
- **Warehouse Service**: Készletfrissítések
- **Carrier APIs**: Szállítmányozókkal való kommunikáció
- **Notification Service**: Állapotváltozások értesítése

## 7. Figyelés és naplózás
- Szállítási állapotváltozások naplózása
- API hívások nyomon követése
- Hibák és kivételek figyelése

## 8. Telepítés és üzemeltetés
### 8.1 Követelmények
- .NET 8.0+
- MS SQL Server 2022+
- Redis gyorsítótárazáshoz
- Külső API kulcsok kezelése (Azure Key Vault)

## 9. Ismert korlátok
- Korlátozott számú szállítmányozó integráció kezdetben
- Nem minden ország támogatott
- Valós idejű követés függ a szállítmányozó API-jától

## 10. Jövőbeli fejlesztések
- Több szállítmányozó integrációja
- Haladó útvonaltervezés
- Szállítási előrejelzések
- Mobilalkalmazás a futárok számára
