# Project Requirement Document - Billing Service

## 1. Áttekintés
A Billing Service felelős a számlázási folyamatok kezeléséért, beleértve a számlák generálását, fizetések feldolgozását és a pénzügyi jelentések készítését.

## 2. Funkcionális követelmények

### 2.1 Főbb funkciók
- [ ] Automatikus számlagenerálás
- [ ] Ismétlődő számlázás (subscription) kezelése
- [ ] Fizetési módok kezelése (bankkártya, átutalás, stb.)
- [ ] Adózási számítások
- [ ] Visszatérítések kezelése
- [ ] Pénzügyi jelentések generálása

### 2.2 Felhasználói történetek
- **Mint pénzügyi munkatárs szeretnék számlát generálni, hogy nyilvántartsam a bevételt**
  - Ügyfél kiválasztása
  - Szolgáltatások hozzáadása
  - Adók és kedvezmények alkalmazása
  - Számla előnézete és jóváhagyása
  - PDF generálás és küldés

## 3. Nem funkcionális követelmények

### 3.1 Teljesítmény
- Válaszidő: < 300ms
- 10,000+ számla/hó kezelése
- Párhuzamos számlagenerálás

### 3.2 Biztonság
- Pénzügyi adatok titkosítása
- PCI DSS megfelelőség
- Minden pénzügyi tranzakció naplózása
- Audit trail minden változtatáshoz

## 4. API specifikáció

### 4.1 Végpontok
- `POST /api/invoices` - Új számla létrehozása
- `GET /api/invoices/{id}` - Számla adatainak lekérdezése
- `POST /api/invoices/{id}/pay` - Fizetés rögzítése
- `GET /api/invoices/customer/{customerId}` - Ügyfél számláinak listázása
- `POST /api/invoices/{id}/refund` - Visszatérítés kezdeményezése

## 5. Adatmodell

### 5.1 Főbb entitások
- **Invoice**
  - Id: Guid
  - InvoiceNumber: string (egyedi)
  - CustomerId: Guid
  - IssueDate: DateTime
  - DueDate: DateTime
  - Status: enum (Draft, Sent, Paid, Overdue, Cancelled)
  - TotalAmount: decimal
  - TaxAmount: decimal
  - Currency: string

- **InvoiceLine**
  - Id: Guid
  - InvoiceId: Guid
  - Description: string
  - Quantity: decimal
  - UnitPrice: decimal
  - TaxRate: decimal
  - LineTotal: decimal

## 6. Integrációk
- **Payment Gateway**: Fizetések feldolgozása
- **Customer Service**: Ügyféladatok szinkronizálása
- **Tax Service**: Adószámítások
- **Notification Service**: Számlaértesítések küldése

## 7. Figyelés és naplózás
- Minden pénzügyi tranzakció naplózása
- Rendellenességek észlelése
- Jelentések generálása

## 8. Telepítés és üzemeltetés
### 8.1 Követelmények
- .NET 8.0+
- MS SQL Server 2022+ (tranzakciós adatokhoz)
- Redis (gyorsítótárazáshoz)
- Külső fizetési átjáró integráció

## 9. Ismert korlátok
- Korlátozott számú fizetési mód kezdetben
- Nem minden ország adószabálya támogatott
- Napi batch feldolgozás egyes jelentésekhez

## 10. Jövőbeli fejlesztések
- Több fizetési átjáró támogatása
- Automatikus követeléskezelés
- Haladó pénzügyi analitikák
- Többpénznámű számlázás
- Automatikus adóbevallás generálás
