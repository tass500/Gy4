# Project Requirement Document (PRD) - Logisztikai Modul

## Áttekintés
Ez a dokumentum meghatározza a követelményeket egy modern, reszponzív logisztikai modul fejlesztésére, amely mikroszerviz architektúrára épül és Kubernetes-kompatibilis környezetben fut. A modul célja a raktári funkciók, csomagküldés, számlázás, valamint be- és kivételezés kezelése. A fejlesztés során kiemelt hangsúlyt kell fektetni a karbantarthatóságra, tesztelhetőségre és CI/CD folyamatokra, összhangban a CODING_REQUIREMENTS.md dokumentumban leírtakkal.

### Főbb technológiák
- **Frontend**: TypeScript, Angular (lásd: CODING_REQUIREMENTS.md - Frontend Design Pattern-ek)
- **Backend**: .NET Core 6.0+ (C#)
- **Adatbázis**: MS SQL Server 2022
- **Üzenetküldés**: Apache Kafka
- **Stream feldolgozás**: Apache Flink
- **Konténerizáció**: Docker
- **Orchestráció**: Kubernetes
- **Monitorozás**: Prometheus + Grafana
- **Naplózás**: ELK Stack (Elasticsearch, Logstash, Kibana)

### Architekturális elvek
- **Mikroszolgáltatás architektúra**
- **Event-driven design**
- **Domain-Driven Design (DDD)**
- **CQRS és Event Sourcing** kiválasztott modulokban
- **Többnyelvűség (i18n) és akadálymentesítés (a11y)** támogatás
- **CI/CD folyamatok** automatikus teszteléssel

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

### 2.0 Mikroszolgáltatás Struktúra

A rendszer a következő főbb szolgáltatásokból fog állni:

1. **API Gateway** (Ocelot/YARP)
   - Bejövő kérések útválasztása
   - Hitelesítés és engedélyezés
   - Terheléselosztás
   - Gyorsítótárazás

2. **Felhasználói Felület** (Angular)
   - Reszponzív webes felület
   - Progresszív Web App (PWA) támogatás
   - Offline működés támogatása

3. **Logisztikai Alapszolgáltatás** (.NET Core)
   - Alapvető logisztikai műveletek
   - Adatintegritás biztosítása
   - Tranzakciókezelés

4. **Raktárkezelő Szolgáltatás** (.NET Core)
   - Készletnyilvántartás
   - Raktárhelyek kezelése
   - Készletmozgások nyomon követése

5. **Szállítási Szolgáltatás** (.NET Core)
   - Csomagküldés kezelése
   - Szállítási címkék generálása
   - Szállítási státusz követése

6. **Számlázási Szolgáltatás** (.NET Core)
   - Számlák generálása
   - Fizetések kezelése
   - Kimutatások készítése

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

A frontend Angular keretrendszerre épül, és a következő tervezési mintákat követi:

#### 2.2.1 Alapvető szerkezet

```typescript
// Példa a BaseScreen osztályra (Angular komponens)
@Component({
  template: `
    <div class="base-screen">
      <app-header [title]="title" [loading]="loading"></app-header>
      
      <main>
        <ng-container *ngIf="loading">
          <app-loading-spinner></app-loading-spinner>
        </ng-container>
        
        <app-error-message *ngIf="error" [message]="error"></app-error-message>
        
        <div *ngIf="!loading && !error" class="content">
          <ng-content></ng-content>
        </div>
      </main>
      
      <app-footer></app-footer>
    </div>
  `,
  styleUrls: ['./base-screen.component.scss']
})
export abstract class BaseScreenComponent implements OnInit, OnDestroy {
  @Input() title: string = '';
  loading: boolean = false;
  error: string | null = null;
  
  // Dependency injection a szükséges szolgáltatásokhoz
  constructor(
    protected translate: TranslateService,
    protected errorHandler: ErrorHandlerService,
    protected loadingService: LoadingService
  ) {}
  
  // Életciklus metódusok
  ngOnInit(): void {
    this.initialize();
  }
  
  ngOnDestroy(): void {
    this.cleanup();
  }
  
  // Kötelezően implementálandó metódusok
  protected abstract initialize(): void;
  protected abstract cleanup(): void;
  
  // Közös metódusok
  protected setLoading(state: boolean): void {
    this.loading = state;
    this.loadingService.setLoading(state);
  }
  
  protected handleError(error: any): void {
    this.error = this.errorHandler.handleError(error);
    this.loading = false;
  }
  
  // Nyelvváltás kezelése
  protected changeLanguage(lang: string): void {
    this.translate.use(lang);
    localStorage.setItem('userLanguage', lang);
  }
}
```

#### 2.2.2 State Management (NgRx)

```typescript
// Állapotkezelés NgRx-el
@Injectable()
export class ScreenEffects {
  constructor(
    private actions$: Actions,
    private screenService: ScreenService
  ) {}

  loadScreenData$ = createEffect(() => 
    this.actions$.pipe(
      ofType(ScreenActions.loadScreenData),
      switchMap(({ screenId }) => 
        this.screenService.getScreenData(screenId).pipe(
          map(data => ScreenActions.loadScreenDataSuccess({ data })),
          catchError(error => of(ScreenActions.loadScreenDataFailure({ error })))
        )
      )
    )
  );
}
```
```

### 2.3 Backend implementáció (.NET Core)

#### 2.3.1 Alapvető szerkezet

```csharp
// Alap modell a képernyőkhöz
public abstract class BaseScreenModel
{
    [JsonIgnore]
    public string RequestId { get; set; } = Guid.NewGuid().ToString();
    
    [JsonIgnore]
    public bool IsLoading { get; protected set; }
    
    [JsonIgnore]
    public string ErrorMessage { get; protected set; }
    
    public string CurrentLanguage { get; set; } = "hu";
    
    [JsonIgnore]
    public Dictionary<string, string> ValidationErrors { get; } = new();
    
    protected virtual void Initialize()
    {
        this.IsLoading = false;
        this.ErrorMessage = string.Empty;
    }
    
    protected void SetError(string message, Exception ex = null)
    {
        this.ErrorMessage = message;
        this.IsLoading = false;
        // Logolás stb.
    }
    
    public virtual void ClearValidation()
    {
        this.ValidationErrors.Clear();
    }
}

// Alap kontroller osztály
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public abstract class BaseApiController : ControllerBase
{
    protected readonly ILogger _logger;
    protected readonly IMediator _mediator;
    
    protected BaseApiController(ILogger logger, IMediator mediator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
    
    protected async Task<ActionResult<T>> HandleQuery<T>(IRequest<T> query)
    {
        try
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error occurred");
            return BadRequest(ex.Errors);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found");
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing your request");
            return StatusCode(500, "Internal server error");
        }
    }
}
```

#### 2.3.2 CQRS implementáció

```csharp
// Példa Command és CommandHandler implementációra
public class CreateOrderCommand : IRequest<OrderDto>
{
    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderItemDto> Items { get; set; }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<CreateOrderCommandHandler> _logger;
    
    public CreateOrderCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        ILogger<CreateOrderCommandHandler> logger)
    {
        _context = context;
        _currentUserService = currentUserService;
        _logger = logger;
    }
    
    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            OrderNumber = request.OrderNumber,
            OrderDate = request.OrderDate,
            Status = OrderStatus.Created,
            CreatedBy = _currentUserService.UserId,
            CreatedAt = DateTime.UtcNow
        };
        
        // Tovább feldolgozás...
        
        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Order {OrderId} created successfully", order.Id);
        
        return new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            OrderDate = order.OrderDate,
            Status = order.Status.ToString()
        };
    }
}
```
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
