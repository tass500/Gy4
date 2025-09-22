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

## 2. Rendszerarchitektúra

### 2.1 Képernyő-öröklődési modell

A rendszer egy közös ős-képernyőből (BaseScreen) származtatott képernyőket használ, mindkét rétegben (frontend és backend) egységesen.

**Központi követelmények:**
- Minden képernyő típusnak a BaseScreen-ből kell származnia
- A BaseScreen tartalmazza az összes alappéldányt és viselkedést
- A módosítások a BaseScreen-ben automatikusan érvényesülnek az összes leszármazott képernyőn
- Különleges esetekben lehessen felülírni az örökölt viselkedést

**BaseScreen főbb komponensei:**
- Fejléc (fejlesztői környezetben látható, élesben elrejtett)
- Navigációs menü
- Lábléc
- Betöltési állapot kezelése
- Hibaüzenetek kezelése
- Nyelvi váltás

### 2.2 Frontend implementáció

```typescript
// Példa a BaseScreen osztályra
abstract class BaseScreen {
  // Közös állapotok
  protected loading: boolean = false;
  protected error: string | null = null;
  
  // Kötelezően implementálandó metódusok
  abstract renderContent(): JSX.Element;
  
  // Közös metódusok
  protected showLoading(): void {
    this.loading = true;
    this.error = null;
  }
  
  protected handleError(error: Error): void {
    this.error = error.message;
    this.loading = false;
  }
  
  // Sablon metódus
  public render(): JSX.Element {
    return (
      <div className="base-screen">
        {this.renderHeader()}
        <main>
          {this.loading && <LoadingSpinner />}
          {this.error && <ErrorMessage message={this.error} />}
          {!this.loading && !this.error && this.renderContent()}
        </main>
        {this.renderFooter()}
      </div>
    );
  }
}
```

### 2.3 Backend implementáció

```csharp
// Példa a BaseScreen modellre
public abstract class BaseScreenModel
{
    // Közös mezők
    public string Title { get; set; }
    public bool IsLoading { get; set; }
    public string ErrorMessage { get; set; }
    public string CurrentLanguage { get; set; }
    
    // Közös metódusok
    protected void Initialize(string title)
    {
        this.Title = title;
        this.IsLoading = false;
        this.CurrentLanguage = "hu"; // Alapértelmezett nyelv
    }
    
    protected void SetError(string message)
    {
        this.ErrorMessage = message;
        this.IsLoading = false;
    }
}
```

## 3. Funkcionalitás (Kezdeti Fázis)

### 3.1 Alapképernyő (BaseScreen)
- **Központi vezérlés**: Minden képernyő alapja
- **Kötelező elemek**: 
  - Fejléc (alkalmazás neve, nyelvválasztó)
  - Betöltési állapot indikátor
  - Hibaüzelek megjelenítése
  - Navigációs menü
  - Lábléc (verziószám, copyright)
- **Kötelező funkcionalitások**: 
  - Nyelvváltás kezelése
  - Betöltési állapot kezelése
  - Hibaüzenetek kezelése
  - Navigáció kezelése

### 3.2 Üdvözlő Képernyő (HomeScreen)
- **Öröklés**: BaseScreen-ből származik
- **Egyedi tartalom**:
  - Üdvözlő üzenet a modulról
  - Interaktív gomb mikroszerviz híváshoz
  - Dinamikus üdvözlő üzenet megjelenítése
- **Kötelező felülírások**:
  - renderContent() - az egyedi tartalom megjelenítése
  - onInit() - adatok betöltése indításkor

### 3.3 Képernytípusok és öröklődési modell

Minden új képernyő a BaseScreen-ből fog származni, és csak az egyedi funkcionalitást kell implementálnia. Az alábbi alaptípusok állnak rendelkezésre a gyakori felhasználói felületi minták megvalósításához:

#### 3.3.1 Szerkesztő Képernyő (EditScreen)
- **Cél**: Adatbeviteli űrlapok megjelenítése és kezelése
- **Öröklés**: BaseScreen → EditScreen
- **Főbb jellemzők**:
  - Űrlap mezők dinamikus betöltése és validálása
  - Mentés, visszaállítás, megszakítás gombok
  - Automatikus mentés tervezett időközönként (opcionális)
  - Mezőszintű validáció és hibaüzenetek
  - Függő mezők kezelése

```typescript
abstract class EditScreen<T> extends BaseScreen {
  protected formData: T;
  protected isNew: boolean = true;
  protected validationErrors: Record<keyof T, string>;
  
  abstract getDefaultValues(): T;
  abstract validateForm(): boolean;
  abstract saveData(): Promise<boolean>;
  
  protected initializeForm(): void {
    this.formData = this.getDefaultValues();
    this.validationErrors = {} as Record<keyof T, string>;
  }
  
  protected handleInputChange(field: keyof T, value: any): void {
    this.formData[field] = value;
    // Automatikus validáció a mező módosításakor
    this.validateField(field);
  }
}
```

#### 3.3.2 Adatnézeti Képernyő (GridViewScreen)
- **Cél**: Adatok táblázatos megjelenítése és kezelése
- **Öröklés**: BaseScreen → GridViewScreen
- **Főbb jellemzők**:
  - Oldaltördelés és rendezés
  - Oszlopszűrés és keresés
  - Soronkénti műveletek (szerkesztés, törlés)
  - Exportálás (CSV, Excel, PDF)
  - Változások nyomon követése

```typescript
import { BaseScreen } from './BaseScreen';

/**
 * Constants for grid configuration
 */
const GRID_DEFAULTS = {
  PAGE_SIZE: 20,
  DEFAULT_SORT_COLUMN: 'id',
  DEFAULT_SORT_DIRECTION: 'asc' as const
} as const;

/**
 * Enum for sort direction
 */
enum SortDirection {
  ASC = 'asc',
  DESC = 'desc'
}

/**
 * Text alignment options for grid columns
 */
enum TextAlignment {
  LEFT = 'left',
  CENTER = 'center',
  RIGHT = 'right',
  JUSTIFY = 'justify'
}

/**
 * Interface for grid column definitions
 */
interface GridColumnDef<T> {
  /** The field name from the data object */
  field: keyof T;
  /** Display name for the column header */
  headerName: string;
  /** Whether the column is sortable */
  sortable?: boolean;
  /** Whether the column is filterable */
  filterable?: boolean;
  /** Width of the column in pixels */
  width?: number;
  /** Text alignment within the column */
  align?: TextAlignment;
}

/**
 * Abstract base class for grid view screens
 */
abstract class GridViewScreen<T> extends BaseScreen {
  // Grid data and state
  protected data: T[] = [];
  protected currentPage: number = 1;
  protected pageSize: number = GRID_DEFAULTS.PAGE_SIZE;
  protected sortColumn: string = GRID_DEFAULTS.DEFAULT_SORT_COLUMN;
  protected sortDirection: SortDirection = GRID_DEFAULTS.DEFAULT_SORT_DIRECTION;
  protected filters: Record<string, any> = {};
  
  abstract loadData(): Promise<void>;
  abstract getColumns(): GridColumnDef<T>[];
  
  /**
   * Resets the grid to its initial state
   */
  protected resetGrid(): void {
    this.currentPage = 1;
    this.sortColumn = GRID_DEFAULTS.DEFAULT_SORT_COLUMN;
    this.sortDirection = GRID_DEFAULTS.DEFAULT_SORT_DIRECTION;
    this.filters = {};
    this.loadData();
  }
  
  /**
   * Applies filters and reloads data
   * @param newFilters - New filter values to apply
   */
  protected applyFilters(newFilters: Record<string, any>): void {
    this.filters = { ...this.filters, ...newFilters };
    this.currentPage = 1; // Reset to first page when filters change
    this.loadData();
  }
  
  /**
   * Toggles the sort direction
   * @returns The new sort direction
   */
  protected toggleSortDirection(): SortDirection {
    return this.sortDirection === SortDirection.ASC 
      ? SortDirection.DESC 
      : SortDirection.ASC;
  }
}
```

#### 3.3.3 Kereső Képernyő (SearchDialog)
- **Cél**: Adatok szűrése és kiválasztása egy felugró ablakban
- **Öröklés**: BaseScreen → SearchDialog
- **Főbb jellemzők**:
  - Többszörös kiválasztás támogatása
  - Speciális szűrési lehetőségek
  - Előnézet a kiválasztott elemekről
  - Visszatérési érték a szülő képernyőnek

```typescript
// Search dialog configuration constants
const SEARCH_DIALOG_DEFAULTS = {
  MULTI_SELECT: false,
  PAGE_SIZE: 10,
  MIN_SEARCH_LENGTH: 3
} as const;

/**
 * Search dialog component for selecting items from a list
 */
class SearchDialog<T> extends BaseScreen {
  private resolve: (value: T[] | null) => void;
  private selectedItems: T[] = [];
  
  constructor(
    private readonly title: string,
    private readonly columns: GridColumnDef<T>[],
    private readonly dataLoader: (filters: any) => Promise<T[]>,
    private readonly multiSelect: boolean = SEARCH_DIALOG_DEFAULTS.MULTI_SELECT,
    private readonly pageSize: number = SEARCH_DIALOG_DEFAULTS.PAGE_SIZE,
    private readonly minSearchLength: number = SEARCH_DIALOG_DEFAULTS.MIN_SEARCH_LENGTH
  ) {
    super();
  }
  
  public show(): Promise<T[] | null> {
    return new Promise((resolve) => {
      this.resolve = resolve;
      // Megjelenítés logika
    });
  }
  
  protected handleSelect(): void {
    this.resolve(this.selectedItems);
    this.close();
  }
  
  protected handleCancel(): void {
    this.resolve(null);
    this.close();
  }
}
```

#### 3.3.4 Speciális Képernytípusok

##### 3.3.4.1 Mester-Detail Képernyő (MasterDetailScreen)
- **Leírás**: Fő lista és a kiválasztott elem részleteinek együttes megjelenítése
- **Használati esetek**: Terméklista és termék részletek, Rendelés és tétellista

##### 3.3.4.2 Lépésről-lépésre Végrehajtás (WizardScreen)
- **Leírás**: Többlépéses folyamatok vezérlése
- **Használati esetek**: Új rendelés létrehozása, Regisztrációs folyamat

### 3.4 Jövőbeli üzleti képernyők tervezete
- Raktárkezelő Képernyő (WarehouseScreen) - GridViewScreen-ből származik
- Csomagküldő Képernyő (ShippingScreen) - WizardScreen-ből származik
- Számlázó Képernyő (BillingScreen) - MasterDetailScreen-ből származik
- Be-/Kivételezési Képernyő (InventoryScreen) - GridViewScreen-ből származik

## 4. Mikroszerviz Specifikáció (Üdvözlő Funkció)
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

## 5. Nem Funkcionalitás Követelmények
- **Teljesítmény**: Gyors betöltés, optimalizált API hívások.
- **Biztonság**: HTTPS, input validáció, SQL injection védelem.
- **Skálázhatóság**: Mikroszerviz architektúra, hogy könnyen bővíthető legyen a későbbi funkcionalitásokkal.
- **Kompatibilitás**: Modern böngészők, mobil eszközök.

## 6. Architektúra és Technológia
- **Mikroszerviz Alapú Felépítés**: Külön mikroszervizek a frontend (JS/HTML/CSS), backend (C# .NET), adatbázis (MS SQL Server 2022) és egyéb szolgáltatások (lokalizáció, akadálymentesítés) számára.
- **Frontend**: ES6+ JavaScript, CSS Grid/Flexbox, i18next i18n könyvtár.
- **Backend**: C# .NET Core/6+, Entity Framework Core (EF Core) az adatbázis eléréséhez.
- **Adattárolás**: MS SQL Server 2022, database-per-service minta.
- **Többnyelvűség (i18n)**: JSON fájlok (hu.json, en.json) frontendben, .NET IStringLocalizer backendben.
- **Akadálymentesítés (a11y)**: ARIA címkék, billentyűzet navigáció, Lighthouse/axe-core tesztelés.

## 7. Fejlesztési Szakaszok
- **1. Fázis**: Üdvözlő oldal fejlesztése és tesztelése.
- **2. Fázis**: Raktári funkciók implementálása (későbbiekben részletezendő).
- **3. Fázis**: Csomagküldés funkciók.
- **4. Fázis**: Számlázás és be-/kivételezés.

## 8. Tesztelhetőség
- **Unit Tesztek**: Minden komponens tesztelve (Jest/Mocha JS-hez, xUnit .NET-hez).
- **Integrációs Tesztek**: Adatbázis interakciók tesztelése in-memory SQL Serverrel.
- **E2E Tesztek**: Teljes folyamatok (Selenium/Cypress).
- **Teljesítmény Tesztek**: Load testing.
- **Automatizált Teszt Futtatás**: Minden commit/pull request előtt, sikertelen build blokkolás.

## 9. CI/CD Folyamatok
- **CI**: Automatizált build (GitHub Actions), linting, statikus analízis, teszt futtatás.
- **CD**: Deploy staging/production-ba, rollback mechanizmus.
- **Monitoring**: Logging (Serilog .NET, Winston JS), Application Insights.

## 10. Kockázatok és Korlátok
- **Kockázatok**: Adatbázis teljesítmény, i18n fordítások késedelme.
- **Korlátok**: Kezdetben csak üdvözlő oldal, teljes funkcionalitás későbbi fázisokban.

## 11. Ellenőrzőlista Implementálás Előtt
- Megfelel-e a SOLID elveknek?
- Tesztelhetőség biztosítva?
- Karbantartható és dokumentált?
- CI/CD integrálható?
- Reszponzív és biztonságos?
- MS SQL Server 2022, i18n, a11y támogatott?

Ez a dokumentum alapvető irányelv. Bármilyen eltérés indokolt legyen és dokumentált.
