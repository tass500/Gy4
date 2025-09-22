# Project Requirement Document - Logistics Service

## 1. Áttekintés
A Logistics Service a logisztikai folyamatok központi koordinálásáért felel, integrálva a raktárkezelési, szállítási és számlázási szolgáltatásokat. Ez a szolgáltatás biztosítja a teljes rendelés teljesítési folyamatának vezérlését és nyomon követését.

## 2. Funkcionális követelmények

### 2.1 Főbb funkciók
- [ ] Rendelések feldolgozásának koordinálása
- [ ] Raktári és szállítási folyamatok összehangolása
- [ ] Valós idejű rendelés követés
- [ ] Kivételek kezelése és riasztások
- [ ] Teljesítmény és hatékonyság elemzése

### 2.2 Felhasználói történetek
- **Mint rendeléskezelő szeretném követni egy rendelés állapotát, hogy informálhassam a vásárlót**
  - Rendelés azonosítójának megadása
  - Valós idejű állapot megtekintése
  - Várható szállítási idő becslése
  - Kivételek kezelése, ha vannak

## 3. Nem funkcionális követelmények

### 3.1 Teljesítmény
- Válaszidő: < 100ms
- 1,000+ rendelés/óra feldolgozása
- Valós idejű állapotfrissítések

### 3.2 Megbízhatóság
- 99.9% elérhetőség
- Adatkonzisztencia biztosítása
- Tranzakciós integritás

## 4. API specifikáció

### 4.1 Végpontok
- `POST /api/orders` - Új rendelés feldolgozásának indítása
- `GET /api/orders/{id}` - Rendelés állapotának lekérdezése
- `GET /api/orders/{id}/timeline` - Rendelés idővonalának lekérdezése
- `POST /api/orders/{id}/cancel` - Rendelés törlése
- `GET /api/orders/customer/{customerId}` - Ügyfél rendeléseinek listázása

## 5. Állapotgép

### 5.1 Rendelés állapotai
- **Created**: Rendelés létrehozva
- **Validated**: Rendelés ellenőrizve
- **Processing**: Feldolgozás alatt
- **Picking**: Kivételezés folyamatban
- **Packed**: Csomagolva
- **Shipped**: Kiszállítás alatt
- **Delivered**: Kiszállítva
- **Cancelled**: Törölve
- **Returned**: Visszaküldve

## 6. Integrációk
- **Order Service**: Rendelések fogadása
- **Warehouse Service**: Készlet ellenőrzés és kivételezés
- **Shipping Service**: Szállítási információk frissítése
- **Billing Service**: Számla generálás
- **Notification Service**: Állapotváltozások értesítése

## 7. Figyelés és naplózás
- Rendelés életciklus nyomon követése
- Teljesítmény metrikák gyűjtése
- Rendellenességek észlelése
- Riasztások beállítása kritikus eseményekre

## 8. Telepítés és üzemeltetés
### 8.1 Követelmények
- .NET 8.0+
- MS SQL Server 2022+ (tranzakciós adatokhoz)
- Redis (gyorsítótárazáshoz)
- Message Queue (RabbitMQ/Azure Service Bus)

## 9. Ismert korlátok
- Korlátozott párhuzamos feldolgozás
- Késleltetett állapotfrissítések integrációktól függően
- Korlátozott számú egyéni munkafolyamat kezdetben

## 10. Jövőbeli fejlesztések
- Mesterséges intelligencia alapú előrejelzések
- Automatikus útvonaloptimalizálás
- Valós idejű követési térkép
- Önkiszolgáló visszaküldési portál
- Haladó analitikák és jelentések
