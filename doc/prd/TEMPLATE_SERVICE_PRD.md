# Project Requirement Document - [Service Name] Service

## 1. Áttekintés
A [Service Name] szolgáltatás felelőssége [rövid leírás a szolgáltatás céljáról és főbb funkcióiról].

## 2. Funkcionális követelmények

### 2.1 Főbb funkciók
- [ ] Funkció 1
- [ ] Funkció 2
- [ ] Funkció 3

### 2.2 Felhasználói történetek
- **Mint [szerepkör] szeretnék [cselekvés], hogy [érték]**
  - Részletes leírás
  - Elfogadási feltételek

## 3. Nem funkcionális követelmények

### 3.1 Teljesítmény
- Válaszidő: < 200ms
- Egyidejű felhasználók száma: 1000+
- Adatmennyiség: X rekord/nap

### 3.2 Biztonság
- Hitelesítés: [pl. JWT, OAuth2]
- Engedélyezés: [pl. Role-based access control]
- Adatvédelem: [pl. GDPR megfelelőség]

### 3.3 Skálázhatóság
- Horizontális skálázhatóság
- Terheléselosztás

## 4. API specifikáció

### 4.1 Végpontok
- `GET /api/[resource]` - Összes [erőforrás] lekérdezése
- `GET /api/[resource]/{id}` - Egy [erőforrás] lekérdezése ID alapján
- `POST /api/[resource]` - Új [erőforrás] létrehozása
- `PUT /api/[resource]/{id}` - [Erőforrás] frissítése
- `DELETE /api/[resource]/{id}` - [Erőforrás] törlése

## 5. Adatmodell

### 5.1 Főbb entitások
- **Entitás1**
  - Mező1: Típus (pl. string, int, DateTime)
  - Mező2: Típus

## 6. Integrációk
- **Integrált szolgáltatások**:
  - [Másik szolgáltatás neve] - [Integráció típusa] - [Leírás]

## 7. Figyelés és naplózás
- Kritikus metrikák:
  - Kérések száma/perc
  - Hibák száma
  - Válaszidők
- Naplózandó események:
  - Fontos üzleti események
  - Biztonsági események

## 8. Tesztelés
### 8.1 Tesztesetek
- Egységtesztek
- Integrációs tesztek
- Teljesítménytesztek

## 9. Telepítés és üzemeltetés
### 9.1 Követelmények
- Minimális hardver követelmények
- Függőségek
### 9.2 Konfiguráció
- Környezeti változók
- Alkalmazás beállítások

## 10. Ismert korlátok
- [Ismert korlátok vagy hiányzó funkciók leírása]

## 11. Jövőbeli fejlesztések
- [Tervezett jövőbeli fejlesztések listája]

## 12. Jegyzetek
- [Egyéb fontos megjegyzések]
