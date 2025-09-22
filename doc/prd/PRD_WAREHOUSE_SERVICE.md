# Project Requirement Document - Warehouse Service

## 1. Áttekintés
A Warehouse Service felelős a raktári műveletek kezeléséért, beleértve a készletnyilvántartást, raktárhelyek kezelését, valamint a be- és kivételezési folyamatokat.

## 2. Funkcionális követelmények

### 2.1 Főbb funkciók
- [ ] Készletnyilvántartás (inventory management)
- [ ] Raktárhelyek kezelése (locations, zones)
- [ ] Bevételezés (goods receipt)
- [ ] Kivételezés (picking)
- [ ] Készletmozgások nyomon követése
- [ ] Készletkorrekciók kezelése

### 2.2 Felhasználói történetek
- **Mint raktáros szeretnék bevételezni új árut, hogy a készlet naprakész legyen**
  - Vonalkód beolvasása
  - Mennyiség és minőség ellenőrzése
  - Tárolási hely kijelölése

## 3. Nem funkcionális követelmények

### 3.1 Teljesítmény
- Válaszidő: < 150ms
- 10,000+ termék kezelése
- Valós idejű készletfrissítések

### 3.2 Biztonság
- Szerepköralapú hozzáférés-vezérlés
- Minden készletművelet naplózása
- Adatintegritás biztosítása tranzakciók segítségével

## 4. API specifikáció

### 4.1 Végpontok
- `GET /api/inventory` - Készlet lekérdezése
- `GET /api/inventory/{productId}` - Termék készletének lekérdezése
- `POST /api/inventory/receipt` - Bevételezés rögzítése
- `POST /api/inventory/pick` - Kivételezés kezdeményezése
- `GET /api/locations` - Raktárhelyek listázása

## 5. Adatmodell

### 5.1 Főbb entitások
- **Product**
  - Id: Guid
  - SKU: string
  - Name: string
  - Description: string

- **InventoryItem**
  - Id: Guid
  - ProductId: Guid
  - LocationId: Guid
  - Quantity: decimal
  - BatchNumber: string (opcionális)
  - ExpiryDate: DateTime? (opcionális)

## 6. Integrációk
- **Product Service**: Termékadatok szinkronizálása
- **Order Service**: Rendelések feldolgozása
- **Shipping Service**: Szállítási információk frissítése

## 7. Figyelés és naplózás
- Készletváltozások naplózása
- Raktári műveletek nyomon követése
- Késleltetések és szűk keresztmetszetek azonosítása

## 8. Telepítés és üzemeltetés
### 8.1 Követelmények
- .NET 8.0+
- MS SQL Server 2022+ (tranzakciós adatokhoz)
- Redis (gyorsítótárazáshoz)
- Message Queue (RabbitMQ/Azure Service Bus)

## 9. Ismert korlátok
- Maximális tranzakciószám/perc: 10,000
- Késleltetett frissítések nagy terhelés alatt

## 10. Jövőbeli fejlesztések
- Haladó raktározási stratégiák (FIFO, LIFO, FEFO)
- Automata raktározási javaslatok
- Mobilalkalmazás támogatás raktári műveletekhez
